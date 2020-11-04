using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Options;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SocialMedia.Infrastructure.Extensios
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SocialMediaContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SocialMedia")));
            return services;
        }
        
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            // services.Configure<PaginationOptions>(configuration.GetSection("Pagination"));
            // Need to pick up the same method but with the correct version
            services.Configure<PaginationOptions>(options => configuration.GetSection("Pagination").Bind(options));
            // TODO: Learn about the Bind method.
            // services.Configure<PasswordOptions>(configuration.GetSection("PasswordOptions"));
            // Need to do the same thing as the code above
            services.Configure<PasswordOptions>(options => configuration.GetSection("PasswordOptions").Bind(options));
            return services;
        }
        
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IUriService>(provider =>
            {
                // (14) Get access to the Http context of this application.
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                // (14) The HttpContextAccessor allow us to access the http context of the application.
                var request = accesor.HttpContext.Request; // Get access to the request object.
                var requestScheme = request.Scheme; // Http or Https.
                var host = request.Host.ToUriComponent(); // Host of the application (Ej. 127.0.0.1:44310 when running locally).
                var absoluteUri = string.Concat(requestScheme, "://", host);
                Console.WriteLine("Absolute URI:" + absoluteUri);
                // (14) Because we are injecting a sinlgeton service, we must return an instance of the implementation.
                return new UriService(absoluteUri);
            });
            // (20) Repositories can be added here or on its own extension method.]
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
        
        public static IServiceCollection AddSwagger(this IServiceCollection services, string xmlFileName)
        {
            services.AddSwaggerGen(doc =>
            {
                // (15) Generate API Documentation:
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1" });
                // (15) Configura Swagger documentation to include xml comments (comments above methods) for documentation.
                // (15) The line below uses the Reflection API (using System.Reflection;).
                // var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // (20) Grab the file name from function parameters
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                Console.WriteLine($"Xml comments file path: {xmlPath}");
                doc.IncludeXmlComments(xmlPath);
            });
            return services;
        }
    }
}
