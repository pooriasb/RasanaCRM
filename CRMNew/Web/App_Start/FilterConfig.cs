﻿using System.Web.Mvc;

namespace Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
           // filters.Add(new Filterss.UserActionFilter());
        }
    }
}
