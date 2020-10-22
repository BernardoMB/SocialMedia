using SocialMedia.Core.Entities;
using SocialMedia.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly SocialMediaContext _context;

        // Also use dependency injection to inject the database context.
        public UserRepository(SocialMediaContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var items = await _context.Users.ToListAsync();
            return items;
        }

        public async Task<User> GetById(int id)
        {
            var item = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<bool> Update(User user)
        {
            var currentItem = await GetById(user.Id);
            currentItem.DateOfBirth = user.DateOfBirth;
            currentItem.Email = user.Email;
            currentItem.IsActive = user.IsActive;
            currentItem.FirstName = user.FirstName;
            currentItem.LastName = user.LastName;
            currentItem.Phone = user.Phone;
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var currentItem = await GetById(id);
            _context.Users.Remove(currentItem);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
