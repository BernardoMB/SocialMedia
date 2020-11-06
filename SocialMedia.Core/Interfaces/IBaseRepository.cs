using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    /// <summary>
    /// This generic insterface is the abstraction of any repository.
    /// Here the keyword where T : BaseEntity is just for creating a restriction:
    /// Only classes extending from BaseEntity can be used for creating an abstraction of a repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task Add(T entity);
        IEnumerable<T> GetAll();
        Task<T> GetById(int id);
        T GetByIdSync(int id);
        void Update(T entity);
        Task Delete(int id);
    }
}
