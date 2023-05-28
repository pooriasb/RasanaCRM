using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web.Models.Entity
{
    [Table("dbo.WorkFlows")]
    public partial class WorkFlow
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkFlow()
        {
            WorkFlowJobs = new HashSet<WorkFlowJob>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string title { get; set; }

        public int sectionType_id { get; set; }

        [StringLength(128)]
        public string role_id { get; set; }

        [StringLength(128)]
        public string user_id { get; set; }

        [StringLength(400)]
        public string description { get; set; }
        [DefaultValue(false)]
        public bool isDelete { get; set; }
        public int code { get; set; }
        public virtual SiteValue SiteValue { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkFlowJob> WorkFlowJobs { get; set; }
    }
}