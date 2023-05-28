namespace Web.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dbo.FactorItem")]
    public partial class FactorItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FactorItem()
        {
            FactorItemCosts = new HashSet<FactorItemCost>();
        }

        public int id { get; set; }

        public int factor_id { get; set; }

        public int product_id { get; set; }

        public int price_id { get; set; }

        [Required]
        public long price { get; set; }

        public int count { get; set; }

        [Required]
        public long total { get; set; }

        public byte warranty { get; set; }

        public byte? garanty { get; set; }

        [StringLength(400)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactorItemCost> FactorItemCosts { get; set; }

        public virtual Factor Factor { get; set; }

        public virtual ProductPrice ProductPrice { get; set; }

        public virtual Product Product { get; set; }
    }
}
