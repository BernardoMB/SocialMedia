using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Repositories;

/**
 * This class is the most important class of the project.
 * Here we will register all tecnologies or services that we will be using in this project:
 * Database connection
 * Authentication
 * Dependency injection
 * Etc
 */

namespace SocialMedia.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Database configuration
            services.AddDbContext<SocialMediaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SocialMedia")));

            // Resolve dependencies
            services.AddTransient<IPostRepository, PostRepository>();
            // With this, we are telling the program that everytime we use the IPostRepository interface (an abstraction), we will resolve an instance of the implementation of PostRpository

            // In case we want to change from database provider we would need to work with a different implementation, then we should resolve the other implementation
            //services.AddTransient<IPostRepository, PostMongoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
