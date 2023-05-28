using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class SiteValuesViewModels
    {
        public class Add
        {
            public int? parentId { get; set; }
            public int code { get; set; }
            [Required(ErrorMessage = "لطفا فیلد نام را پر کنید",AllowEmptyStrings = false)]
            [MaxLength(255, ErrorMessage = "مقدار متن ورودی بیش از حد مجاز است . مقدار مجاز 255 کاراکتر میباشد")]

            public string name { get; set; }
            public string value { get; set; }
            public string description { get; set; }
            public bool isDelete { get; set; }
            public bool isEnable { get; set; }
        }
        public class AddSectionType
        {
            public int? parentId { get; set; }
            public int code { get; set; }
            [Required(ErrorMessage = "لطفا فیلد نام را پر کنید", AllowEmptyStrings = false)]
            [MaxLength(255, ErrorMessage = "مقدار متن ورودی بیش از حد مجاز است . مقدار مجاز 255 کاراکتر میباشد")]

            public string name { get; set; }
            public bool workFlow { get; set; }
            public string description { get; set; }
            public bool isDelete { get; set; }
            public bool isEnable { get; set; }
        }
        public class EditSectionType
        {
            public int id { get; set; }
            public int? parentId { get; set; }
            public int code { get; set; }
            [Required(ErrorMessage = "لطفا فیلد نام را پر کنید", AllowEmptyStrings = false)]
            [MaxLength(255, ErrorMessage = "مقدار متن ورودی بیش از حد مجاز است . مقدار مجاز 255 کاراکتر میباشد")]

            public string name { get; set; }
            public bool workFlow { get; set; }
            public string description { get; set; }
            public bool isDelete { get; set; }
            public bool isEnable { get; set; }
        }
        public class Edit
        {
            public int id { get; set; }
            public int? parentId { get; set; }
            public int code { get; set; }
            [Required(ErrorMessage = "لطفا فیلد نام را پر کنید", AllowEmptyStrings = false)]
            [MaxLength(255, ErrorMessage = "مقدار متن ورودی بیش از حد مجاز است . مقدار مجاز 255 کاراکتر میباشد")]

            public string name { get; set; }
            public string value { get; set; }
            [MaxLength(400, ErrorMessage = "مقدار متن ورودی بیش از حد مجاز است . مقدار مجاز 400 کاراکتر میباشد")]

            public string description { get; set; }
            public bool isDelete { get; set; }
            public bool isEnable { get; set; }
        }
        public class Gategories
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
        }
        public class IdName
        {
            public int id { get; set; }
            public string name { get; set; }
        }
        public class AddHeaderOption
        {
            public string TypeOptopn { get; set; }
            [Required(ErrorMessage = "لطفا محتوا را پر کنید",AllowEmptyStrings = false)]
            public string name { get; set; }
            public int subCategoryId { get; set; }
            public bool isRequired { get; set; }

        }
        public class AddOptionValue
        {
            public int optionNameId { get; set; }
            public List<string> value { get; set; }
        }


        #region Discount
        public class ShowDiscount
        {
            public string name { get; set; }
            public string description { get; set; }
            public bool isEnable { get; set; }
            public List<AddDiscountJson> json { get; set; }
        }
        public class AddDiscount
        {
            public int parentId { get; set; }
            [Required(ErrorMessage = "لطفا فیلد کم تر را پر کنید", AllowEmptyStrings = false)]
            public float left { get; set; }
            [Required(ErrorMessage = "لطفا فیلد بیشتر را پر کنید", AllowEmptyStrings = false)]
            public float right { get; set; }
            [Required(ErrorMessage = "لطفا فیلد مقدار را پر کنید", AllowEmptyStrings = false)]
            public string value { get; set; }
            public string description { get; set; }

            private List<SiteValuesViewModels.AddDiscountJson> _json;
            public List<SiteValuesViewModels.AddDiscountJson> json
            {
                get
                {
                    if (_json == null)
                    {
                        _json = new List<AddDiscountJson>()
                        {
                            new SiteValuesViewModels.AddDiscountJson()
                            {
                                right = this.right,
                                left = this.left,
                                value = this.value,
                            }
                        };
                        return _json;
                    }
                    else
                    {
                        return _json;
                    }
                }
                set
                {
                    new SiteValuesViewModels.AddDiscountJson()
                    {
                        right = this.right,
                        left = this.left,
                        value = this.value,
                    };
                }
            }
        }
        public class EditDiscount
        {
            [Required(ErrorMessage = "لطفا فیلد کم تر را پر کنید", AllowEmptyStrings = false)]
            public float left { get; set; }
            [Required(ErrorMessage = "لطفا فیلد بیشتر را پر کنید", AllowEmptyStrings = false)]
            public float right { get; set; }
            [Required(ErrorMessage = "لطفا فیلد مقدار را پر کنید", AllowEmptyStrings = false)]
            public string value { get; set; }
            public string description { get; set; }
            public string oldJson { get; set; }
            private List<AddDiscountJson> _json;
            public List<AddDiscountJson> json
            {
                get
                {
                    if (_json == null)
                    {
                        _json = new List<AddDiscountJson>()
                        {
                            new AddDiscountJson()
                            {
                                right = this.right,
                                left = this.left,
                                value = this.value,
                            }
                        };
                        return _json;
                    }
                    else
                    {
                        return _json;
                    }
                }
                set
                {
                    new AddDiscountJson()
                    {
                        right = this.right,
                        left = this.left,
                        value = this.value,
                    };
                }
            }
        }

        public class AddDiscountJson
        {
            public float left { get; set; }
            public float right { get; set; }
            public string value { get; set; }
        }

        #endregion

        //public class AddDiscountJsonAll
        //{
        //    public List<SiteValuesViewModels.AddDiscountJson> jsons { get; set; }
        //}
    }
}