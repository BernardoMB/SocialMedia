using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public interface IPostService
    {
        Task InsertPost(Post post);
        PagedList<Post> GetPosts(PostQueryFilter filters);
        Task<Post> GetPost(int id);
        Post GetPostSync(int id);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(int id);
    }
}