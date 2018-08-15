using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Filters.Models;

namespace MVC_Filters
{

    public class ActionFilterAttribute : FilterAttribute, IActionFilter
    {
        private UsersLogingEntities _dbEntities = new UsersLogingEntities();


        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
        private string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        //перед вызовом метода действия
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var referrer = filterContext.HttpContext.Request.Headers["Referer"];
            var ip = GetIPAddress();
            var userName = Environment.UserName;
            var dateOfVisit = DateTime.Now;
            try
            {
                User u = new User()
                {
                    DateOfVisiting = dateOfVisit,
                    PageOfVisiting = referrer,
                    User_Ip = ip,
                    UserName = userName
                };
                _dbEntities.Users.Add(u);
                _dbEntities.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }


    }
}