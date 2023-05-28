using System;
using System.Web.Mvc;

namespace Web.Insfrastructure.Utilities
{
    public class IsAjaxAttribute:ActionFilterAttribute//,IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
             base.OnActionExecuting(filterContext);   
            else
                throw new InvalidOperationException("فقط از طریق درخواست اجکس می توانید به این تابع دسترسی داشته باشید.");
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}