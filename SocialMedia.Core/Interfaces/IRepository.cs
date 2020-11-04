using SocialMedia.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task Add(T entity);
        IEnumerable<T> GetAll();
        Task<T> GetById(int id);
        T GetByIdSync(int id);
        void Update(T entity);
        Task Delete(int id);
    }
}
