using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models.Entity
{
    [Table("dbo.ProductOptionValue")]
    public partial class ProductOptionValue
    {
        public int id { get; set; }

        public int product_id { get; set; }

        public int option_id { get; set; }

        public int? value_id { get; set; }

        [StringLength(255)]
        public string strValue { get; set; }

        public virtual Product Product { get; set; }

        public virtual SiteValue SiteValue { get; set; }

        public virtual SiteValue SiteValue1 { get; set; }
    }
}