using clu.openapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace clu.openapi.OperationFilters
{
    public class GetBookOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId != "GetBook")
            {
                return;
            }

            operation.Responses[StatusCodes.Status200OK.ToString()].Content.Add(
                "application/clu.openapi.bookwithconcatenatedauthorname+json",
                new OpenApiMediaType
                {
                    Schema = context.SchemaRegistry.GetOrRegister(typeof(BookWithConcatenatedAuthorName))
                });

            operation.Responses[StatusCodes.Status200OK.ToString()].Content.Add(
               "application/clu.openapi.bookwithamountofpages+json",
               new OpenApiMediaType
               {
                   Schema = context.SchemaRegistry.GetOrRegister(typeof(BookWithAmountOfPages))
               });
        }
    }
}