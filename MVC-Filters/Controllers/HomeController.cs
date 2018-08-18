using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using MVC_Filters.Models;
using MVC_Filters.Filters;
namespace MVC_Filters.Controllers
{

    public class HomeController : Controller
    {
        private readonly Random _rnd = new Random();
        [MyOwnActionFilter]
        [AllTypeExceptionAtrribute]
        public ActionResult Index()
        {
            switch (_rnd.Next(1, 9))
            {
                case 1: { throw new IndexOutOfRangeException(); }
                case 2: { throw new AccessViolationException(); }
                default: return View();
            }

        }
        [MyOwnActionFilter]
        public ActionResult IndexOutOfRangeException()
        {
            return View();
        }

        [MyOwnActionFilter]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Filters.MyOwnActionFilter]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult ErrorsStatistics()
        {
            return View(GetAllErrorsFromLogFile());
        }

        public ActionResult ErrorStatistics_Details(string id)
        {
            return View(GetAllErrorsFromLogFile().FirstOrDefault(f => f.Id == id));
        }

        private IEnumerable<ErrorModel> GetAllErrorsFromLogFile()
        {
            DirectoryInfo info = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            XDocument xdoc = XDocument.Load(info.FullName + "/Models/ErrorLog.xml");
            if (xdoc.Root != null && xdoc.Root.Elements().Any())
            {
                var errors = xdoc.Root.Elements().Select(s => new ErrorModel()
                {
                    IsAuthenticated = Convert.ToBoolean(s.Element("IsAuthenticated")?.Value.ToString()),
                    ErrorCode = s.Element("ErrorCode")?.Value.ToString(),
                    Error = new Exception(s.Element("Error")?.Value),
                    StackTrace = s.Element("StackTrace")?.Value.ToString(),
                    DateOfError = s.Element("DateOfError")?.Value.ToString(),
                    Id = s.Element("Id")?.Value.ToString()
                });
                return errors;
            }
            else
            {
                return null;
            }

        }

    }
}