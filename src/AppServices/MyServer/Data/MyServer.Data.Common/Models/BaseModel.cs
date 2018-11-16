namespace MyServer.Data.Common.Models
{
    using System;

    using Microsoft.WindowsAzure.Storage.Table;

    public class BaseModel : TableEntity
    {
        public BaseModel()
            : base(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
        {
        }

        public BaseModel(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}