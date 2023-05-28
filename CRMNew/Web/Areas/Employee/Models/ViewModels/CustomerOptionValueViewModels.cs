using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Entity;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class CustomerOptionValueViewModels
    {
        public class IndexJson
        {
            public int id { get; set; }
            public string name { get; set; }
            public int parentId { get; set; }
            public List<IndexJson> SiteValue { get; set; }
        }
    }
}