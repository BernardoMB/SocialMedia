using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface ICommentRespository
    {
        Task<bool> DeleteComment(int id);
        Task<Comment> GetComment(int id);
        Task<IEnumerable<Comment>> GetComments();
        Task InsertComment(Comment comment);
        Task<bool> UpdateComment(Comment comment);
    }
}
