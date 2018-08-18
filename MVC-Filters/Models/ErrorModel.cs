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
        public string Id { get; set; } = System.Guid.NewGuid().ToString();
        public bool IsAuthenticated { get; set; }

        public string DateOfError { get; set; }
        public Exception Error { get; set; }
        public string StackTrace { get; set; }

        public string ErrorCode { get; set; }

    }
}