using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace MVC_Filters.Models
{
    public class ErrorModel
    {
        /// <summary>
        /// Зарегестрированный ли пользователь
        /// </summary>
        public bool IsAuthenticated { get; set; }

        public DateTime DateOfError { get; set; }
        public Exception Error { get; set; }
        public string StackTrace { get; set; }

        public string ErrorCode { get; set; }

    }
}