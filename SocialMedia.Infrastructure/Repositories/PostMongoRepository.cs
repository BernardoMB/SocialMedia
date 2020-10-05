using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    // This class is only for test porpuses
    public class PostMongoRepository : IPostRepository
    {
        public async Task<IEnumerable<Post>> GetPosts()
        {
            //List<Posts> posts2 = new List<Posts>();
            //foreach (int i in Enumerable.Range(1, 10))
            //{
            //    Posts post = new Posts()
            //    {
            //        PostId = i,
            //        Description = $"Description Mongo {i}",
            //        Date = DateTime.Now,
            //        Image = $"https://misapis.com/{i}",
            //        UserId = i * 2 + 1
            //    };
            //    posts2.Add(post);
            //};

            List<Post> posts = new List<Post>();
            foreach (int i in Enumerable.Range(1, 10))
            {
                Console.WriteLine(i);
                Post post = new Post()
                {
                };
                posts.Add(post);
            };

            // Await asynchronous task
            await Task.Delay(10);

            // Return fake data
            return posts;
        }
        public async Task<Post> GetPost(int id)
        {
            //List<Posts> posts2 = new List<Posts>();
            //foreach (int i in Enumerable.Range(1, 10))
            //{
            //    Posts post = new Posts()
            //    {
            //        PostId = i,
            //        Description = $"Description Mongo {i}",
            //        Date = DateTime.Now,
            //        Image = $"https://misapis.com/{i}",
            //        UserId = i * 2 + 1
            //    };
            //    posts2.Add(post);
            //};

            List<Post> posts = new List<Post>();
            foreach (int i in Enumerable.Range(1, 10))
            {
                Console.WriteLine(i);
                Post post = new Post()
                {
                };
                posts.Add(post);
            };

            // Await asynchronous task
            await Task.Delay(10);

            // Return fake data
            return posts[0];
        }
        public Task InsertPost(Post post)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePost(Post post)
        {
            await Task.Delay(10);
            throw new NotImplementedException();
        }

        public async Task<bool> DeletePost(int id)
        {
            await Task.Delay(10);
            throw new NotImplementedException();
        }
    }
}
