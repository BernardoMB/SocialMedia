using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task Add(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);

    }
}
