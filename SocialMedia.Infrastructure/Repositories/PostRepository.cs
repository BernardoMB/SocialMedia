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
    public class PostRepository : IPostRepository
    {
        private readonly SocialMediaContext _context;
        // Also use dependency injectionto inject the database context
        public PostRepository(SocialMediaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Publicacion>> GetPosts()
        {
            // Simulate database connection

            // Generate fake data
            //var posts = Enumerable.Range(1, 10).Select(x => new Post { 
            //    PostId = x,
            //    Description = $"Description {x}",
            //    Date = DateTime.Now,
            //    Image = $"https://misapis.com/{x}",
            //    UserId = x * 2
            //});

            // Alternatively
            //List<Post> posts2 = new List<Post>();
            //foreach (int i in Enumerable.Range(1, 10))
            //{
            //    Post post = new Post() 
            //    {
            //        PostId = i,
            //        Description = $"Description {i}",
            //        Date = DateTime.Now,
            //        Image = $"https://misapis.com/{i}",
            //        UserId = i * 2
            //    };
            //    posts2.Add(post);
            //};

            // Use a real connection to the database instead
            var posts = await _context.Publicacion.ToListAsync();

            // Await asynchronous task
            await Task.Delay(10);

            // Return fake data
            return posts;
        }
    }
}
