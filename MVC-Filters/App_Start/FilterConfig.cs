using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_Filters
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
