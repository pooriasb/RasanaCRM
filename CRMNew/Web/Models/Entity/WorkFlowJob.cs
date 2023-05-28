using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models.Entity
{
    [Table("dbo.WorkFlowJobs")]
    public partial class WorkFlowJob
    {
        public int id { get; set; }

        public int workFlow_id { get; set; }

        [Required]
        [StringLength(128)]
        public string object_id { get; set; }

        [Required]
        [StringLength(128)]
        public string fromUser_id { get; set; }

        [Required]
        [StringLength(128)]
        public string toUser_id { get; set; }

        [Required]
        [StringLength(400)]
        public string message { get; set; }

        public DateTime createDate { get; set; }

        [StringLength(400)]
        public string replyMessage { get; set; }

        public DateTime? replyDate { get; set; }

        public byte status { get; set; }

        public int? token_id { get; set; }

        public virtual WorkFlow WorkFlow { get; set; }

        public virtual WorkFlowToken WorkFlowToken { get; set; }
    }
}