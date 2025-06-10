using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class RequestResponseLoggingFilter : IAsyncActionFilter
{
    private readonly ILogger<RequestResponseLoggingFilter> _logger;

    public RequestResponseLoggingFilter(ILogger<RequestResponseLoggingFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
       
        var request = context.HttpContext.Request;
        _logger.LogInformation("Handling request: {Method} {Path}", request.Method, request.Path);

       
        var executedContext = await next();

      
        var response = context.HttpContext.Response;
        _logger.LogInformation("Response status code: {StatusCode}", response.StatusCode);
    }
}
