namespace Web.Models.Entity
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("dbo.SiteValues")]
    public partial class SiteValue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SiteValue()
        {
            ProductOptionValues = new HashSet<ProductOptionValue>();
            ProductOptionValues1 = new HashSet<ProductOptionValue>();
            ProductPriceLogs = new HashSet<ProductPriceLog>();
            ProductPrices = new HashSet<ProductPrice>();
            Products = new HashSet<Product>();
            SiteValues1 = new HashSet<SiteValue>();
            WorkFlows = new HashSet<WorkFlow>();
            Users = new HashSet<User>();
            Customers = new HashSet<Customer>();
            CustomersCategoriesBridges = new HashSet<CustomersCategoriesBridge>();
            CustomersOptionValues = new HashSet<CustomersOptionValue>();
            CustomersOptionValues1 = new HashSet<CustomersOptionValue>();
            CustomersOrganizationsBridges = new HashSet<CustomersOrganizationsBridge>();
            Factors = new HashSet<Factor>();
            Factors1 = new HashSet<Factor>();
            Factors2 = new HashSet<Factor>();
            FactorOptionValues = new HashSet<FactorOptionValue>();
            FactorOptionValues1 = new HashSet<FactorOptionValue>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? code { get; set; }
        public int? parentId { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        public string value { get; set; }
        [DefaultValue(false)]
        public bool isDelete { get; set; }
        [DefaultValue(false)]
        public bool isEnable { get; set; }

        [StringLength(400)]
        public string description { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOptionValue> ProductOptionValues { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOptionValue> ProductOptionValues1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPriceLog> ProductPriceLogs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<SiteValue> SiteValues1 { get; set; }
        ////coment
        public virtual SiteValue SiteValue1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkFlow> WorkFlows { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomersCategoriesBridge> CustomersCategoriesBridges { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomersOptionValue> CustomersOptionValues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomersOptionValue> CustomersOptionValues1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomersOrganizationsBridge> CustomersOrganizationsBridges { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Factor> Factors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Factor> Factors1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Factor> Factors2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactorOptionValue> FactorOptionValues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactorOptionValue> FactorOptionValues1 { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<SiteValue> SiteValues1 { get; set; }
    }
}
