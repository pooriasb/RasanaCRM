using System.ComponentModel;

namespace Web.Models.Entity
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("dbo.ProductPrices")]
    public partial class ProductPrice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductPrice()
        {
            FactorItems = new HashSet<FactorItem>();
        }

        public int id { get; set; }

        public int product_id { get; set; }

        [Required]
        [StringLength(255)]
        public string price { get; set; }
        [DefaultValue(false)]
        public bool isDelete { get; set; }

        public int? vahed_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactorItem> FactorItems { get; set; }

        public virtual Product Product { get; set; }

        public virtual SiteValue SiteValue { get; set; }
    }
}
