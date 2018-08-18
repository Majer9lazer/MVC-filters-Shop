using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MVC_Filters.Models
{
    public class AllTypeExceptionAtrribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is IndexOutOfRangeException)
            {
                exceptionContext.Result = new RedirectResult("/Home/IndexOutOfRangeException");
                exceptionContext.ExceptionHandled = true;
            }
            else if (exceptionContext.Exception is Exception ex)
            {
                if (WriteError(ex))
                {
                    exceptionContext.ExceptionHandled = true;

                }
                else
                {
                    exceptionContext.ExceptionHandled = false;
                }
            }
        }

        private bool WriteError(Exception ex)
        {
            bool isNoErrors;
            try
            {
                DirectoryInfo info = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                XDocument xdoc = XDocument.Load(info.FullName + "/Models/ErrorLog.xml");
                XElement isAuthenticated = new XElement("IsAuthenticated");
                XElement dateOfError = new XElement("DateOfError");
                XElement error = new XElement("Error");
                XElement stackTrace = new XElement("StackTrace");
                XElement userErrorLog = new XElement("userErrorLog");
                isAuthenticated.Value = "false";
                dateOfError.Value = DateTime.Now.ToShortTimeString();
                error.Value = ex.ToString();
                stackTrace.Value = ex.StackTrace;
                userErrorLog.Add(isAuthenticated,dateOfError,error,stackTrace);
                xdoc.Root?.Add(userErrorLog);
                xdoc.Save(info.FullName + "/Models/ErrorLog.xml");
                isNoErrors = true;
            }
            catch (Exception e)
            {
                isNoErrors = false;
            }

            return isNoErrors;
        }
    }
}