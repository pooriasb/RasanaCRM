using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Factor.Models.ViewModels
{
    public class FactorViewModels
    {
        public class Add
        {
            public int productId { get; set; }
            public int factorId { get; set; }
            public int factorItemId { get; set; }
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
        public class Save
        {
            public int factorId { get; set; }
            public int code { get; set; }
            public int customerId { get; set; }
            public string date { get; set; }
            public int presentationId { get; set; }
            public bool isRasmi { get; set; }
            public int factorMessageId { get; set; }
            public string factorDescription { get; set; }
            public string paymentDescription { get; set; }
            public int expireDate { get; set; }
            public long priceTotalFactor { get; set; }
            public long priceFinish { get; set; }
            public int paymentTypeId { get; set; }
            public string deliveryPlace { get; set; }
            public string person { get; set; }
            public int statusId { get; set; }
            public int? addonCode { get; set; }
            public int? parentId { get; set; }
            public List<FactorOptionValueViewModels.Add> addDynamics { get; set; }
            public List<FactorCostViewModels.Add> addFactorCost { get; set; }
            public List<FactorItemCostViewModels.FactorItemCostAfters> factorCostAfters { get; set; }
        }
        public class Edit
        {
            public int factorId { get; set; }
            public int code { get; set; }
            public int customerId { get; set; }
            public string date { get; set; }
            public int presentationId { get; set; }
            public bool isRasmi { get; set; }
            public int factorMessageId { get; set; }
            public string factorDescription { get; set; }
            public string paymentDescription { get; set; }
            public int expireDate { get; set; }
            public long priceTotalFactor { get; set; }
            public long priceFinish { get; set; }
            public int paymentTypeId { get; set; }
            public string deliveryPlace { get; set; }
            public string person { get; set; }
            public int statusId { get; set; }
            public int? parentId { get; set; }
            public int? addonCode { get; set; }
            public List<FactorOptionValueViewModels.Edit> dynamicsEdit { get; set; }
            public List<FactorItemViewModels.Edit> FactorItem { get; set; }
            public List<FactorCostViewModels.Add> addFactorCost { get; set; }
            public List<FactorItemCostViewModels.FactorItemCostAfters> factorCostAfter { get; set; }
        }

    }
}