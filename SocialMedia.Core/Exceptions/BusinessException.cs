using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.Exceptions
{
    /* (11)
     * This class is for throwing apropiate exceptions as a real API
     */
    public class BusinessException : Exception
    {
        public BusinessException()
        {

        }

        public BusinessException(string message) : base(message)
        {

        }
    }
}
