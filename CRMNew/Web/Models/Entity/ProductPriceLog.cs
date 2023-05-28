using System;

namespace Web.Models.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.ProductPriceLog")]
    public partial class ProductPriceLog
    {
        public int id { get; set; }

        public int product_id { get; set; }

        public string price { get; set; }

        public int vahed_id { get; set; }

       
        public DateTime? date { get; set; }
       
        public virtual Product Product { get; set; }

        public virtual SiteValue SiteValue { get; set; }
    }
}
