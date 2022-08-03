using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using System.Text.Json;

namespace Backend.Validators
{
    public class ValidatorActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var validationResult = new ContentResult();
                var sb = new StringBuilder();

                foreach (var valuePair in context.ModelState)
                {
                    sb.Append(valuePair.Value.Errors.Select(x => x.ErrorMessage).Aggregate((a, b) => a + b) + " ");
                }

                string error = sb.ToString();

                string content = JsonSerializer.Serialize(new { error }); 
                validationResult.Content = content;
                validationResult.ContentType = "application/json";

                context.HttpContext.Response.StatusCode = 422; 
                context.Result = validationResult;
                return;
            }

            await next();
        }
    }
}
