using System;
using System.IO;
using System.Reflection;
using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;

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
* (12) Filtering Data and Return Types
* (13) Implementing Pagination
* (14) Configuration settings
* (15) Documenting API with Swagger
* (16) Securing API with JWT
* (17) Deploying API in Azure and IIS
* (18) Register and Login User
* (19.1) Generate Hashing Passwords
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
			services.AddControllers(
			// (11) Register exception filters
			options =>
			{
				// (11) Configure the response when there has been 
				options.Filters.Add<GlobalExceptionFilter>();
			})
			// Use NewtonsoftJson to work with Json objects in our application
			.AddNewtonsoftJson(options =>
			{
				// Ignore circular reference error.
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
				// (14) Not every ApiResponse instance should include the Meta property, then do the following:
				options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore; // Delete null properties.
			})
			// (7) Configure the behavior of the API
			.ConfigureApiBehaviorOptions(options =>
			{
				// (7) Disbale ApiController decorator automatic validation for manualy validating the model state (DTOs).
				// (7) Turn off automatic model state (DTOs) validation filter to ignore data anotations (decorators) in the model.
				// options.SuppressModelStateInvalidFilter = true;
				// (7) The previous line is commented out because we actually want to use the model state validation so we get
				// better error messages in case the model state (DTO) is invalid.
				// (7) This automatic validation would not be implemented using property decorators (data anotations), instead
				// we shall use a custom validation filter.
			});

			// (14) Configure using configuration values using the appsettings.json file.
			// (14) We want to have all the pagination configuration mapped onto a class, that class would be PaginationOptions (mapping is automatic).
			services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));

			// Database configuration
			services.AddDbContext<SocialMediaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SocialMedia")));

			#region Register services
			// (9) Register which implementations to resolve when injecting services.
			services.AddTransient<IPostService, PostService>();
			// (9) The trasient is for injecting services whose instance is created on every request.
			// (18) Register the security
			services.AddTransient<ISecurityService, SecurityService>();
			// (14) Register the URI Service as a singleton service.
			// (14) Adding a singleton dependency means that the application will only use one instance of the service during it hole lifetime.
			// (14) We don't need to create an instance of the service every time we get a request.
			// services.AddSingleton<IUriService, UriService>();
			// (14) The UriService has one parameter of type string in it constructor. When injecting this singleton service we must provide
			// the appropiate parameters into the constructor. Therefore, better do the following:
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

			// (15) Configure Swashbuckle.AspNetCore package for generating Swagger documentation.
			services.AddSwaggerGen(doc =>
			{
				// (15) Generate API Documentation:
				doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1" });
				// (15) Configura Swagger documentation to include xml comments (comments above methods) for documentation.
				// (15) The line below uses the Reflection API (using System.Reflection;).
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				Console.WriteLine($"Xml comments file path: {xmlPath}");
				doc.IncludeXmlComments(xmlPath);
			});

			// (16) Add authentication
			// (16) This must be added before configuring MVC. Most middlewares must be configured before adding the MVC.
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options => {
				// Configure how to validate tokens
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					// (16) Specify valid values:
					ValidIssuer = Configuration["Authentication:Issuer"],
					ValidAudience = Configuration["Authentication:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
					// (16) See controllers to see how endpoints are secured.
				};
			});

			// (7) Add MVC compatibility for using global action filters (this application is not an MVC application, it is just an API).
			// (7) Global actions filters are code that gets executed before and after the code that resides inside every controller.
			services.AddMvc(options =>
			{
				// (7) Register the validation filter.
				// (7) This will register our ValidationFilter.cs class.
				options.Filters.Add<ValidationFilter>();
			}).AddFluentValidation(options =>
			{
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

			// (15) Configura Swagger documentation.
			// (15) Visit https://localhost:44310/swagger/v1/swagger.json to view documentation.
			app.UseSwagger();

			// (15) Configure the applicatino to serve the visual Swagger documentation.
			// (15) Visit https://localhost:44310/swagger to view visual Swagger documentation.
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("../swagger/v1/swagger.json", "Social Media API V1");
				// (15) Start application showing the documentation instead of making a post request to the api/Post endpoint.
				// options.RoutePrefix = string.Empty;
			});

			app.UseRouting();

			// (16) Use authentication
			// (16) Authentication must come first before the UseAuthorization call.
			app.UseAuthentication();

			app.UseAuthorization(); // Not being used.

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
