using System.Web.Mvc;
using System.Web.Routing;
using Web.Insfrastructure.ManagePermission;

namespace Web.Insfrastructure.ManagePermission.Filters
{
    public class UserActionFilter : ActionFilterAttribute
    {

        short[] userPermission;
        IManagePermission managePermission;
        public UserActionFilter()
        {



        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var session = filterContext.HttpContext.Session["permission"];
            if (session != null)
                userPermission = (short[])session;


            string controller = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();
            string area = (string)filterContext.RouteData.DataTokens["area"] ?? string.Empty;

            managePermission = new ManagePermission(userPermission);



            if (!managePermission.CheckAction(area, action, controller))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                { "controller", "Account" },
                { "action", "Login" }
                    });

            }

            base.OnActionExecuting(filterContext);
        }


    }

}