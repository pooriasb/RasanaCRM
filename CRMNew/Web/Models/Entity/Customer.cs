using System.ComponentModel;

namespace Web.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.Customer")]
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            CustomersCategoriesBridges = new HashSet<CustomersCategoriesBridge>();
            CustomersOptionValues = new HashSet<CustomersOptionValue>();
            CustomersOrganizationsBridges = new HashSet<CustomersOrganizationsBridge>();
            CustomerRelations = new HashSet<CustomerRelation>();
            Factors = new HashSet<Factor>();
        }

        public int id { get; set; }
        [DefaultValue(false)]
        public bool isDelete { get; set; }
        public int? localId { get; set; }
        [Required(ErrorMessage ="فیلد {0} الزامی است")]
        [StringLength(255)]
        [Display(Name ="نام")]
        public string name { get; set; }
        [StringLength(10)]
        [Display(Name ="کد ملی")]
        public string nationalCode { get; set; }
        [StringLength(255)]
        [Display(Name ="شماره حساب")]
        public string accountNumber { get; set; }
        [StringLength(11)]
        [Display(Name ="موبایل")]
        public string phone { get; set; }

        [StringLength(11)]
        [Display(Name = "تلفن 1")]
        public string tell { get; set; }
        [StringLength(11)]
        [Display(Name = "تلفن 2")]
        public string tell1 { get; set; }
        [StringLength(11)]
        [Display(Name = "تلفن 3")]
        public string tell2 { get; set; }

        [StringLength(11)]
        [Display(Name ="فکس")]
        public string fax { get; set; }

        [StringLength(10)]
        [Display(Name ="کد پستی")]
        public string postCode { get; set; }

        [StringLength(255)]
        [Display(Name ="شماره اقتصادی")]
        public string economicCode { get; set; }
        [Display(Name ="استان")]
        public int? province_id { get; set; }
        [Display(Name ="شهر")]
        public int? city_id { get; set; }
        [StringLength(400)]
        [Display(Name ="آدرس")]
        public string address { get; set; }
        [StringLength(400)]
        [Display(Name = "آدرس1")]
        public string address1 { get; set; }
        [StringLength(400)]
        [Display(Name = "آدرس2")]
        public string address2 { get; set; }
        [Display(Name ="کد")]
        //[Remote(action:"CheckCode",controller:"Customers",areaName:"Employee")]
        public int? code { get; set; }

        public DateTime dateTime { get; set; }
        [StringLength(400)]
        public string description { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerRelation> CustomerRelations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomersCategoriesBridge> CustomersCategoriesBridges { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomersOptionValue> CustomersOptionValues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomersOrganizationsBridge> CustomersOrganizationsBridges { get; set; }
        public virtual ICollection<Factor> Factors { get; set; }
        public virtual SiteValue SiteValue { get; set; }
    }
}
