using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Catalog.API.Filters
{
    public class ResponseMappingFilter : IActionFilter
    {
        /// <summary>
        ///  Will map any ErrorStatusCode to http result status code
        /// </summary>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult { Value: CatalogApiResponse response } && response.StatusCode != HttpStatusCode.OK)
            {
                context.Result = new ObjectResult(new { response.ErrorMessage }) 
                { 
                    StatusCode = (int)response.StatusCode
                };
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // no need to implement logic here
        }
    }
}
