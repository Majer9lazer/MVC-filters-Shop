using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MVC_Filters.Models;

namespace MVC_Filters
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AllTypeExceptionAtrribute());
        }
    }
}
