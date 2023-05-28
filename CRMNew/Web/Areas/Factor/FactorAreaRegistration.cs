using System.Web.Mvc;

namespace Web.Areas.Factor
{
    public class FactorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Factor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Factor_default",
                "Factor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Web.Areas.Factor.Controllers" }
            );
        }
    }
}