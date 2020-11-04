using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PostController(
            IPostService postService,
            IMapper mapper,
            IUriService uriService
        ) {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }
        
        [HttpGet(Name = nameof(GetPosts))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<PostDto>>))]
        public IActionResult GetPosts(
            [FromQuery]
            PostQueryFilter filters
        )
        {
            if (!ModelState.IsValid) { }
            var posts = _postService.GetPosts(filters);
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
            var routeUrl = Url.RouteUrl(nameof(GetPosts));
            var metadata = new Metadata
            {
                CurrentPage = posts.CurrentPage,
                TotalPages = posts.TotalPages,
                PageSize = posts.PageSize,
                TotalCount = posts.TotalCount,
                HasNextPage = posts.HasNextPage,
                HasPreviousPage = posts.HasPreviousPage,
                NextPageUrl = _uriService.GetPostPaginationUri(filters, routeUrl).ToString(),
                PreviousPageUrl = _uriService.GetPostPaginationUri(filters, routeUrl).ToString()
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            var response = new ApiResponse<IEnumerable<PostDto>>(postsDto) {
                Meta = metadata
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var post = await _postService.GetPost(id);
            var postDto = _mapper.Map<PostDto>(post);
            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPost(PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var post = _mapper.Map<Post>(postDto);
            await _postService.InsertPost(post);
            postDto = _mapper.Map<PostDto>(post);
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
