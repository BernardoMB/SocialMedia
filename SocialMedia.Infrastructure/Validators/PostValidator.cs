using FluentValidation;
using SocialMedia.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Validators
{
    /// <summary>
    /// This class is a validator class and will be implemented in a validation filter.
    /// This validator must be registered in the Startup.cs file of this web API applicacion.
    /// </summary>
    public class PostValidator : AbstractValidator<PostDto> 
    {
        /// <summary>
        /// Class constructor. Inside this constructor go all validation rules for the PostDto.
        /// Is better to implement this validation filter rather than using class annotations inside our domain entity class.
        /// </summary>
        public PostValidator()
        {
            RuleFor(post => post.Description)
                .NotNull()
                .WithMessage("La descripcion no puede ser nula");

            RuleFor(post => post.Description)
                .Length(1, 380)
                .WithMessage("La logitud de la descripcion debe estar entre 1 y 380 caracteres");

            RuleFor(post => post.Date)
                .NotNull()
                .LessThan(DateTime.Now);
        }
    }
}