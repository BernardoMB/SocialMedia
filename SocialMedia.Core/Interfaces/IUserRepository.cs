using SocialMedia.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<bool> DeleteUser(int id);
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
        Task InsertUser(User user);
        Task<bool> UpdateUser(User user);
    }
}