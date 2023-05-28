namespace Web.Models.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.CRMLog")]
    public partial class CRMLog
    {
        [StringLength(128)]
        public string id { get; set; }

        public int logType { get; set; }

        public int logDocument { get; set; }

        [Required]
        [StringLength(50)]
        public string action { get; set; }

        public string enteredData { get; set; }

        public string resultData { get; set; }

        public DateTime logDate { get; set; }

        [Required]
        [StringLength(15)]
        public string ip { get; set; }

        [StringLength(128)]
        public string user_id { get; set; }

        [StringLength(400)]
        public string description { get; set; }
    }
}
