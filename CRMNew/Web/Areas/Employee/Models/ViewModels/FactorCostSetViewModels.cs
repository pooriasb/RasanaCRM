using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class FactorCostSetViewModels
    {
        public class Add
        {
            [Required(ErrorMessage = "لطفا فیلد نام را پر کنید",AllowEmptyStrings = false)]
            public string name { get; set; }
            [Required(ErrorMessage = "لطفا گزینه مورد نظر را انتخاب کنید",AllowEmptyStrings = false)]
            public bool isIncrese { get; set; }
            [Required(ErrorMessage = "لطفا گزینه مورد نظر را انتخاب کنید", AllowEmptyStrings = false)]
            public bool isPresent { get; set; }
            public bool isShowCustomer { get; set; }
            [Required(ErrorMessage = "لطفا گزینه مورد نظر را انتخاب کنید", AllowEmptyStrings = false)]
            public bool isEnable { get; set; }
            [Required(ErrorMessage = "لطفا گزینه مورد نظر را انتخاب کنید", AllowEmptyStrings = false)]
            public bool isInItem { get; set; }
            [Required(ErrorMessage = "لطفا گزینه مورد نظر را انتخاب کنید", AllowEmptyStrings = false)]
            public bool isInFee { get; set; }
            public string description { get; set; }
        }
        public class Edit
        {
            public int id { get; set; }
            [Required(ErrorMessage = "لطفا فیلد نام را پر کنید", AllowEmptyStrings = false)]
            public string name { get; set; }
            [Required(ErrorMessage = "لطفا گزینه مورد نظر را انتخاب کنید", AllowEmptyStrings = false)]
            public bool isIncrese { get; set; }
            [Required(ErrorMessage = "لطفا گزینه مورد نظر را انتخاب کنید", AllowEmptyStrings = false)]
            public bool isPresent { get; set; }
            public bool isShowCustomer { get; set; }
            [Required(ErrorMessage = "لطفا گزینه مورد نظر را انتخاب کنید", AllowEmptyStrings = false)]
            public bool isEnable { get; set; }
            [Required(ErrorMessage = "لطفا گزینه مورد نظر را انتخاب کنید", AllowEmptyStrings = false)]
            public bool isInItem { get; set; }
            [Required(ErrorMessage = "لطفا گزینه مورد نظر را انتخاب کنید", AllowEmptyStrings = false)]
            public bool isInFee { get; set; }
            public string description { get; set; }
        }
    }
}