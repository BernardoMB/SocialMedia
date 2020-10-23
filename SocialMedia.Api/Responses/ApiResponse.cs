using SocialMedia.Core.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Api.Responses
{
    /**
     * The API will allways return a class from this type.
     * This is for better coding practices.
     */
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }

        // (14) Adding metadata property for returning pagination info in the response body.
        public Metadata Meta { get; set; }
    }
}
