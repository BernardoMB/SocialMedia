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
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public PostService(
            IUnitOfWork unitOfWork,
            IOptions<PaginationOptions> options
        )
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task InsertPost(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new BusinessException("User doesn't exists");
            }
            var userPosts = await _unitOfWork.PostRepository.GetPostsByUser(user.Id);
            if (userPosts.Count() < 10)
            {
                var lastPost = userPosts.OrderByDescending(x => x.Date).ElementAt(0);
                // Alternatively
                // var lastPost = userPosts.OrderByDescending(x => x.Date).FirstOrDefault();
                if ((DateTime.Now - lastPost.Date).Value.TotalDays < 7)
                {
                    throw new BusinessException("Users with less than 10 post are only allowed to post once a week");
                }
            }
            if (post.Description.ToLower().Contains("sexo"))
            {
                throw new BusinessException("Content not allowed");
            }
            await _unitOfWork.PostRepository.Add(post);
            await _unitOfWork.SaveChangesAsync();
        }

        public PagedList<Post> GetPosts(PostQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;
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
            var pagedPosts = PagedList<Post>.Create(posts, filters.PageNumber, filters.PageSize);
            return pagedPosts;
        }

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public Post GetPostSync(int id)
        {
            return _unitOfWork.PostRepository.GetByIdSync(id);
        }

        public async Task<bool> UpdatePost(Post post)
        {

            var existingPost = await _unitOfWork.PostRepository.GetById(post.Id);
            existingPost.Image = post.Image;
            existingPost.Description = post.Description;
            _unitOfWork.PostRepository.Update(existingPost);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
