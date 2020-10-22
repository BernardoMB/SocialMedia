using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    /* (10)
     * This class is a genercic class and it is an implementation of the IRepository abstraction.
     * One can use this generic class or other implementation. 
     */
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        // (10) Her the keyword where T : BaseEntity is setting a restriction:
        // (10) Only classes that extend from BaseEntity can be used to instanciate an object of type BaseRepository.

        private readonly SocialMediaContext _context;
        private readonly DbSet<T> _entities;

        public BaseRepository(SocialMediaContext context) {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task Add(T entity)
        {
            _entities.Add(entity); // Compare how this is done in the PostRepository
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<bool> Update(T entity)
        {
            _entities.Update(entity);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await GetById(id);
            _entities.Remove(entity);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
