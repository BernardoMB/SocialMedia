using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // Inject the SocialMediaContext.
        private readonly SocialMediaContext _context;
        // Declare the entities access as protected so it is accesible by this class and classes exteding from this class.
        protected readonly DbSet<T> _entities;

        public BaseRepository(SocialMediaContext context) {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity); // Compare how this is done in the PostRepository
            // await _context.SaveChangesAsync();
            // (11) The previous line got commented out because the unit of work implementation is
            // the one in charge of handling the transactions, therefore the UnifOfWork class is the
            // one in charge of saving changes after every transaction.
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
            // await _context.SaveChangesAsync();
            // (11) The previous line got commented out because the unit of work implementation is
            // the one in charge of handling the transactions, therefore the UnifOfWork class is the
            // one in charge of saving changes after every transaction.
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            _entities.Remove(entity);
            // await _context.SaveChanges();
            // (11) The previous line got commented out because the unit of work implementation is
            // the one in charge of handling the transactions, therefore the UnifOfWork class is the
            // one in charge of saving changes after every transaction.
        }
    }
}
