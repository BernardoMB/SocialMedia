using System;

namespace SocialMedia.Core.Entities
{
    public partial class Comment : BaseEntity
    {
        // (10) This class will Inherit from the BaseEntity class.

        // public int CommentId { get; set; } // This is commented out because this class is extending from BaseEntity.
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
