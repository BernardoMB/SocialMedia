using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    // In this interface we will define the methods that any class implementing this interface should implement
    // This interface is the abstraction of the actual repository
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetPostsByUser(int userId);

        //Task<IEnumerable<Post>> GetPosts();
        //Task<Post> GetPost(int id);
        //Task InsertPost(Post post);
        //Task<bool> UpdatePost(Post post);
        //Task<bool> DeletePost(int id);
        // (11) Methods are commented out because we have them already in the IRepository abstraction.
        // (11) Note that implementation of this abstraction is already extending the BaseReposisotry class.

    }
}
