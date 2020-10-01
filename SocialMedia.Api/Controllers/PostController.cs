using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    /**
     * This controller will handle all https methods related to social media posts
     */
    // We do not need to change the [controller] segment of this API route path. It will automatically pick up the first part of the class name, in this case it would be 'post'
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        // Working with the abstraction of the repository instead of working with the actual implementation
        private readonly IPostRepository _postRepository;

        // Dependency injection via class contructor
        public PostController(IPostRepository postRepository)
        {
            // The post repository this class is going to use id the one passed to the contructor.
            // Si no hacemos esta inyeccion de dependencia, entonces esta clase se esta acoplando a una implementacion en concreto, es mejor hacerlo con dependency injection
            _postRepository = postRepository;
        }

        // HttpGet decorator for telling the controller that the method GetPosts is the function to be called when invoking the the GET api/controller route
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            // We should not use the keyword new. Instead we should use dependency injection.
            // var posts = new PostRepository().GetPosts();
            // Instead we should use the dependency injection pattern
            var posts = await _postRepository.GetPosts();

            // Ok for returning 200 status
            return Ok(posts);
        }
    }
}
