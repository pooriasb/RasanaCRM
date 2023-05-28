using System;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class ProductOptionValueViewModels
    {
        public class GetHeaderOption
        {
            public int id { get; set; }
            public int productId { get; set; }
            public int optionId { get; set; }
            public string productName { get; set; }
            public string optionValue { get; set; }
            public string optionName { get; set; }
        }
        public class Pov
        {
            public int povId { get; set; }
            public int? povValueId { get; set; }
            public string povValueName { get; set; }
            public string povStrValue { get; set; }
            public int povOptionId { get; set; }
            public string povOptionName { get; set; }
            public bool isString { get; set; }
            public bool isRequired { get; set; }
            public string type { get; set; }
        }
        
    }
}