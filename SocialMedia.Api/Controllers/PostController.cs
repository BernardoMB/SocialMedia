using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    /**
     * This controller will handle all https methods related to social media posts.
     * We do not need to change the [controller] segment of this API route path. 
     * It will automatically pick up the first part of the class name, in this case it would be 'post'.
     */
    [Route("api/[controller]")]
    /** (7)
     * Decorate the class with ApiController decorator for using the following features:
     * * Automatically return appropiate status codes (Ej. Returning 404 when resource is not found).
     * * Automatically validate requests DTOs using data anotations (decorators) inside the dto classes.
     */
    [ApiController]
    /** (7)
     * This controller is extending the ControllerBase class. It can extend the Controller class instead of ControllerBase class.
     * Using ControllerBase vs. Controller class: ControllerBase is specially designed to work with an API.
     * Controller aditionaly add features for working with MVC which includes the views. Extending from Controller is for working with views.
     * We will not be using the MVC pattern because we will not be using views, therefore this controller class will not extend the Controller class.
     */
    public class PostController : ControllerBase
    {
        // One must work with the abstraction (interface) of the repository instead of working with the actual implementation.
        // And that is why this class is declaring a property of type interface.
        //private readonly IPostRepository _postRepository;
        // (9) Alternatively, instead of injecting the repository, we should inject the service.
        private readonly IPostService _postService;

        // Use automapper
        private readonly IMapper _mapper;

        /*
         * The following mwthod is the class constructor.
         * The technique of declaring a property of type interface inside this class and then passing the constructor
         * an object of type interface is called dependency injection.
         */
        public PostController(
            //IRepository<Post> postRepository, 
            // (9) Alternatively, instead of injecting the repository, we should inject the service.
            IPostService postService,
            // Inject the mapper service
            IMapper mapper
        ) {
            // The post repository implementation that this controller is going to use is the one passed to this constructor.
            // Si no hacemos esta inyeccion de dependencia, entonces esta clase se esta acoplando a una implementacion en concreto, es mejor hacerlo con inyeccion de dependencias.
            // We use dependency injection to avoid working with actual class implementations.
            // The service.AddTrascient call inside the ConfigureServices method inside the Startup.cs class is determining which implementation of the interface this class will use.
            //_postRepository = postRepository;
            // (9) Alternatively, instead of injecting the repository, we should inject the service.
            _postService = postService;

            // Inject automapper dependency
            _mapper = mapper;
        }

        // HttpGet decorator for telling the controller that the method GetPosts is the function to be called when invoking the the GET api/controller route
        [HttpGet]
        // (12) Return an specific type of data
        // [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))] // (12) Here we will specify which type of response we will return.
        // (12) Because the method is returning an instance of an object of type ActionResult<ApiResponse<IEnumerable<PostDto>>> it is not necesary to specify the type in the anotation
        // [ProducesResponseType((int)HttpStatusCode.OK)]
        // (12) When a method can return other types of response, then we should register that other response type:
        // [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        // (12) Because the method is returning an instance of an object of type ActionResult<ApiResponse<IEnumerable<PostDto>>> it is not necesary to specify the type in the anotation
        // [ProducesResponseType((int)HttpStatusCode.OK)]
        // public IActionResult GetPosts(
        // (12) Return Api response type: 
        // public ApiResponse<IEnumerable<PostDto>> GetPosts( // (12) We will no use this approach becase this aproach will cause the application to always return a 200 status.
        // (12) Use the following approach to return either Ok(response) or response:
        // public ActionResult<ApiResponse<IEnumerable<PostDto>>> GetPosts(
        // (12) The easiest way is to just return IAction result and specify the return type in the annotations:
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        public IActionResult GetPosts(
            // (12) Specify query string optional parameters:
            // int? userId, // (12) Filter by user
            // DateTime? date, // (12) Filter by date
            // string description // (12) Filter by description
            // (12) Removing the ? the parameter will take default values if not especified in the request
            // (12) (Ej. unserId will be 0, date will be 01/01/0001 and description will null)
            // (12) With the ? the parameter will be null if not specified
            // (12) (Ej. unserId will be null, date will be null)
            // (12) Beter use a class for the filters and pagination:
            [FromQuery] // Wiht this, the method will pick the query filters from the query params instead from the body. It also does the name mappings automatically.
            PostQueryFilter filters // (12) Complex object as a parameter. Then this method will expect this data in the body of the request.
        )
        {
            // (7) Because this method is declared in a class that extends from the ControllerBase,
            // we are able to access the ModelState property:
            if (!ModelState.IsValid)
            {
                // The BasRequest method is decalred in the ControllerBase class which is the class this 
                // class is extending from.
                // return BadRequest();
            }
            // (7) This previous code is not actually required thanks to the decorator ApiController 
            // because it automatically validated the model state based on the property decorators inside the dto classes.
            // (8) This previous code is not actually required. Thanks to the ApiController decorator global action filters
            // are triggered. This application implements a custom validation action filter, so automatic validation is handled automatically
            // by the Fluent API validation filter, therefore no manual validation is needed here.

            // var posts = new PostRepository().GetPosts();
            // We should not use the keyword new. Instead we should use dependency injection.
            // Instead we should use the dependency injection pattern
            //var posts = await _postRepository.GetPosts();
            // (9) Alternatively call the service instead of calling the repository directly
            //var posts = _postService.GetPosts();
            // (12) Modify the server call to use filters
            var posts = _postService.GetPosts(filters);

            // Map from the posts domain entities to dtos
            //var postsDto = posts.Select(x => new PostDto()
            //{
            //    PostId = x.PostId,
            //    Date = x.Date,
            //    Description = x.Description,
            //    Image = x.Image,
            //    UserId = x.UserId
            //});
            // Alternatively use automapper library for automatically map entity properties.
            // var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
            // (13) Better work with pagination:
            // var postsDto = _mapper.Map<PagedList<PostDto>>(posts);
            // (13) Automapper is able to transform PagedList to IEnumerable, then use the same code:
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);

            // (13) Return pagination info in the response headers.
            //var metadata = new // Creating a anonymous object. It is better to work with typed objects.
            //{
            //    posts.CurrentPage,
            //    posts.TotalPages,
            //    posts.PageSize,
            //    posts.TotalCount,
            //    posts.HasNextPage,
            //    posts.HasPreviousPage
            //};
            // (14) Better create a Metadata instance for returning pagination data:
            var metadata = new Metadata
            {
                CurrentPage = posts.CurrentPage,
                TotalPages = posts.TotalPages,
                PageSize = posts.PageSize,
                TotalCount = posts.TotalCount,
                HasNextPage = posts.HasNextPage,
                HasPreviousPage = posts.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            // (13) In the previous line we are working with Json objects.

            // (8) Return an object of type ApiResponse for better coding practices.
            // var response = new ApiResponse<IEnumerable<PostDto>>(postsDto);
            // (13) Better work with pagination:
            // var response = new ApiResponse<PagedList<PostDto>>(postsDto);
            // (13) Automapper is able to transform PagedList to IEnumerable, then use the same code:
            // var response = new ApiResponse<IEnumerable<PostDto>>(postsDto);
            // (14) Better return  pagination data in the repsonse body.
            var response = new ApiResponse<IEnumerable<PostDto>>(postsDto) {
                Meta = metadata
            };
            // (14) We are not specifying metadata in other responses (Ej. get post by id), so there is no point in returning the Meta property in the ApiResponse.
            // (14) See startup.cs AddNewtonsoftJson call to see how this problem is solved.

            // Return the dto instead of out domain entity.
            // Ok for returning 200 status.
            return Ok(response); // (12) Return an object of type ActionResult this objectc implements the IActionResult abstraction.
            // (12) We can return something other than Ok(response).
            // We can return an object of type ApiResponse<>. This has some downsides.
            // return response; // (12) With this, the API will allways return a 200 status.
            // The way to handle this issue is to return the success status of the response in the response body. This is not the best approach.
            // (12) That would not be the best aproach so we will return a the Ok(response).
            // (12) When the return type of this decorated method is an instance of an object that implements IActionResult we can
            // return OK(response);
            // or alternativaly (if return type of method is ActionResult<>)
            // return response; // <-- this will get serialized as ActionResult<ApiResponse<IEnumerable<PostDto>>.
            // The best way is to return IActionResult because the code will look clean and easy to write.
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            // (7) Because this method is declared in a class that extends from the ControllerBase,
            // we are able to access the ModelState property:
            if (!ModelState.IsValid)
            {
                // The BasRequest method is decalred in the ControllerBase class which is the class this 
                // class is extending from.
                return BadRequest();
            }
            // (7) This previous code is not actually required thanks to the decorator ApiController 
            // because it automatically validated the model state based on the decorator sinside the dto classes.

            // var posts = new PostRepository().GetPosts();
            // We should not use the keyword new. Instead we should use dependency injection.
            // Instead we should use the dependency injection pattern:
            //var post = await _postRepository.GetPost(id);
            // (9) Alternatively call the service instead of calling the repository directly
            var post = await _postService.GetPost(id);
            //var post = _postService.GetPostSync(id);

            // Mappings
            //var postDto = new PostDto()
            //{
            //    PostId = post.PostId,
            //    Date = post.Date,
            //    Description = post.Description,
            //    Image = post.Image,
            //    UserId = post.UserId
            //};
            // Alternatively 
            var postDto = _mapper.Map<PostDto>(post);

            // (8) Return an object of type ApiResponse for better coding practices.
            var response = new ApiResponse<PostDto>(postDto);

            // Ok for returning 200 status
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPost(PostDto postDto)
        {
            // (7) Because this method is declared in a class that extends from the ControllerBase,
            // we are able to access the ModelState property:
            if (!ModelState.IsValid)
            {
                // The BasRequest method is decalred in the ControllerBase class which is the class this 
                // class is extending from.
                return BadRequest();
            }
            // (7) This previous code is not actually required thanks to the decorator ApiController 
            // because it automatically validated the model state based on the decorator sinside the dto classes.
            // If the automatic validation is disabled in Startup.cs configuration method this code must be used
            // in order to valida the model validation.

            // Mapping from PostDto to Post domain entity
            //var post = new Post()
            //{
            //    Date = postDto.Date,
            //    Description = postDto.Description,
            //    Image = postDto.Image,
            //    UserId = postDto.UserId
            //};
            // Alternatively
            var post = _mapper.Map<Post>(postDto);


            // With the previous statement the application is now protected agains overposting.
            // If the requests postDto contains the user, this endpoit will only use the variables we are mapping in the previous statement.
            //await _postRepository.InsertPost(post);
            // Serialization: convert a C# Class to a JSON object.
            // (9) Alternatively call the service instead of calling the repository directly
            await _postService.InsertPost(post);

            // Map again so no domain entity is returned. Return the mapped post instead.
            postDto = _mapper.Map<PostDto>(post);

            // (8) Return an object of type ApiResponse for better coding practices.
            // Note that the class ApiResponse is a generic class.
            // Note that here in the response we have the post with the created id.
            var response = new ApiResponse<PostDto>(postDto);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            post.Id = id;
            var result = await _postService.UpdatePost(post);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _postService.DeletePost(id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }
    }
}
