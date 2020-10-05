using FluentValidation;
using SocialMedia.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Validators
{
    /**
     * This validator must be registered in the Startup.cs file of this web API applicacion.
     */
    public class PostValidator : AbstractValidator<PostDto> 
    {
        public PostValidator()
        {
            // Define validation rules in the constructor

            RuleFor(post => post.Description)
                .NotNull()
                .Length(1, 380);

            RuleFor(post => post.Date)
                .NotNull() // Date type is never null. We must add a '?' in the Post.cs class to the date property to make it nullable.
                .LessThan(DateTime.Now);
        }
    }
}