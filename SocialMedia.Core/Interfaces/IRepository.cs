using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        // (10) This generic insterface is the abstraction of any repository.
        // (10) Here the keyword where T : BaseEntity is just for creating a restriction:
        // (10) Only classes extending from BaseEntity can be used for creating an abstraction of a repository.

        Task Add(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);

    }
}
