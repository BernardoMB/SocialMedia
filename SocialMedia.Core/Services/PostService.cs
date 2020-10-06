using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    /**
     * This class will hold implement all business logic regarding posts.
     */
    public class PostService : IPostService
    {
        // (9) Always use dependency injection in services.
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostService(
            IPostRepository postRepository,
            IUserRepository userRepository
        ) {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }
        
        public async Task<IEnumerable<Post>> GetPosts()
        {
             return await _postRepository.GetPosts();
        }

        public async Task<Post> GetPost(int id)
        {
            return await _postRepository.GetPost(id);
        }

        public async Task InsertPost(Post post)
        {
            // Here we need to verify that the user that is trying to create a post exists.
            var user = await _userRepository.GetUser(post.UserId);
            // Note that it is not the responsability of this class to validate that the UserId is not null.
            if (user == null)
            {
                // Generate domain exception
                throw new Exception("User doesn't exists");
            }
            if (post.Description.ToLower().Contains("sexo"))
            {
                throw new Exception("Content not allowed");
            }
            await _postRepository.InsertPost(post);
        }
        
        public async Task<bool> UpdatePost(Post post)
        {
            return await _postRepository.UpdatePost(post);
        }
        
        public async Task<bool> DeletePost(int id)
        {
            return await _postRepository.DeletePost(id);
        }
    }
}
