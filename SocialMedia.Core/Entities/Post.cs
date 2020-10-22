using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public partial class Post : BaseEntity
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        public int UserId { get; set; }
        // (7) Adding a ? at the end of the name makes the property nullable which means that this property can be null of not defined.
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
