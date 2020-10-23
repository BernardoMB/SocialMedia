using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.QueryFilters
{
    public class PostQueryFilter
    {
        // (12) Specify query string optional parameters:
        // (12) Filter by user
        public int? UserId { get; set; }
        // (12) Filter by date
        public DateTime? Date { get; set; }
        // (12) Filter by description
        public string Description { get; set; }
        // (12) Removing the ? the parameter will take default values if not especified in the request
        // (12) (Ej. unserId will be 0, date will be 01/01/0001 and description will null)
        // (12) With the ? the parameter will be null if not specified
        // (12) (Ej. unserId will be null, date will be null)


    }
}
