using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Web.Models.Entity;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class WorkFlowViewModels
    {
        public class Add
        {
            public int id { get; set; }
            [Required(ErrorMessage = "لطفا فیلد عنوان را پر کنید", AllowEmptyStrings = false)]
            [StringLength(255, ErrorMessage = "حداکثر کارکتر قابل ثبت 255 می باشد")]
            public string title { get; set; }
            [Required(ErrorMessage = "لطفا فیلد کد را پر کنید", AllowEmptyStrings = false)]
            public int code { get; set; }
        }
        public class SendJob
        {
            [Required(ErrorMessage = "لطفا فیلد نام کارمند را پر کنید", AllowEmptyStrings = false)]
            public string userId { get; set; }
            //id property is sectionType id
            public int id { get; set; }
            [Required(ErrorMessage = "لطفا فیلد پیام را پر کنید", AllowEmptyStrings = false)]
            public string message { get; set; }

            public int factorId { get; set; }
        }
        public class AgreeDenyWorkFlow
        {
            [Required(ErrorMessage = "لطفا همکار مورد نیاز را انتخاب کنید", AllowEmptyStrings = false)]
            public string userId { get; set; }
            [Required(ErrorMessage = "لطفا فیلد پیام را پر کنید", AllowEmptyStrings = false)]
            public string replyMessage { get; set; }

            public int factorId { get; set; }
            public string btn { get; set; }
            public int workFlowJobId { get; set; }
        }

        public class FactorList
        {
            public string customerName { get; set; }

            public int? customerCode { get; set; }

            public int? factorCode { get; set; }
            public string dateTimePersian { get; set; }
            public long priceTotalFactor { get; set; }
            public int factorId { get; set; }
            public WorkFlowJobViewModels.WorkFlowJobCirculation WorkFlowJobs { get; set; }
        }
    }
}