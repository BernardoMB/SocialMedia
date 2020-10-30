using SocialMedia.Core.Enumerations;

namespace SocialMedia.Core.Entities
{
    /*
    * (18) Class for handling security
    */
    public class Security : BaseEntity
    {
        public string User { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public RoleType Role { get; set; }
    }
}
