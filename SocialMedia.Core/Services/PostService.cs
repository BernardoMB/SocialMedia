using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    /**
     * This class will hold implement all business logic regarding posts.
     */
    public class PostService : IPostService
    {
        // (9) Always use dependency injection in services.
        //private readonly IPostRepository _postRepository;
        //private readonly IUserRepository _userRepository;
        // (10) Better use the generic class:
        //private readonly IRepository<Post> _postRepository;
        //private readonly IRepository<User> _userRepository;
        //private readonly IRepository<Comment> _commentRepository;
        // (10) Better use unit of wotk. This will sace us time. _unitOfWork will be able to call any method inside any repository implementation.
        private readonly IUnitOfWork _unitOfWork;

        public PostService(
            //IPostRepository postRepository,
            //IUserRepository userRepository
            // (10) Better use the generic class:
            //IRepository<Post> postRepository,
            //IRepository<User> userRepository
            // (10) Better use unit of work
            IUnitOfWork unitOfWork
        ) {
            //_postRepository = postRepository;
            //_userRepository = userRepository;
            // (10) Better use unit of work
            _unitOfWork = unitOfWork;
             
        }

        public async Task InsertPost(Post post)
        {
            // Here we need to verify that the user that is trying to create a post exists.
            //var user = await _userRepository.GetUser(post.UserId);
            // (10) Better use the generic class:
            //var user = await _userRepository.GetById(post.UserId);
            // (10) Better use unit of work to get access to all repositories in the application
            var user = await _unitOfWork.PostRepository.GetById(post.UserId);
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
            //await _postRepository.InsertPost(post);
            // (10) Better use the generic class:
            //await _postRepository.Add(post);
            // (10) Better use unit of work to get access to all repositories in the application
            await _unitOfWork.PostRepository.Add(post);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            //return await _postRepository.GetPosts();
            // (10) Better use the generic class:
            //return await _postRepository.GetAll();
            // (10) Better use unit of work to get access to all repositories in the application
            return await _unitOfWork.PostRepository.GetAll();
        }

        public async Task<Post> GetPost(int id)
        {
            //return await _postRepository.GetPost(id);
            // (10) Better use the generic class:
            //return await _postRepository.GetById(id);
            // (10) Better use unit of work to get access to all repositories in the application
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            //return await _postRepository.UpdatePost(post);
            // (10) Better use the generic class:
            //return await _postRepository.Update(post);
            // (10) Better use unit of work to get access to all repositories in the application
            return await _unitOfWork.PostRepository.Update(post);
        }

        public async Task<bool> DeletePost(int id)
        {
            //return await _postRepository.DeletePost(id);
            // (10) Better use the generic class:
            //return await _postRepository.Delete(id);
            // (10) Better use unit of work to get access to all repositories in the application
            return await _unitOfWork.PostRepository.Delete(id);
        }
    }
}
