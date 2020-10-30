using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(SocialMediaContext context) : base(context) { }

        public async Task<Security> GetLoginByCredentials(UserLogin login)
        {
            //return await _entities.FirstOrDefaultAsync(x => x.User == login.User && x.Password == login.Password);
            // (19.2) We will not be filtering by password any more because after this method is executen when loggin in a user,
            // another method is executed to actually validate the password entered by the user.
            return await _entities.FirstOrDefaultAsync(x => x.User == login.User);
        }
    }
}
