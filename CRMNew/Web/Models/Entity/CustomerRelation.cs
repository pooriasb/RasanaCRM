namespace Web.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dbo.CustomerRelation")]
    public partial class CustomerRelation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerRelation()
        {
            CustomersOptionValues = new HashSet<CustomersOptionValue>();
        }
        public int id { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "فیلد {0} الزامی است")]
        [Display(Name ="نام")]
        public string name { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "فیلد {0} الزامی است")]
        [Display(Name ="نام خانوادگی")]
        public string family { get; set; }

        [StringLength(10)]
        [Display(Name ="کد ملی")]
        public string nationalCode { get; set; }

        [StringLength(11)]
        [Display(Name ="تلفن")]
        public string phone { get; set; }

        [StringLength(11)]
        [Display(Name = "تلفن")]
        public string tell { get; set; }

        [StringLength(11)]
        [Display(Name = "شماره فکس")]
        public string fax { get; set; }

        [StringLength(255)]
        [Display(Name = "کد پستی")]
        public string postCode { get; set; }

        public int? customer_id { get; set; }
        [StringLength(255)]
        [Display(Name ="شغل")]
        public string Job { get; set; }
        [StringLength(400)]
        public string description { get; set; }
        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomersOptionValue> CustomersOptionValues { get; set; }
    }
}
