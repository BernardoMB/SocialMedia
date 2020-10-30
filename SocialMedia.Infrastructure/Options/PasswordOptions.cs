using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Options
{
    /*
    * (19.1) This class will help to define how to hash and store passwords
    */
    public class PasswordOptions
    {
        public int SaltSize { get; set; }
        public int KeySize { get; set; }
        public int Iterations { get; set; }
    }
}
