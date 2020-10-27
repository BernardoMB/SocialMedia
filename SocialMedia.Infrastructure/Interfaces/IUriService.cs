using SocialMedia.Core.QueryFilters;
using System;

namespace SocialMedia.Infrastructure.Services
{
    /* (14) This insterface could be decalred inside the Core project, but it is better to have it here.
     */
    public interface IUriService
    {
        Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl);
    }
}