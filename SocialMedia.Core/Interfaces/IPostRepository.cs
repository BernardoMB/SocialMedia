using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    // In this interface we will define the methods that any class implementing this interface should implement
    // This interface is the abstraction of the actual repository
    public interface IPostRepository
    {
        Task<IEnumerable<Publicacion>> GetPosts();
    }
}
