using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Factor.Models.ViewModels
{
    public class FactorItemViewModels
    {
        public class Edit
        {
            public int productId { get; set; }
            public int factorId { get; set; }
            public int? factorItemId { get; set; }
            public int priceId { get; set; }
            public long priceVahed { get; set; }
            public long price { get; set; }
            public int count { get; set; }
            public long priceTotalItem { get; set; }
            public int garanty { get; set; }
            public int warranty { get; set; }
            public string description { get; set; }
            public List<FactorItemCostViewModels.FactorItemCost> FactorItemCosts { get; set; }
        }
       
    }
}