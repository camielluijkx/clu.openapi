# Documenting an ASP.NET Core API with OpenAPI / Swagger
Fully functioning finished sample code for my Documenting an ASP.NET Core API with Swagger / OpenAPI course.  

The code contains everything from basic documentation with OpenAPI / Swagger (using Swashbuckle.AspNetCore) over dealing with not-so-obvious use cases, ApiExplorer, authentication and versioning right up to customizing the UI.

Please note that the entire example application is licensed under MIT open source license and originally forked from Kevin Dockx' repository DocumentingAspNetCoreApisWithOpenAPI available at https://github.com/KevinDockx/DocumentingAspNetCoreApisWithOpenAPI and customized to match my own learning goals.

You can find the course over at Pluralsight: https://app.pluralsight.com/library/courses/aspdotnet-core-api-openapi-swagger/table-of-contents

The example application is built with ASP.NET Core 2.2 which is outdated nowadays regarding ended support, it differs however not much from using .NET 8.0 and ASP.NET Core 8.0 so it's still very usefull. Furthermore it is using Automapper and EF Core dependencies, whose usage is not part of the course. Obviously more techniques are applied like using dependency injection, async/await pattern, authentication + identity claims, data annotations, XML documentation, REST API, DDD and EF migrations which are not coverered and considered standard. Feel free to extend or upgrade the example application for the purpose of learning.

For starters use Tools > NuGet Package Manager > Package Manager Console > 'Update-Database' command to initially create database 'LibraryDB' on (localdb)\MSSQLLocalDB.

Also you will need to accept installation of self-signed certificate for your local development environment, unless you make changes to be able to use http yourself (launchSettings.json, ReturnHttpNotAcceptable, UseHttpsRedirection). 

After compilation API documentation should be avalailable at https://localhost:44305/index.html or http://localhost:51415/index.html which will be redirected. 

Authentication is required and for dempo purposes simplified using basic authentication (username = Pluralsight, password = Pluralsight).

Big thanks to Kevin Dockx for providing the course and sample application.