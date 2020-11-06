using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    /* (10)
     * Here we will declare all the repositories our application will be working with.
     */
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        IBaseRepository<User> UserRepository { get; }
        IBaseRepository<Comment> CommentRepository { get; }
        ISecurityRepository SecurityRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();

    }
}
