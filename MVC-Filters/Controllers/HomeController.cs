using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Filters.Models;

namespace MVC_Filters.Controllers
{

    public class HomeController : Controller
    {
        private readonly Random _rnd = new Random();

        [ActionFilter]
        [AllTypeExceptionAtrribute]
        public ActionResult Index()
        {
            throw new AccessViolationException();
            return View();
        }

        public ActionResult IndexOutOfRangeException()
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

        public ActionResult Error()
        {


            return View();
        }

    }
}