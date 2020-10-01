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
        public async Task<IEnumerable<Publicacion>> GetPosts()
        {
            //List<Post> posts2 = new List<Post>();
            //foreach (int i in Enumerable.Range(1, 10))
            //{
            //    Post post = new Post()
            //    {
            //        PostId = i,
            //        Description = $"Description Mongo {i}",
            //        Date = DateTime.Now,
            //        Image = $"https://misapis.com/{i}",
            //        UserId = i * 2 + 1
            //    };
            //    posts2.Add(post);
            //};

            List<Publicacion> posts = new List<Publicacion>();
            foreach (int i in Enumerable.Range(1, 10))
            {
                Publicacion post = new Publicacion()
                {
                };
                posts.Add(post);
            };

            // Await asynchronous task
            await Task.Delay(10);

            // Return fake data
            return posts;
        }
    }
}
