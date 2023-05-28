using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.ViewModels.Enums;

namespace Web.Areas.Factor.Models.ViewModels
{
    public class FactorOptionValueViewModels
    {
        public class Add
        {
            public int optionId { get; set; }
            public int? valueId { get; set; }
            public string strValue { get; set; }
            public string type { get; set; }    
        }
        public class Edit
        {
            public int? id { get; set; }
            public int optionId { get; set; }
            public int? valueId { get; set; }
            public string strValue { get; set; }
            public string type { get; set; }
        }
    }
}