namespace MyServer.Data.Models
{
    using MyServer.Data.Common.Models;

    public class User : BaseModel
    {
        public User()
        {
        }

        public User(string id)
            : base("user", id)
        {
        }

        public User(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }
    }
}