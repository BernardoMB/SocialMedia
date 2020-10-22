using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.DTOs;
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
    /* (11)
     * This class is extending the BaseRepository class therefore it has all its functionality.
     */
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Post>> GetPostsByUser(int userId)
        {
            // (11) Fetch user posts using a custom filter passing a lambda expresion.
            return await _entities.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
