using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Web.Models.Entity;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class WorkFlowJobViewModels
    {
        public class WorkFlowJobList
        {
            public string dateTime { get; set; }
            public List<WorkFlowJobViewModels.WorkFlowJobCirculation> WorkFlowJobCirculation { get; set; }
        }
        public class WorkFlowJobCirculation
        {
            public int id { get; set; }

            public int workFlow_id { get; set; }

            public string object_id { get; set; }

            public string fromUserName { get; set; }
            public string toUserId { get; set; }

            public string toUserName { get; set; }

            public string message { get; set; }

            public DateTime createDate { get; set; }
            public string createDatePersian { get; set; }
            public string createTimePersian { get; set; }
            public string replyMessage { get; set; }

            public DateTime? replyDate { get; set; }
            public string replyDatePersian { get; set; }

            public byte status { get; set; }

            public int? token_id { get; set; }

            public string ownerId { get; set; }
            public virtual WorkFlow WorkFlow { get; set; }

            public static implicit operator WorkFlowJobCirculation(WorkFlowJob v)
            {
                throw new NotImplementedException();
            }
        }

        //public class WorkFlowjobsFactorList
        //{
        //    public int id { get; set; }

        //    public int workFlow_id { get; set; }

        //    [Required]
        //    [StringLength(128)]
        //    public string object_id { get; set; }

        //    [Required]
        //    [StringLength(128)]
        //    public string fromUser_id { get; set; }

        //    [Required]
        //    [StringLength(128)]
        //    public string toUser_id { get; set; }

        //    [Required]
        //    [StringLength(400)]
        //    public string message { get; set; }

        //    public DateTime createDate { get; set; }

        //    [StringLength(400)]
        //    public string replyMessage { get; set; }

        //    public DateTime? replyDate { get; set; }

        //    public byte status { get; set; }

        //    public int? token_id { get; set; }

        //    public virtual WorkFlow WorkFlow { get; set; }

        //    public virtual WorkFlowToken WorkFlowToken { get; set; }
        //}
    }
}