using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Infrastructure
{
    public class InvertedLayoutAttribute : ActionFilterAttribute 
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.Controller is Controller controller)
            {
                controller.ViewData[Settings.LayoutInvertedKey] = true;
			}
		}
	}
}
