using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MVC_Filters.Controllers;
using MVC_Filters.Models;
using WebGrease.Activities;

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
            string ipAddress = Dns.GetHostAddresses(Dns.GetHostName()).Last(w => w.AddressFamily == AddressFamily.InterNetwork).ToString();



            return ipAddress;

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


                Task t = new Task(() =>
                 {
                     referrer = referrer ?? "Index";
                     User u = new User()
                     {
                         DateOfVisiting = dateOfVisit,
                         PageOfVisiting = referrer,
                         User_Ip = ip,
                         UserName = userName
                     };
                     _dbEntities.Users.Add(u);
                     _dbEntities.SaveChanges();
                 });
                t.Start();

            }
            catch (Exception e)
            {
                LogError error = (exception, message, name) => { };
                error.Invoke(e, e.Message, Guid.NewGuid().ToString());

            }
        }


    }
}