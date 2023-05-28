using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Models.Entity;


namespace Web.Areas.Employee.Models.ViewModels
{
    public class ProductViewModels
    {
        public class Add
        {
            [Required(ErrorMessage = "لطفا فیلد نوع کالا را تکمیل کنید", AllowEmptyStrings = false)]
            public int categoryId { get; set; }
            [Required(ErrorMessage = "لطفا فیلد عنوان را پر کنید",AllowEmptyStrings = false)]
            [MaxLength(255, ErrorMessage = "مقدار متن ورودی بیش از حد مجاز است . مقدار مجاز 255 کاراکتر میباشد")]

            public string title { get; set; }
            public bool isEnable { get; set; }
            [MaxLength(400, ErrorMessage = "مقدار متن ورودی بیش از حد مجاز است . مقدار مجاز 400 کاراکتر میباشد")]

            public string description { get; set; }
            public float code { get; set; }
            public List<ProductPriceViewModels.Add> productPrices { get; set; }
            public List<AddDynamics> productDynamic { get; set; }
            public List<AddDynamicString> productDynamicString { get; set; }
        }
        public class AddDynamics
        {
            //[Required(ErrorMessage = "لطفا این فیلد را پر کنید", AllowEmptyStrings = false)]
            public int selectTagName { get; set; }
            //[Required(ErrorMessage = "لطفا این فیلد را پر کنید", AllowEmptyStrings = false)]
            public int selectTagId { get; set; }
        }
        public class AddDynamicString
        {
            public int textBoxId { get; set; }
            //[Required(ErrorMessage = "لطفا این فیلد را پر کنید", AllowEmptyStrings = false)]
            public string textBoxValue { get; set; }
        }
        public class EditDynamics
        {
            [Required(ErrorMessage = "لطفا این فیلد را پر کنید", AllowEmptyStrings = false)]
            public int selectTagName { get; set; }
            public int selectTagId { get; set; }
        }
        public class Edit
        {
            public int id { get; set; }
            public int categoryId { get; set; }
            [Required(ErrorMessage = "لطفا فیلد عنوان را پر کنید", AllowEmptyStrings = false)]
            [MaxLength(255,ErrorMessage ="مقدار متن ورودی بیش از حد مجاز است . مقدار مجاز 255 کاراکتر میباشد")]
            public string title { get; set; }
            public bool isEnable { get; set; }
            [Required(ErrorMessage = "لطفا فیلد کد را پر کنید", AllowEmptyStrings = false)]
            public float code { get; set; }
            [MaxLength(400, ErrorMessage = "مقدار متن ورودی بیش از حد مجاز است . مقدار مجاز 400 کاراکتر میباشد")]

            public string description { get; set; }
            public List<ProductPriceViewModels.EditProductPrice> productPrices { get; set; }
            public List<AddDynamics> productDynamic { get; set; }
            public List<AddDynamicString> productDynamicString { get; set; }

            //public float code { get; set; }
        }
        public class ProductJson
        {
            public int id { get; set; }
            public int localId { get; set; }
            public float code { get; set; }
            public string createDate { get; set; }
            public string title { get; set; }

            public string isEnable { get; set; }
            public int category_id { get; set; }
            public string user_id { get; set; }
            public string lastUpdateUser_id { get; set; }
            public string categoryName { get; set; }
            public string description { get; set; }
            public List<ProductPriceViewModels.ProductPriceJson> ProductPrices { get; set; }
        }
    }
}