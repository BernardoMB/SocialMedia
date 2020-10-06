using SocialMedia.Core.Entities;
using SocialMedia.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SocialMediaContext _context;

        public UserRepository(SocialMediaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var items = await _context.Users.ToListAsync();
            return items;
        }

        public async Task<User> GetUser(int id)
        {
            var item = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
            return item;
        }

        public async Task InsertUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
            var currentItem = await GetUser(user.UserId);
            currentItem.DateOfBirth = user.DateOfBirth;
            currentItem.Email = user.Email;
            currentItem.IsActive = user.IsActive;
            currentItem.FirstName = user.FirstName;
            currentItem.LastName = user.LastName;
            currentItem.Phone = user.Phone;
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var currentItem = await GetUser(id);
            _context.Users.Remove(currentItem);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

    }
}
