using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Factor.Models.ViewModels
{
    public class FactorCostViewModels
    {
        public class Add
        {
            public int factorId { get; set; }
            public int factorCostId { get; set; }
            public string value { get; set; }
        }
    }
}