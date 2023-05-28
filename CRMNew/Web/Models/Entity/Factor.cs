namespace Web.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("dbo.Factor")]
    public partial class Factor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Factor()
        {
            FactorCosts = new HashSet<FactorCost>();
            FactorItems = new HashSet<FactorItem>();
            FactorOptionValues = new HashSet<FactorOptionValue>();
        }

        public int id { get; set; }
        [Remote("GetValidCode","Factor")]
        public int? code { get; set; }

        public int? addonCode { get; set; }

        public int? parent_id { get; set; }

        public DateTime? dateTime { get; set; }

        public int? customer_id { get; set; }

        [Required]
        [StringLength(128)]
        public string user_id { get; set; }

        public bool isRasmi { get; set; }

        public DateTime? expireDate { get; set; }

        public int? status { get; set; }

        public byte? type { get; set; }

        public int? presentation { get; set; }

        public int? paymentType { get; set; }

        [StringLength(400)]
        public string paymentDescription { get; set; }

        [StringLength(400)]
        public string placeOfDelivery { get; set; }

        [StringLength(400)]
        public string description { get; set; }

        [StringLength(255)]
        public string person { get; set; }

        [StringLength(128)]
        public string owner_id { get; set; }

        public long priceTotalFactor { get; set; }
        public long priceTotalItem { get; set; }
        public int priceCredit { get; set; }
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactorOptionValue> FactorOptionValues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactorCost> FactorCosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactorItem> FactorItems { get; set; }
        public virtual SiteValue SiteValue { get; set; }

        public virtual SiteValue SiteValue1 { get; set; }

        public virtual SiteValue SiteValue2 { get; set; }
        [NotMapped]
        
        public string expair{ get; set; }
        
    }
}
