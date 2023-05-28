using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models.Entity;

namespace Web.Areas.Employee.Models.ViewModels
{

    public class CustomerViewModels
    {
        public class Add
        {
            [Required(ErrorMessage = "لطفا فیلد کد را پر کنید", AllowEmptyStrings = false)]
            public int id { get; set; }
            public int? localId { get; set; }
            [Required(ErrorMessage = "فیلد {0} الزامی است", AllowEmptyStrings = false)]
            [StringLength(255)]
            [Display(Name = "نام")]
            public string name { get; set; }
            [StringLength(10,ErrorMessage = "فیلد کد ملی 10 کارکتری می باشد")]
            [RegularExpression("[0-9]*",ErrorMessage = "کد ملی رقمی می باشد")]
            [Display(Name = "کد ملی")]
            public string nationalCode { get; set; }
            [StringLength(255)]
            [Display(Name = "شماره حساب")]
            public string accountNumber { get; set; }
            [StringLength(11,ErrorMessage = "فیلد موبایل 11 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "کد موبایل می باشد")]
            [Display(Name = "موبایل")]
            public string phone { get; set; }

            [StringLength(11, ErrorMessage = "فیلد {0} 11 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "تلفن1 رقمی می باشد")]
            [Display(Name = "تلفن 1")]
            public string tell { get; set; }
            [StringLength(11, ErrorMessage = "فیلد {0} 11 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "تلفن2 رقمی می باشد")]
            [Display(Name = "تلفن 2")]
            public string tell1 { get; set; }
            [StringLength(11, ErrorMessage = "فیلد {0} 11 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "تلفن رقمی می باشد")]
            [Display(Name = "تلفن 3")]
            public string tell2 { get; set; }

            [StringLength(11, ErrorMessage = "فیلد شماره تلفن 11 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "فکس رقمی می باشد")]
            [Display(Name = "شماره فکس")]
            public string fax { get; set; }

            [StringLength(10, ErrorMessage = "فیلد کد پستی 10 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "کد پستی رقمی می باشد")]
            [Display(Name = "کد پستی")]
            public string postCode { get; set; }

            [StringLength(255)]
            [Display(Name = "شماره اقتصادی")]
            public string economicCode { get; set; }

            [Required(ErrorMessage = "فیلد {0} الزامی است")]
            [Display(Name = "استان")]
            public int? province_id { get; set; }

            [Required(ErrorMessage = "فیلد {0} الزامی است")]
            [Display(Name = "شهر")]
            public int? city_id { get; set; }

            [StringLength(400)]
            [Display(Name = "آدرس")]
            public string address { get; set; }
            [StringLength(400)]
            [Display(Name = "آدرس1")]
            public string address1 { get; set; }
            [StringLength(400)]
            [Display(Name = "آدرس2")]
            public string address2 { get; set; }
            [Display(Name = "کد")]
            public int? code { get; set; }
            [Display(Name = "توضیحات")]
            public string description { get; set; }
            [Required(ErrorMessage = "لطفا فیلد گروه مشتریان را پر کنید",AllowEmptyStrings = false)]
            public List<int> customerCategory_id { get; set; }
            [Required(ErrorMessage = "لطفا فیلد واحد سازمانی را پر کنید", AllowEmptyStrings = false)]
            public List<int> organizationUnit_id { get; set; }

            public List<AddDynamics> customerCategoryDynamic { get; set; }
            public List<AddDynamicString> customerCategoryDynamicString { get; set; }
            public List<AddDynamics> customerOrganizationDynamic { get; set; }
            public List<AddDynamicString> customerOrganizationDynamicString { get; set; }
        }
        public class Edit
        {
            [Required(ErrorMessage = "لطفا فیلد کد را پر کنید",AllowEmptyStrings = false)]
            public int id { get; set; }
            public string cityName { get; set; }

            public int? localId { get; set; }
            [Required(ErrorMessage = "فیلد {0} الزامی است", AllowEmptyStrings = false)]
            [StringLength(255)]
            [Display(Name = "نام")]
            public string name { get; set; }
            [StringLength(10, ErrorMessage = "فیلد کد ملی 10 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "کد ملی رقمی می باشد")]
            [Display(Name = "کد ملی")]
            public string nationalCode { get; set; }
            [StringLength(255)]
            [Display(Name = "شماره حساب")]
            public string accountNumber { get; set; }
            [StringLength(11, ErrorMessage = "فیلد موبایل 11 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "موبایل رقمی می باشد")]
            [Display(Name = "موبایل")]
            public string phone { get; set; }

            [StringLength(11, ErrorMessage = "فیلد {0} 11 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "تلفن1 رقمی می باشد")]
            [Display(Name = "تلفن 1")]
            public string tell { get; set; }
            [StringLength(11, ErrorMessage = "فیلد {0} 11 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "تلفن2 رقمی می باشد")]
            [Display(Name = "تلفن 2")]
            public string tell1 { get; set; }
            [StringLength(11, ErrorMessage = "فیلد {0} 11 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "تلفن3 رقمی می باشد")]
            [Display(Name = "تلفن 3")]
            public string tell2 { get; set; }

            [StringLength(11, ErrorMessage = "فیلد شماره تلفن 11 کارکتری می باشد")]
            [Display(Name = "شماره فکس")]
            public string fax { get; set; }

            [StringLength(10, ErrorMessage = "فیلد کد پستی 10 کارکتری می باشد")]
            [RegularExpression("[0-9]*", ErrorMessage = "فکس رقمی می باشد")]
            [Display(Name = "کد پستی")]
            public string postCode { get; set; }

            [StringLength(255)]
            [Display(Name = "شماره اقتصادی")]
            public string economicCode { get; set; }
            [Required(ErrorMessage = "فیلد {0} الزامی است")]
            [Display(Name = "استان")]
            public int? province_id { get; set; }
            [Required(ErrorMessage = "فیلد {0} الزامی است")]
            [Display(Name = "شهر")]
            public int? city_id { get; set; }
            [StringLength(400)]
            [Display(Name = "آدرس")]
            public string address { get; set; }
            [StringLength(400)]
            [Display(Name = "آدرس1")]
            public string address1 { get; set; }
            [StringLength(400)]
            [Display(Name = "آدرس2")]
            public string address2 { get; set; }
            [Display(Name = "کد")]
            public int? code { get; set; }
            [Display(Name = "توضیحات")]
            public string description { get; set; }
            //[Required(ErrorMessage = "لطفا فیلد گروه مشتریان را پر کنید", AllowEmptyStrings = false)]
            public List<int> customerCategory_id { get; set; }
            //[Required(ErrorMessage = "لطفا فیلد واحد سازمانی را پر کنید", AllowEmptyStrings = false)]
            public List<int> organizationUnit_id { get; set; }
            public List<AddDynamics> customerCategoryDynamic { get; set; }
            public List<AddDynamicString> customerCategoryDynamicString { get; set; }
            public List<AddDynamics> customerOrganizationDynamic { get; set; }
            public List<AddDynamicString> customerOrganizationDynamicString { get; set; }
            public List<CustomersOptionValue> CustomersOptionValues { get; set; }
        }
        public class CustomerJson
        {
            public string description { get; set; }
            public int id { get; set; }
            public int? localId { get; set; }
            public string name { get; set; }
            public string nationalCode { get; set; }
            public string accountNumber { get; set; }
            public string phone { get; set; }
            public string tell { get; set; }
            public string tell1 { get; set; }
            public string tell2 { get; set; }
            public string fax { get; set; }
            public string postCode { get; set; }
            public string economicCode { get; set; }
            public int? province_id { get; set; }
            public int? city_id { get; set; }
            public string provinceName { get; set; }
            public string cityName { get; set; }
            public string address { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public int? code { get; set; }
            public DateTime dateTime { get; set; }
            public string dateTimePersian { get; set; }
            public List<CustomerRelationViewModels.CustomerRelationJson> CustomerRelationJsons { get; set; }
        }

        #region Dynamic
        public class EditDynamicSelect
        {
            public string optionName { get; set; }
            public int optionId { get; set; }
            public SiteValue values { get; set; }
        }

        public class AddDynamics
        {
            //[Required(ErrorMessage = "لطفا این فیلد را پر کنید", AllowEmptyStrings = false)]
            public int? selectTagName { get; set; }
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
            //[Required(ErrorMessage = "لطفا این فیلد را پر کنید", AllowEmptyStrings = false)]
            public int? selectTagName { get; set; }
            public int selectTagId { get; set; }
        }

        #endregion


    }


}