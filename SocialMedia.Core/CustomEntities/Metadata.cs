﻿namespace SocialMedia.Core.CustomEntities
{
    /* (14) Pagination metadata returned to the client after every request
     * for a paginatio nendpoint.
     */
    public class Metadata
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public string PreviousPageUrl { get; set; }
        public string NextPageUrl { get; set; }
    }
}
