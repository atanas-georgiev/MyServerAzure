namespace MyServer.Data.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.Storage.Table;

    using MyServer.Data.Common.Models;

    public interface IRepository<T>
        where T : BaseModel, new()
    {
        Task<object> AddAsync(T entity);

        Task<IEnumerable<T>> AddBatchAsync(IEnumerable<T> entities, BatchOperationOptions options);

        Task<object> AddOrUpdateAsync(T entity);

        Task<object> DeleteAsync(T entity);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetAsync(string partitionKey, string rowKey);

        Task<IEnumerable<T>> QueryAsync(TableQuery<T> query);

        Task<object> UpdateAsync(T entity);
    }
}