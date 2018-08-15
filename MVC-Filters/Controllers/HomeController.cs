using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Filters.Controllers
{
    public class HomeController : Controller
    {
        [ActionFilter]
        public ActionResult Index()
        {
            return View();
        }
        [ActionFilter]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [ActionFilter]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}