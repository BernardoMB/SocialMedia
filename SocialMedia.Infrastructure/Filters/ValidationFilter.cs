using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Filters
{
    /**
     * This class in an action filter.
     * Action filters are used to execute code before and after the controller execution.
     * Action filters help us making validations to the model state of the request response cycle.
     */
    public class ValidationFilter : IAsyncActionFilter
    {
        /**
         * This method will be executing before and after the code of the controller.
         */
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Validate model state.
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }
            // Continue with the normal flow of the request response cycle.
            await next();
        }
    }
}
