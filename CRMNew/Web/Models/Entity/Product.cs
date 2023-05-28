namespace Web.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("dbo.Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            FactorItems = new HashSet<FactorItem>();
            ProductPriceLogs = new HashSet<ProductPriceLog>();
            ProductPrices = new HashSet<ProductPrice>();
            ProductOptionValues = new HashSet<ProductOptionValue>();
        }

        public int id { get; set; }
        public int localId { get; set; }

        [Required]
        public float code { get; set; }

        public DateTime createDate { get; set; }

        [Required]
        [StringLength(255)]
        public string title { get; set; }

        public DateTime? lastUpdateDate { get; set; }
        [DefaultValue(false)]
        public bool isDalete { get; set; }
        [DefaultValue(false)]
        public bool isEnable { get; set; }

        public int category_id { get; set; }

        [Required]
        [StringLength(128)]
        public string user_id { get; set; }

        [StringLength(128)]
        public string lastUpdateUser_id { get; set; }

        [StringLength(400)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactorItem> FactorItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPriceLog> ProductPriceLogs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOptionValue> ProductOptionValues { get; set; }
        public virtual SiteValue SiteValue { get; set; }
    }
}
