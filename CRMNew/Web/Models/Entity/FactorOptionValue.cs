using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models.Entity
{
    [Table("dbo.FactorOptionValues")]
    public partial class FactorOptionValue
    {
        public int id { get; set; }

        public int? factor_id { get; set; }

        public int? option_id { get; set; }

        public int? value_id { get; set; }

        [StringLength(255)]
        public string strValue { get; set; }

        [Required]
        [StringLength(255)]
        public string type { get; set; }

        public virtual Factor Factor { get; set; }

        public virtual SiteValue SiteValue { get; set; }

        public virtual SiteValue SiteValue1 { get; set; }
    }
}