using FluentValidation;
using SocialMedia.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Validators
{
    /** (7)
     * This validator must be registered in the Startup.cs file of this web API applicacion.
     */
    public class PostValidator : AbstractValidator<PostDto> 
    {
        public PostValidator()
        {
            // (7) Define validation rules in the constructor

            RuleFor(post => post.Description)
                .NotNull()
                .WithMessage("La descripcion no puede ser nula");

            RuleFor(post => post.Description)
                .Length(1, 380)
                .WithMessage("La logitud de la descripcion debe estar entre 1 y 380 caracteres");

            RuleFor(post => post.Date)
                .NotNull() // (7) Date type is never null. We must add a '?' in the Post.cs class to the date property to make it nullable.
                .LessThan(DateTime.Now);
        }
    }
}