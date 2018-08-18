using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MVC_Filters.Models
{
    public class AllTypeExceptionAtrribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            bool isAuthenticated = exceptionContext.HttpContext.User.Identity.IsAuthenticated;
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is IndexOutOfRangeException)
            {
                exceptionContext.Result = new RedirectResult("/Home/IndexOutOfRangeException");
                exceptionContext.ExceptionHandled = SendMessage("gersen.e.a@gmail.com", "Ошибка",
                    $"Уважаемый админ произошла ошибка на сервере. " +
                    $"Информация по ошибке будет приведена ниже\n" +
                    $"Error - {exceptionContext.Exception}");
            }
            else if (exceptionContext.Exception is Exception ex && !exceptionContext.ExceptionHandled)
            {
                exceptionContext.ExceptionHandled =
                    WriteError(ex, isAuthenticated);
                //ErrorModel errorInfoModel = new ErrorModel()
                //{
                //    DateOfError = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                //    Error = ex,
                //    ErrorCode = ex.HResult.ToString(),
                //    IsAuthenticated = isAuthenticated,
                //    StackTrace = ex.TargetSite.Name
                //};
                exceptionContext.Result = new RedirectResult("/Home/Error");

            }
        }

        private bool SendMessage(string to, string subject, string message)
        {
            try
            {
                Task sendMessageTask = new Task(() =>
                  {
                      MailAddress from = new MailAddress("csharp.sdp.162@gmail.com", "Система отправки возникших ошибок");
                      MailAddress toAddress = new MailAddress(to);
                      MailMessage m = new MailMessage(from, toAddress)
                      {
                          Subject = subject,
                          Body = message
                      };
                      SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                      {
                          Credentials = new NetworkCredential("csharp.sdp.162@gmail.com", "sdp123456789"),
                          EnableSsl = true
                      };
                      smtp.Send(m);
                  });

                sendMessageTask.Start();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool WriteError(Exception ex, bool isUserAuthenticated)
        {
            bool isNoErrors;
            try
            {
                DirectoryInfo info = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                XDocument xdoc = XDocument.Load(info.FullName + "/Models/ErrorLog.xml");
                XElement Id = new XElement("Id");
                XElement isAuthenticated = new XElement("IsAuthenticated");
                XElement dateOfError = new XElement("DateOfError");
                XElement error = new XElement("Error");
                XElement errorCode = new XElement("ErrorCode");
                XElement stackTrace = new XElement("StackTrace");
                XElement userErrorLog = new XElement("userErrorLog");
                Id.Value = Guid.NewGuid().ToString();
                isAuthenticated.Value = isUserAuthenticated.ToString();
                dateOfError.Value = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                error.Value = ex.ToString();
                errorCode.Value = ex.HResult.ToString();
                stackTrace.Value = ex.TargetSite.Name;
                userErrorLog.Add(isAuthenticated, dateOfError, error, stackTrace, errorCode,Id);
                xdoc.Root?.Add(userErrorLog);
                xdoc.Save(info.FullName + "/Models/ErrorLog.xml");
                isNoErrors = true;

            }
            catch (Exception)
            {
                isNoErrors = false;
            }

            return isNoErrors;
        }
    }
}