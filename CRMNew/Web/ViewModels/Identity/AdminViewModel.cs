using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Identity
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

       //[Required(ErrorMessage ="لطفا نام کاربری را وارد کنید")]
       //[MaxLength(255,ErrorMessage ="مقدار مجاز 255 کاراکتر میباشد")]
       // public string Username { get; set; }
    
       // [Required(ErrorMessage = "لطفا  گذرواژه را وارد کنید")]
       // public string PassWord { get; set; }
       // [Required(ErrorMessage = "لطفا تکرار گذرواژه را وارد کنید")]

       // [System.ComponentModel.DataAnnotations.Compare("PassWord", ErrorMessage ="گذرواژه ها یکسان نیستند")]
       // public string PassWordCopy { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}