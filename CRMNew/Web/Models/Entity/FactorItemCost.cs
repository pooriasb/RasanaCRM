namespace Web.Models.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.FactorItemCost")]
    public partial class FactorItemCost
    {
        public int id { get; set; }

        public int? factorItem_id { get; set; }

        public int? FactorCostSet_id { get; set; }

        [StringLength(255)]
        public string value { get; set; }

        public virtual FactorCostSet FactorCostSet { get; set; }

        public virtual FactorItem FactorItem { get; set; }
    }
}
