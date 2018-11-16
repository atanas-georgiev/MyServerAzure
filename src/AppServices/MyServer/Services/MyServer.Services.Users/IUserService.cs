namespace MyServer.Services.Users
{
    using System.Linq;
    using System.Threading.Tasks;

    using MyServer.Data.Models;

    public interface IUserService
    {
        Task <IQueryable<User>> GetAllAsync();

        Task<User> GetByIdAsync(string id);

        Task UpdateAsync(string id, bool isAdmin);
    }
}