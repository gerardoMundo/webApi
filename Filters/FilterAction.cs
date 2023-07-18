using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class FilterAction : IActionFilter
    {
        private readonly ILogger<FilterAction> logger;

        public FilterAction(ILogger<FilterAction> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Se ejecuta durante el lanzamiento");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Se ejecuta despues el lanzamiento");

        }
    }
}
