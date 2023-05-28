using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Web.Models.Entity;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class CustomerRelationViewModels
    {
        public class Add    
        {
            public int id { get; set; }

            [StringLength(255)]
            [Required(ErrorMessage = "فیلد {0} الزامی است")]
            [Display(Name = "نام")]
            public string name { get; set; }

            [StringLength(255)]
            [Required(ErrorMessage = "فیلد {0} الزامی است")]
            [Display(Name = "نام خانوادگی")]
            public string family { get; set; }

            [StringLength(10, ErrorMessage = "فیلد کد ملی 10 رقمی می باشد")]
            [Display(Name = "کد ملی")]
            public string nationalCode { get; set; }

            [StringLength(11, ErrorMessage = "فیلد تلفن همراه 11 رقمی می باشد")]
            [Display(Name = "تلفن همراه")]
            public string phone { get; set; }

            [StringLength(11, ErrorMessage = "فیلد تلفن 11 رقمی می باشد")]
            [Display(Name = "تلفن")]
            public string tell { get; set; }

            [StringLength(11, ErrorMessage = "فیلد شماره فکس 11 رقمی می باشد")]
            [Display(Name = "شماره فکس")]
            public string fax { get; set; }

            [StringLength(10, ErrorMessage = "فیلد کد پستی 10 رقمی می باشد")]
            [Display(Name = "کد پستی")]
            public string postCode { get; set; }

            public int? customer_id { get; set; }
            [StringLength(255)]
            [Display(Name = "سمت")]
            public string Job { get; set; }

            public List<CustomerViewModels.AddDynamics> customerCategoryDynamic { get; set; }
            public List<CustomerViewModels.AddDynamicString> customerCategoryDynamicString { get; set; }
            public List<CustomerViewModels.AddDynamics> customerOrganizationDynamic { get; set; }
            public List<CustomerViewModels.AddDynamicString> customerOrganizationDynamicString { get; set; }

        }
        public class Edit
        {
            public int id { get; set; }

            [StringLength(255)]
            [Required(ErrorMessage = "فیلد {0} الزامی است")]
            [Display(Name = "نام")]
            public string name { get; set; }

            [StringLength(255)]
            [Required(ErrorMessage = "فیلد {0} الزامی است")]
            [Display(Name = "نام خانوادگی")]
            public string family { get; set; }

            [StringLength(10,ErrorMessage = "فیلد کد ملی 10 رقمی می باشد")]
            [Display(Name = "کد ملی")]
            public string nationalCode { get; set; }

            [StringLength(11, ErrorMessage = "فیلد تلفن همراه 11 رقمی می باشد")]
            [Display(Name = "تلفن همراه")]
            public string phone { get; set; }

            [StringLength(11, ErrorMessage = "فیلد تلفن 11 رقمی می باشد")]
            [Display(Name = "تلفن")]
            public string tell { get; set; }

            [StringLength(11, ErrorMessage = "فیلد شماره فکس 11 رقمی می باشد")]
            [Display(Name = "شماره فکس")]
            public string fax { get; set; }

            [StringLength(10,ErrorMessage = "فیلد کد پستی 10 رقمی می باشد")]
            [Display(Name = "کد پستی")]
            public string postCode { get; set; }

            public int? customer_id { get; set; }
            [StringLength(255)]
            [Display(Name = "سمت")]
            public string Job { get; set; }
            
            public List<CustomerViewModels.AddDynamics> customerCategoryDynamic { get; set; }
            public List<CustomerViewModels.AddDynamicString> customerCategoryDynamicString { get; set; }
            public List<CustomerViewModels.AddDynamics> customerOrganizationDynamic { get; set; }
            public List<CustomerViewModels.AddDynamicString> customerOrganizationDynamicString { get; set; }
            public List<CustomersOptionValue> CustomersOptionValues { get; set; }

        }

        public class CustomerRelationJson
        {
            public string name { get; set; }
            public string family { get; set; }
            public string nationalCode { get; set; }
            public string phone { get; set; }
            public string tell { get; set; }
            public string fax { get; set; }
            public string postCode { get; set; }
            public string Job { get; set; }
            public int customerRelationId { get; set; }
        }
    }
}