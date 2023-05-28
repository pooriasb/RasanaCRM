using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Factor.Models.ViewModels
{
    public class FactorItemCostViewModels
    {
        public class FactorItemCost
        {
            public int id { get; set; }
            public string value { get; set; }
        }
        public class FactorItemCostAfters
        {
            public List<FactorItemCostAfter> ItemCostAfters { get; set; }
        }
        public class FactorItemCostAfter
        {
            public int? id { get; set; }
            public int factorItemId { get; set; }
            public int costId { get; set; }
            public string value { get; set; }
        }
    }
}