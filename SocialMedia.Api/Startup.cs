using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
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
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Repositories;

/**
 * This class is the most important class of the project.
 * Here we will register all tecnologies or services that we will be using in this project:
 * * Database connection
 * * Authentication
 * * Dependency injection
 * * Etc
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

        /**
         * This method gets called by the runtime. Use this method to add services to the container.
         * Inside this m ethod we will configure the application and declare middlewares.
         */
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure the automapper middleware and register all mappings.
            // Mappings profiles will be automatically detected thanks to the following sintax.
            // Any additional mapping will be automatically detected and registered.
            // Pass the assembliens so the AddAutoMapper function can search and register all existing mapping profiles.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Configure NewtonSoftJson package
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Ignore circular reference error.
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            // Disbale ApiController decorator automatic validation.
            .ConfigureApiBehaviorOptions(options =>
            {
                // Turn off automativ model state validation filter.
                // A custom validation filter will be used.
                //options.SuppressModelStateInvalidFilter = true;
            });

            // Database configuration
            services.AddDbContext<SocialMediaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SocialMedia")));

            // (9) Register which implementation to resolve when injectin the post service.
            services.AddTransient<IPostService, PostService>();

            // Define which implementation of the interface (abstraction) IPostRepository to use.
            // We will resolve an instance of the implementation of PostRpository.
            services.AddTransient<IPostRepository, PostRepository>();
            // In case we want to change from database provider we would need to work with a different implementation, then we should resolve the other implementation
            //services.AddTransient<IPostRepository, PostMongoRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            // Add MVC compatibility for using global action filters (this application is not an MVC application, it is just an API).
            // Global actions filters are code that gets executed before and after the code that resides inside every controller.
            services.AddMvc(options =>
            {
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(options => {
                // Automatically register validators
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
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
