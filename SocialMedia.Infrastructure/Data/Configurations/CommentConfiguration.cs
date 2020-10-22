using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            /*
                * The code that we see inside here is what is known as the fluent API
                * This is what we use for configuring our entities
                */

            // Dealing with different table names problem:
            builder.ToTable("Comentario"); // This is how this builder is named on the actual db.

            // KEYS

            builder.HasKey(e => e.Id);

            // PROPERTIES

            builder.Property(e => e.Id)
                .HasColumnName("IdComentario") // Properties are also mapped with different names
                .ValueGeneratedNever();

            builder.Property(e => e.PostId)
                .HasColumnName("IdPublicacion"); // Properties are also mapped with different names

            builder.Property(e => e.UserId)
                .HasColumnName("IdUsuario"); // Properties are also mapped with different names

            builder.Property(e => e.IsActive)
                .HasColumnName("Activo"); // Properties are also mapped with different names

            builder.Property(e => e.Description)
                .HasColumnName("Descripcion") // Properties are also mapped with different names
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.Property(e => e.Date)
                .HasColumnName("Fecha") // Properties are also mapped with different names
                .HasColumnType("datetime");

            // RELATIONS

            builder.HasOne(d => d.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comentario_Publicacion");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comentario_Usuario");
        }
    }
}
