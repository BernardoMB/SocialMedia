using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    /**
     * If we want to use depedency injection we must implement abstractions of classes or interfaces.
     */
    public interface IPostService
    {
        Task InsertPost(Post post);
        // IEnumerable<Post> GetPosts();
        // (12) Better user query filters:
        // IEnumerable<Post> GetPosts(PostQueryFilter filters);
        // (13) Better work with out paginated list entity:
        PagedList<Post> GetPosts(PostQueryFilter filters);
        Task<Post> GetPost(int id);
        Post GetPostSync(int id);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(int id);
    }
}