namespace MyServer.Services.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Graph;

    using MyServer.Data.Common;

    using User = MyServer.Data.Models.User;

    public class UserService : IUserService
    {
        private readonly IConfiguration configuration;

        private readonly GraphServiceClient graphClient;

        private readonly IRepository<User> users;

        public UserService(IConfiguration configuration, IRepository<User> users)
        {
            this.configuration = configuration;
            this.users = users;
            this.graphClient = CreateAuthorizationProviderClass.GetAuthenticatedGraphClient(this.configuration);
        }

        public async Task<IQueryable<User>> GetAllAsync()
        {
            var graphResult = await this.graphClient.Users.Request().GetAsync();
            var usersLocal = new List<User>();
            var repoUsers = await this.users.GetAllAsync();

            foreach (var graphUser in graphResult)
            {
                var repoUser = repoUsers.FirstOrDefault(u => u.Email == graphUser.UserPrincipalName);
                var user = new User("user", graphUser.UserPrincipalName)
                               {
                                   RowKey = graphUser.Id,
                                   DisplayName = graphUser.DisplayName,
                                   Email = graphUser.UserPrincipalName,
                                   IsAdmin = repoUser?.IsAdmin ?? false
                               };

                usersLocal.Add(user);
            }

            return usersLocal.AsQueryable();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var graphResult = await this.graphClient.Users.Request().GetAsync();
            var graphUser = graphResult.FirstOrDefault(u => u.Id == id);

            if (graphUser != null)
            {
                var repoUser = await this.users.GetAsync("user", id);

                return new User("user", graphUser.Id)
                           {
                               DisplayName = graphUser.DisplayName,
                               Email = graphUser.UserPrincipalName,
                               IsAdmin = repoUser?.IsAdmin ?? false
                           };
            }

            return null;
        }

        public async Task UpdateAsync(string id, bool isAdmin)
        {
            var user = await this.GetByIdAsync(id);

            if (user != null)
            {
                user.IsAdmin = isAdmin;
                user.ModifiedOn = DateTime.UtcNow;
                user.CreatedOn = DateTime.UtcNow;
                user.Timestamp = DateTimeOffset.UtcNow;

                await this.users.AddOrUpdateAsync(user);
            }
        }
    }
}