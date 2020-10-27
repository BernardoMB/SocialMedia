using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Linq;
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
        // (9) The application will know which implementation to use for these abstractions because they are registered in the Startup.cs class.

        // (10) Better use the generic repository:
        //private readonly IRepository<Post> _postRepository;
        //private readonly IRepository<User> _userRepository;
        //private readonly IRepository<Comment> _commentRepository;
        // (10) Note that BaseRepository is a generic class and it is the implementation to use for these abstractions (objects of type IRepository).

        // (10) Better use unit of wotk: 
        // (10) This will save us time. _unitOfWork will be able to call any method inside any repository implementation.
        private readonly IUnitOfWork _unitOfWork;
        // (10) The implementation to use for this abstraction is registered in the Startup.cs file.

        // (14) Use the configuration variables. Use the pagination options defined in appsettings.json
        private readonly PaginationOptions _paginationOptions;

        public PostService(
            // (9) Always use dependency injection in services.
            //IPostRepository postRepository,
            //IUserRepository userRepository
            // (9) The application will know which implementation to use for these abstractions because they are registered in the Startup.cs class.

            // (10) Better use the generic repository:
            //IRepository<Post> postRepository, // This will use the implementation 
            //IRepository<User> userRepository
            // (10) Note that BaseRepository is a generic class and it is the implementation to use for these abstractions (objects of type IRepository).

            // (10) Better use unit of work
            // (10) This will save us time. _unitOfWork will be able to call any method inside any repository implementation.
            IUnitOfWork unitOfWork,
            // (10) The implementation to use for this abstraction is registered in the Startup.cs file.

            // (14) Use the configuration variables. Inject the pagination options defined in appsettings.json
            IOptions<PaginationOptions> options
        )
        {
            //_postRepository = postRepository;
            //_userRepository = userRepository;
            // (10) Better use unit of work
            _unitOfWork = unitOfWork;

            // (14) Use the paginatino options
            _paginationOptions = options.Value;
             
        }

        public async Task InsertPost(Post post)
        {
            // Here we need to verify that the user that is trying to create a post exists.
            //var user = await _userRepository.GetUser(post.UserId);
            // (10) Better use the generic class:
            //var user = await _userRepository.GetById(post.UserId);
            // (10) Better use unit of work to get access to all repositories in the application
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            // Note that it is not the responsability of this class to validate that the UserId is not null.
            if (user == null)
            {
                // Generate domain exception
                // throw new Exception("User doesn't exists");
                // (11) The previous line was commented out because throwing exceptions this way is not 
                // acceptable for an API because the client sending the request will see the stack
                // trace error from .Net in the response object
                // (11) Better show our custom business exception
                throw new BusinessException("User doesn't exists");
                // (11) To property pass this exception to the client making the request showing a better format. Use a custom exception filter.
                // (11) See the Global exception filter class.

            }

            // (11) Add validation: If user has less than 10 posts he can only post once a week.
            var userPosts = await _unitOfWork.PostRepository.GetPostsByUser(user.Id);
            if (userPosts.Count() < 10)
            {
                var lastPost = userPosts.OrderByDescending(x => x.Date).ElementAt(0);
                // Alternativale one can use the following:
                // var lastPost = userPosts.OrderByDescending(x => x.Date).FirstOrDefault();
                if ((DateTime.Now - lastPost.Date).Value.TotalDays < 7)
                {
                    // throw new Exception("Users with less than 10 post are only allowed to post once a week");
                    // (11) Better show our custom business exception
                    throw new BusinessException("Users with less than 10 post are only allowed to post once a week");
                }
            }

            if (post.Description.ToLower().Contains("sexo"))
            {
                // throw new Exception("Content not allowed");
                // (11) Better show our custom business exception
                throw new BusinessException("Content not allowed");
            }
            
            //await _postRepository.InsertPost(post);
            // (10) Better use the generic class:
            //await _postRepository.Add(post);
            // (10) Better use unit of work to get access to all repositories in the application
            await _unitOfWork.PostRepository.Add(post);
            // (11) The save changes was deleted from the base repository. That task has to be made by the unit of work.
            await _unitOfWork.SaveChangesAsync();
        }

        //public IEnumerable<Post> GetPosts()
        // (12) Better user query filters:
        // public IEnumerable<Post> GetPosts(PostQueryFilter filters)
        // (13) Better work with out paginated list entity:
        public PagedList<Post> GetPosts(PostQueryFilter filters)
        {
            //return await _postRepository.GetPosts();
            // (10) Better use the generic class:
            //return await _postRepository.GetAll();
            // (10) Better use unit of work to get access to all repositories in the application
            // return await _unitOfWork.PostRepository.GetAll();
            // (11) Previous line call is no loger async.
            //return _unitOfWork.PostRepository.GetAll();

            // (14) Handling empty filters
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            // (12) Better implement filtering
            // (12) Filtering logic
            var posts = _unitOfWork.PostRepository.GetAll();
            if (filters.UserId != null)
            {
                posts = posts.Where(x => x.UserId == filters.UserId);
            }
            if (filters.Date != null)
            {
                posts = posts.Where(x => x.Date.Value.ToShortDateString() == filters.Date.Value.ToShortDateString());
            }
            if (filters.Description != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            // (13) Pagination goes after filtering data.
            var pagedPosts = PagedList<Post>.Create(posts, filters.PageNumber, filters.PageSize);

            return pagedPosts;
        }

        public async Task<Post> GetPost(int id)
        {
            //return await _postRepository.GetPost(id);
            // (10) Better use the generic class:
            //return await _postRepository.GetById(id);
            // (10) Better use unit of work to get access to all repositories in the application
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public Post GetPostSync(int id)
        {
            return _unitOfWork.PostRepository.GetByIdSync(id);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            //return await _postRepository.UpdatePost(post);
            // (10) Better use the generic class:
            //return await _postRepository.Update(post);
            // (10) Better use unit of work to get access to all repositories in the application
            // await _unitOfWork.PostRepository.Update(post);
            // (11) Previous line call is no loger async.
            _unitOfWork.PostRepository.Update(post);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePost(int id)
        {
            //return await _postRepository.DeletePost(id);
            // (10) Better use the generic class:
            //return await _postRepository.Delete(id);
            // (10) Better use unit of work to get access to all repositories in the application
            await _unitOfWork.PostRepository.Delete(id);
            return true;
        }
    }
}
