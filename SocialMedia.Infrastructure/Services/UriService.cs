using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Services
{
    /* (14) URI Service
    * Class for getting the appropriate uri depending the environment in which the application is running.
    * This class goes inside the infrasctructure project because is not related to the core of the application or to the business logic.
    * 
    * This class' job is to generate Uri.
    */
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl)
        {
            // Working with string interpolation.
            string baseUrl = $"{_baseUri}{actionUrl}";
            return new Uri(baseUrl);
        }
    }
}
