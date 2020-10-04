using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SocialMedia.Core.Entities;
using SocialMedia.Infrastructure.Data.Configurations;

namespace SocialMedia.Infrastructure.Data
{
    public partial class SocialMediaContext : DbContext
    {
        public SocialMediaContext()
        {
        }

        public SocialMediaContext(DbContextOptions<SocialMediaContext> options)
            : base(options)
        {
        }

        /*
         * All of our entities are in Spanish but all of our code is written in English,
         * We will use an special tool for mapping the code witten in Spanish to make it
         * work with English names.
         * 
         * Renaming can be donde using Visual Studio renaming feature: highlight the word
         * to be renamed and then hit ctrl + r + r.
         * 
         * For example, for the Comments DbSet, here the code will asume that the name on the
         * table is Comments, however this is not true. See each configuration class below to
         * see how we address this naming problem.
         */
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // (Apply schema definitions)
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
