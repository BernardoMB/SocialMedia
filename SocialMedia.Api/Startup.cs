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
 * (1) Summary
 * (2) Clean Arquitecture
 * (3) Dependency injection
 * (4) Fluent API in EF Core
 * (5) HTTP Basics and REST
 * (6) Data Transfer Objects and Automapper
 * (7) Web API Configuration and Fluent Validations
 * (8) CRUD Operations with EF Core
 * (9) Business Logic and Respository Pattern
 * (10) Generic Repository and Unit of Work
 * (11) Custom Exceptions
 */

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
         * Inside this method we will configure the application and declare middlewares.
         * This method will also specify which class implementations to use when using depedency injection.
         * Classes that depend on other services must inject the service in their constructor, however they will
         * only inject the abstraction of a class (the interface) in order to specify which implementation of
         * the abstractino the applicatino will use, one must use this method to determine the specific implementation to use.
         */
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure the automapper middleware and register all mappings.
            // Mappings profiles will be automatically detected thanks to the following sintax.
            // Any additional mapping will be automatically detected and registered.
            // Pass the assembliens so the AddAutoMapper function can search and register all existing mapping profiles.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Configure NewtonSoftJson package.
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Ignore circular reference error.
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            // (7) Configure the behavior of the API
            .ConfigureApiBehaviorOptions(options => {
                // (7) Disbale ApiController decorator automatic validation for manualy validating the model state (DTOs).
                // (7) Turn off automatic model state (DTOs) validation filter to ignore data anotations (decorators) in the model.
                // options.SuppressModelStateInvalidFilter = true;
                // (7) The previous line is commented out because we actually want to use the model state validation so we get
                // better error messages in case the model state (DTO) is invalid.
                // (7) This automatic validation would not be implemented using property decorators (data anotations), instead
                // we shall use a custom validation filter.
            });

            // Database configuration
            services.AddDbContext<SocialMediaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SocialMedia")));

            #region Register services
            // (9) Register which implementation to resolve when injecting the post service.
            services.AddTransient<IPostService, PostService>();
            #endregion

            #region Register repositories
            // Define which implementation to resolve when injecting an abstraction (an object implementing an interface) of our repository classes.
            //services.AddTransient<IPostRepository, PostRepository>();
            //services.AddTransient<IUserRepository, UserRepository>();
            // In case we want to change from database provider we would need to work with a different implementation, then we should resolve the other implementation.
            //services.AddTransient<IPostRepository, PostMongoRepository>();
            // (10) Better uase the generic class:
            // (10) Register the base repository so it can be injectable in other services.
            // (10) Add using scope. this will especify how many time the injectable object will be alive.
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            // (10) Register the unit of work
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            #endregion

            // (7) Add MVC compatibility for using global action filters (this application is not an MVC application, it is just an API).
            // (7) Global actions filters are code that gets executed before and after the code that resides inside every controller.
            services.AddMvc(options =>
            {
                // (7) Register the validation filter.
                // (7) This will register our ValidationFilter.cs class.
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(options => {
                // (7) Automatically register Fluent validation filters.
                // (7) Fluent is a Nuget package for making validations.
                // (7) This will register the PostValidator.cs class and any other class extending from AbstractValidator.
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
