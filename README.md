# Documenting an ASP.NET Core API with OpenAPI / Swagger
Fully functioning finished sample code for my Documenting an ASP.NET Core API with Swagger / OpenAPI course.  

The code contains everything from basic documentation with OpenAPI / Swagger (using Swashbuckle.AspNetCore) over dealing with not-so-obvious use cases, ApiExplorer, authentication and versioning right up to customizing the UI.

Please note that the entire example application is licensed under MIT open source license and originally forked from Kevin Dockx' repository DocumentingAspNetCoreApisWithOpenAPI available at https://github.com/KevinDockx/DocumentingAspNetCoreApisWithOpenAPI and customized to match my own learning goals.

You can find the course over at Pluralsight: https://app.pluralsight.com/library/courses/aspdotnet-core-api-openapi-swagger/table-of-contents

The example application is built with ASP.NET Core 2.2 which is outdated nowadays regarding ended support, it differs however not much from using .NET 8.0 and ASP.NET Core 8.0 so it's still very usefull. Furthermore it is using Automapper and EF Core dependencies, whose usage is not part of the course. Obviously more techniques are applied like using dependency injection, async/await pattern, authentication + identity claims, data annotations, XML documentation, REST API, DDD and EF migrations which are not coverered and considered standard. Feel free to extend or upgrade the example application for the purpose of learning.

For starters use Tools > NuGet Package Manager > Package Manager Console > 'Update-Database' command to initially create database 'LibraryDB' on (localdb)\MSSQLLocalDB.

Also you will need to accept installation of self-signed certificate for your local development environment, unless you make changes to be able to use http yourself (launchSettings.json + UseHttpsRedirection). 

After compilation API documentation should be avalailable at https://localhost:44305/index.html or http://localhost:51415/index.html which will be redirected. 

Authentication is required and for dempo purposes simplified using basic authentication (username = Pluralsight, password = Pluralsight).

Postman collection with example requests for the different API endpoint invocations is available on request.

Big thanks to Kevin Dockx for providing the course and sample application.


** Additional Course Notes **

Getting Started with OpenAPI / Swagger

	OpenAPI (formerly known as Swagger Specification) is a specification for defining RESTful APIs in a standardized format. It provides a machine-readable format for describing the structure of APIs, including endpoints, request/response formats, parameters, authentication methods, and more.

	Swagger UI is a tool that automatically generates a user-friendly interface for interacting with APIs defined by OpenAPI specifications. It allows developers and testers to explore API endpoints and see request and response formats in a visually appealing way.

	SwaggerGen is a tool within the Swashbuckle library that generates OpenAPI specifications (formerly known as Swagger specifications) based on the ASP.NET Core application’s controllers and models.

	Swashbuckle is a library for .NET applications that simplifies the process of generating OpenAPI specifications and integrating Swagger UI into ASP.NET Core applications. It allows developers to automatically generate the OpenAPI document based on their ASP.NET Core controllers and models.

Documenting Your First API with OpenAPI / Swagger

	Please note API error responses in example application are following RFC 7807 standard, see https://datatracker.ietf.org/doc/html/rfc7807.

	Error response is using 'ProblemDetails' model having 'TraceId' property by default which can be extended using custom middleware implementation.

	Infer actual type using 'ActionResult<T>' instead of 'IActionResult' or declare returning type in 'ProducesResponseType' declaration.

	Response code custom explanation can be overwritten using XML comment, see /// <response code="200">Returns the requested book</response>.  

	Use Microsoft.AspNetCore.Mvc.Api.Analyzers nuget package installation to generate compilation warnings for API problems that might need your attention although it is not a complete solution because it is based on responses returned explicitly for controllers having [ApiController] attribute.

	[ProducesResponseType(StatusCodes.Status400BadRequest)] isn't specified as possible endpoint response because it is handled by customizing startup configuration to configure basic response types on a global level, see 'AddMvc' > 'Filters' and 'InvalidModelStateResponseFactory' (in case of validation errors), but it can also be set at (custom base) controller level.

	You can use 'ProducesDefaultResponseType' to declare any default response in case it is not specified but is not recommended because it might not alway be a 'ProblemDetails' model you want to return (for example 'StatusCodes.Status500InternalServerError').

Using and Overriding Conventions for OpenAPI Generation

	You can use 'ApiConventionMethod' and 'DefaultApiConventions' or custom conventions (using 'ApiConventionNameMatch' = Any|Exact|Prefix|Suffix declaration) at assembly, controller or endpoint level whenever applicable but keep in mind conventions are overridden by attributes and mistakes can have dire consequences. It is a better approach to use attributes that follow common practices, such as not fully relying on API Explorer, using specific response types instead of default and applying attributes at a global level.

	Content negotiation is about resource representation (for example JSON vs XML format) for a resource available at same URI using accept header (for example 'application/json' vs 'application/xml' type) not strictly obligatory for REST API. Media type text/plain for accept header value is not supported by default and additional output formatter (JsonOutputFormatter, XmlSerializerOutputFormatter) registered in startup resulting in a 'StatusCodes.Status406NotAcceptable' when 'ReturnHttpNotAcceptable' is set to true. Leaving this option false (default) is considered bad practice because it will simply return data in potential incorrect requested media type (default is application/json). To avoid text/plain being selectable for response body (media) type in Swagger UI controllers are annotated with a 'Produces' specific response body (media) type attibute. Endpoints can be decorated with 'Consumes' (input), 'Produces' (output) and custom attributes to support any (media) type whenever desired for operations using a request body.

Generating OpenAPI Specifications for Advanced Input and Output Scenarios

	Content negotiation with vendor-specific media types is used to provide more information about the actual type instead of only specifying the data format (for example 'application/clu.openapi.book+json' vs 'application/clu.openapi.bookwithconcatenatedauthorname+json' type). For Swagger UI this means varying schema from the specification by media type which is supported since OpenAPI 3 and might be supported since current SwashBuckle version. For older versions this might not be the case which might require a workaround implementation using custom 'ResolveConflictingActions' that might be useful in any case to have a better understanding why API description generation is failing.
	
	A better solution to fix this problem is by implementing operation filters following interface 'IOperationFilter' and to include them in 'AddSwaggerGen' startup configuration. Operation filters can be used to customize the OpenAPI documentation generation process by modifying the operations of API endpoints (in this case to add additional supported request body media type).
	
Dealing with Different Versions and Protecting the Documentation

	TODO
	
Improving your Documentation with Advanced Customization

	TODO