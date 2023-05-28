using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class EmployeeViewModels
    {
        public class GetEmployeeForJson
        {
            public string id { get; set; }
            public string userName { get; set; }
            public string phone { get; set; }
        }
    }
}