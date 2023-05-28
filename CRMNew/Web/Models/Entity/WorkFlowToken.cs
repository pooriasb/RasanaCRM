using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models.Entity
{
    [Table("dbo.WorkFlowTokens")]
    public partial class WorkFlowToken
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkFlowToken()
        {
            WorkFlowJobs = new HashSet<WorkFlowJob>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(128)]
        public string sTRToken { get; set; }

        [Required]
        [StringLength(128)]
        public string fromUser_id { get; set; }

        [Required]
        [StringLength(128)]
        public string toUser_id { get; set; }

        public DateTime createDate { get; set; }

        public int expireTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkFlowJob> WorkFlowJobs { get; set; }
    }
}