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
    public class PostRepository : IPostRepository
    {
        private readonly SocialMediaContext _context;
        // Also use dependency injectionto inject the database context
        public PostRepository(SocialMediaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            // Simulate database connection

            // Generate fake data
            //var posts = Enumerable.Range(1, 10).Select(x => new Posts { 
            //    PostId = x,
            //    Description = $"Description {x}",
            //    Date = DateTime.Now,
            //    Image = $"https://misapis.com/{x}",
            //    UserId = x * 2
            //});

            // Alternatively
            //List<Posts> posts2 = new List<Posts>();
            //foreach (int i in Enumerable.Range(1, 10))
            //{
            //    Posts post = new Posts() 
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
            var posts = await _context.Posts.ToListAsync();

            // Await asynchronous task
            await Task.Delay(10);

            // Return fake data
            return posts;
        }

        public async Task<Post> GetPost(int id)
        {
            // Use a real connection to the database
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == id);

            // Return fake data
            return post;
        }

        public async Task InsertPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePost(Post post)
        {
            var currentPost = await GetPost(post.PostId);
            currentPost.Date = post.Date;
            currentPost.Description = post.Description;
            currentPost.Image = post.Image;

            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeletePost(int id)
        {
            var currentPost = await GetPost(id);
            //_context.Posts.RemoveRange(); // Use RemoveRange() to delete several posts at the same time.
            _context.Posts.Remove(currentPost);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
