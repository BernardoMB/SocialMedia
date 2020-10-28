This project was built using the following tutorial series:

RESTFul API with .NET Core 3.1 by Code Skills

## Configuration

In the `appsettings.json` one can add any configuratino value.
App pagination default behavior is configured in this `appsettings.json` file.

## Project Dependencies

Read dependencies description in each project's Nuget Package Manager 

### SocialMedia.Api

Newtonsoft: For ignoring circular reference error dealing with database entities.
ComponentModel.DataAnnotations: For using decorators inside the dto classes.
Swashbuckle.AspNetCore: For generating Swagger documentation.

### SocialMedia.Core

This project shohuld not have many dependencies because of good practices.
Microsoft.Extensions.Options: For using configuration values in appsettings.json

### SocialMedia.Infrastructure

Automapper: For mapping dtos into domain entities
Fluent validation: Validate classes and dto. Provides a simpler and more practical way for class validation and implementation.

## Documentation

Documentation is generated in the following route once the application is running: https://localhost:44310/swagger/v1/swagger.json
The returned json file can be downloaded and imported in https://editor.swagger.io to generate the documentation.