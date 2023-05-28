using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class ProductPriceViewModels
    {
        public class Add
        {
            [Required(ErrorMessage = "لطفا فیلد قیمت را تکمیل کنید",AllowEmptyStrings = false)]
            //[RegularExpression("[0-9]*",ErrorMessage = "لطفا فقط عدد وارد کنید")]
            public string price { get; set; }
            [Required(ErrorMessage = "لطفا فیلد واحد را تکمیل کنید", AllowEmptyStrings = false)]
            public int vahedId { get; set; }

            public string isDelete { get; set; }
        }
        public class ProductPriceJson
        {
            public string price { get; set; }
            public string vahedName { get; set; }
            public int? vahed_id { get; set; }
        }
        public class AddSingle
        {
            [Required(ErrorMessage = "لطفا فیلد قیمت را تکمیل کنید", AllowEmptyStrings = false)]
            //[RegularExpression("[0-9]*", ErrorMessage = "لطفا فقط عدد وارد کنید")]
            public string price { get; set; }
            [Required(ErrorMessage = "لطفا فیلد واحد را تکمیل کنید", AllowEmptyStrings = false)]
            public int vahedId { get; set; }

            public int id { get; set; }
        }
        public class Edit
        {
            public int id { get; set; }
            [Required(ErrorMessage = "لطفا فیلد قیمت را تکمیل کنید", AllowEmptyStrings = false)]
            //[RegularExpression("[0-9]*", ErrorMessage = "لطفا فقط عدد وارد کنید")]
            public string price { get; set; }
            [Required(ErrorMessage = "لطفا فیلد واحد را تکمیل کنید", AllowEmptyStrings = false)]
            public int vahedId { get; set; }
            public string vahedValue { get; set; }
        }
        public class EditProductPrice
        {
            public int productId { get; set; }
            [Required(ErrorMessage = "لطفا فیلد قیمت را تکمیل کنید", AllowEmptyStrings = false)]
            //[RegularExpression("[0-9]*", ErrorMessage = "لطفا فقط عدد وارد کنید")]
            public string price { get; set; }
            public string vahedName { get; set; }
            public int productPriceId { get; set; }
            public int vahedId { get; set; }

        }
    }
}