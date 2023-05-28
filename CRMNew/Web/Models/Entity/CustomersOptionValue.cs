namespace Web.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomersOptionValue
    {
        public int id { get; set; }

        public int? customer_id { get; set; }

        public int? customerRelation_id { get; set; }

        public int option_id { get; set; }

        public int? value_id { get; set; }

        [StringLength(255)]
        public string strValue { get; set; }

        public string type { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual CustomerRelation CustomerRelation { get; set; }

        public virtual SiteValue SiteValue { get; set; }

        public virtual SiteValue SiteValue1 { get; set; }
    }
}
