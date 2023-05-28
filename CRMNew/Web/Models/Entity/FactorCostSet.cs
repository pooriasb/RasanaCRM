namespace Web.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dbo.FactorCostSet")]
    public partial class FactorCostSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FactorCostSet()
        {
            FactorCosts = new HashSet<FactorCost>();
            FactorItemCosts = new HashSet<FactorItemCost>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public bool isPresent { get; set; }

        public bool isShowCustomer { get; set; }

        public bool isEnable { get; set; }

        public bool isInItem { get; set; }

        public bool isInFee { get; set; }

        public bool isInCrease { get; set; }

        [StringLength(400)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactorCost> FactorCosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactorItemCost> FactorItemCosts { get; set; }
    }
}
