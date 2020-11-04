using SocialMedia.Core.QueryFilters;
using System;

namespace SocialMedia.Infrastructure.Services
{
    public interface IUriService
    {
        Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl);
    }
}