using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models.Entity
{
    public class Policy
    {
        public int id { get; set; }
        public string Catch { get; set; }
        
        [MaxLength(128)]
        public string  UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }

   


    }
}