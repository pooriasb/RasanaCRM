namespace Web.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomersOrganizationsBridge")]
    public partial class CustomersOrganizationsBridge
    {
        public int id { get; set; }

        public int customer_id { get; set; }

        public int organization_id { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual SiteValue SiteValue { get; set; }
    }
}
