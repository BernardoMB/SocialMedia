using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.Entities
{
    /**
     * Base entity.
     * All entities in this APi will extend this base entity.
     * This class use the abstract accesor because we will not be creating instances of this class, we will only use this class to extend its functionality.
     */
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        // This base entity can contain some other fields for compliance porpuses, for example: createdAt, updatedAt, createdBy, updatedBy.
    }
}
