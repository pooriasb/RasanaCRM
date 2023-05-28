using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Insfrastructure.ManagePermission;

namespace Web.Models.Entity
{
    public class Permission
    {
        public int Id { get; set; }
        [Code(20000)]
        public string Name { get; set; }
        [Code(12,21,2,1)]
        public short Code { get; set; }
        public int? ParentId { get; set; }
        public string Group { get; set; }
        public virtual Permission Parent { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
       
        public virtual ICollection<Policy> Policies { get; set; }

       
        [NotMapped]
        public bool isChecked { get; set; }

    }
}