namespace Web.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.FactorCost")]
    public partial class FactorCost
    {
        public int id { get; set; }

        public int factor_id { get; set; }

        public int factorCostSet_id { get; set; }

        [Required]
        [StringLength(255)]
        public string value { get; set; }

        public virtual FactorCostSet FactorCostSet { get; set; }

        public virtual Factor Factor { get; set; }
    }
}
