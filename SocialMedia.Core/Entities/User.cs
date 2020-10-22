using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public partial class User : BaseEntity
    {
        // (10) This class will Inherit from the BaseEntity class.
        public User()
        {
            Comments = new HashSet<Comment>();
            Posts = new HashSet<Post>();
        }

        // public int UserId { get; set; } // This is commented out because this class is extending from BaseEntity.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
