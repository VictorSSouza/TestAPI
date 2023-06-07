using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogAPI.Filters
{
    public class APILoggingFilter : IActionFilter
    {
        private readonly ILogger<APILoggingFilter> _logger;
        public APILoggingFilter(ILogger<APILoggingFilter> logger)
        {
            _logger = logger;
        }

        // executa depois da Action
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("#### Executando -> OnActionExecuted");
            _logger.LogInformation("###################################");
            _logger.LogInformation($"{ DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
            _logger.LogInformation("###################################");
        }

        // executa antes da Action
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("#### Executando -> OnActionExecuting");
            _logger.LogInformation("###################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
            _logger.LogInformation("###################################");
        }
    }
}
