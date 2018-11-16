namespace MyServer.Data.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    using MyServer.Data.Common.Models;

    public class AzureTableStorage<T> : IRepository<T>
        where T : BaseModel, new()
    {
        private readonly CloudTableClient client;

        private readonly string tableName;

        private ConcurrentDictionary<string, CloudTable> tables;

        public AzureTableStorage(IConfiguration configuration)
        {
            var account = CloudStorageAccount.Parse(configuration["TableConnectionString"]);
            this.client = account.CreateCloudTableClient();
            this.tableName = typeof(T).Name;
            this.tables = new ConcurrentDictionary<string, CloudTable>();
        }

        public async Task<object> AddAsync(T entity)
        {
            var table = await this.EnsureTable(this.tableName).ConfigureAwait(false);
            var insertOperation = TableOperation.Insert(entity);
            var result = await table.ExecuteAsync(insertOperation).ConfigureAwait(false);

            return result.Result;
        }

        public async Task<IEnumerable<T>> AddBatchAsync(IEnumerable<T> entities, BatchOperationOptions options)
        {
            var table = await this.EnsureTable(this.tableName).ConfigureAwait(false);
            options = options ?? new BatchOperationOptions();
            var tasks = new List<Task<IList<TableResult>>>();

            const int addBatchOperationLimit = 100;
            var entitiesOffset = 0;

            while (entitiesOffset < entities?.Count())
            {
                var entitiesToAdd = entities.Skip(entitiesOffset).Take(addBatchOperationLimit).ToList();
                entitiesOffset += entitiesToAdd.Count;

                Action<TableBatchOperation, ITableEntity> batchInsertOperation = null;
                switch (options.BatchInsertMethod)
                {
                    case BatchInsertMethod.Insert:
                        batchInsertOperation = (bo, entity) => bo.Insert(entity);
                        break;
                    case BatchInsertMethod.InsertOrReplace:
                        batchInsertOperation = (bo, entity) => bo.InsertOrReplace(entity);
                        break;
                    case BatchInsertMethod.InsertOrMerge:
                        batchInsertOperation = (bo, entity) => bo.InsertOrMerge(entity);
                        break;
                }

                var batchOperation = new TableBatchOperation();
                foreach (var entity in entitiesToAdd)
                {
                    batchInsertOperation(batchOperation, entity);
                }

                tasks.Add(table.ExecuteBatchAsync(batchOperation));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            return results.SelectMany(tableResults => tableResults, (tr, r) => r.Result as T);
        }

        public async Task<object> AddOrUpdateAsync(T entity)
        {
            var table = await this.EnsureTable(this.tableName).ConfigureAwait(false);
            var insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);
            var result = await table.ExecuteAsync(insertOrReplaceOperation).ConfigureAwait(false);

            return result.Result;
        }

        public async Task<object> DeleteAsync(T entity)
        {
            var table = await this.EnsureTable(this.tableName).ConfigureAwait(false);
            var deleteOperation = TableOperation.Delete(entity);
            var result = await table.ExecuteAsync(deleteOperation).ConfigureAwait(false);

            return result.Result;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var table = await this.EnsureTable(this.tableName).ConfigureAwait(false);

            TableContinuationToken token = null;
            var entities = new List<T>();
            do
            {
                var queryResult =
                    await table.ExecuteQuerySegmentedAsync(new TableQuery<T>(), token).ConfigureAwait(false);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            }
            while (token != null);

            return entities;
        }

        public async Task<T> GetAsync(string partitionKey, string rowKey)
        {
            var table = await this.EnsureTable(this.tableName).ConfigureAwait(false);
            var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var result = await table.ExecuteAsync(retrieveOperation).ConfigureAwait(false);

            return result.Result as T;
        }

        public async Task<IEnumerable<T>> QueryAsync(TableQuery<T> query)
        {
            var table = await this.EnsureTable(this.tableName).ConfigureAwait(false);
            var shouldConsiderTakeCount = query.TakeCount.HasValue;

            return shouldConsiderTakeCount
                       ? await this.QueryAsyncWithTakeCount(table, query).ConfigureAwait(false)
                       : await this.QueryAsync(table, query).ConfigureAwait(false);
        }

        public async Task<object> UpdateAsync(T entity)
        {
            var table = await this.EnsureTable(this.tableName).ConfigureAwait(false);
            var replaceOperation = TableOperation.Replace(entity);
            var result = await table.ExecuteAsync(replaceOperation).ConfigureAwait(false);

            return result.Result;
        }

        private async Task<CloudTable> EnsureTable(string tableLocal)
        {
            if (!this.tables.ContainsKey(tableLocal))
            {
                var table = this.client.GetTableReference(tableLocal);
                await table.CreateIfNotExistsAsync().ConfigureAwait(false);
                this.tables[tableLocal] = table;
            }

            return this.tables[tableLocal];
        }

        private async Task<IEnumerable<T>> QueryAsync<T>(CloudTable table, TableQuery<T> query)
            where T : class, ITableEntity, new()
        {
            var entities = new List<T>();

            TableContinuationToken token = null;
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(false);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            }
            while (token != null);

            return entities;
        }

        private async Task<IEnumerable<T>> QueryAsyncWithTakeCount<T>(CloudTable table, TableQuery<T> query)
            where T : class, ITableEntity, new()
        {
            var entities = new List<T>();

            const int maxEntitiesPerQueryLimit = 1000;
            var totalTakeCount = query.TakeCount;
            var remainingRecordsToTake = query.TakeCount;

            TableContinuationToken token = null;
            do
            {
                query.TakeCount = remainingRecordsToTake >= maxEntitiesPerQueryLimit
                                      ? maxEntitiesPerQueryLimit
                                      : remainingRecordsToTake;
                remainingRecordsToTake -= query.TakeCount;

                var queryResult = await table.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(false);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            }
            while (entities.Count < totalTakeCount && token != null);

            return entities;
        }
    }
}