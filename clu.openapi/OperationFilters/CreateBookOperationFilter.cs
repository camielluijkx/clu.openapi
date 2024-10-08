﻿using clu.openapi.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace clu.openapi.OperationFilters
{
    public class CreateBookOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId != "CreateBook")
            {
                return;
            }

            operation.RequestBody.Content.Add(
                "application/clu.openapi.bookforcreationwithamountofpages+json",
                new OpenApiMediaType
                {
                    Schema = context.SchemaRegistry.GetOrRegister(typeof(BookForCreationWithAmountOfPages))
                });
        }
    }
}