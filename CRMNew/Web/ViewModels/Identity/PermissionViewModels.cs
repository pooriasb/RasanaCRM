using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.ViewModels.Identity
{
    public class PermissionViewModels
    {
        IUnitOfWork unitOfWork;
        Policy policy;
        User user;
        List<PermissionsGroup> htmlGroup;
        public List<PermissionsGroup> HtmlGroup { get
            {
                return htmlGroup;
            }
        }
        private List<Permission> permissions;
        private List<Permission> oldPermissions;
        public string Id { get; private set; }
        string sub;
        public PermissionViewModels(IUnitOfWork unitOfWork,string id)
        {
            this.unitOfWork = unitOfWork ?? new UnitOfWork();
            try
            {
                Id = id;
                user = unitOfWork.UserRepository.Get(c => c.Id == id);
                try
                {
                    policy = user.Policies.FirstOrDefault();
                    if (policy != null)
                        oldPermissions = policy.Permissions.ToList();
                }
                catch
                {
                    oldPermissions = new List<Permission>();
                }
              
            }
            catch(Exception ex)
            {
                throw new Exception("کاربر یافت نشد");
            }

            try
            {
                permissions = unitOfWork.PermissionRepository.GetAll().ToList();
            }
            catch
            {

                permissions = new List<Permission>();
            }
            
            IEnumerable<Permission> headPermission = permissions.Where(c => c.Parent == null);
            htmlGroup = new List<PermissionsGroup>();
            foreach (Permission p in headPermission)
            {
              
                    // sub = "";
                    //htmlGroup.Add(new PermissionsGroup() { Html = Menu(permission), Name = permission.Name ,Group=permission.Group});
                    htmlGroup.Add(new PermissionsGroup(oldPermissions){ Html =permissions.Where(c=>c.ParentId== p.Id && c.Parent!=null),Name = p.Name, Group = p.Group });
             
                // PermissionsGroup.Add();


            }



        }

        //private string Menu(Permission parent)
        //{
        //    int? parentId;
        //    if (parent.Id == 0)
        //        parentId = null;
        //    else
        //        parentId = parent.Id;

        //    foreach (Permission item in permissions.Where(c => c.ParentId == parentId))
        //    {

        //        if (permissions.Where(c => c.ParentId == item.Id).Count() > 0)
        //        {
        //            sub += "<li class='dropdown-submenu'><a tabindex='-1'>" + item.Name + "</a>" + "<ul class=''>" + Menu(item);

        //            sub += "</ul></li>";

        //        }
        //        else if (!string.IsNullOrEmpty(item.Name))
        //        {
        //            sub += "<li><a href='" + item.Id + "'>" + item.Name + "</a></li>";
        //            sub += "<input type='checkbox' name='item' value='" + item.Id + "' />";
        //        }

        //    }
        //    return sub;

        //}


    }
    public class PermissionsGroup
    {
       
        IEnumerable<Permission> permissions;
        IEnumerable<Permission> oldPermitions;
        public PermissionsGroup(IEnumerable<Permission>oldPermitions)
        {
            this.oldPermitions = oldPermitions ?? new List<Permission>();
        }
        public string Name { get; set; }
        public string Group { get; set; }
        public IEnumerable<Permission> Html { get
            {
                if (permissions == null)
                    permissions = new List<Permission>();
                foreach (Permission permission in permissions)
                    if (oldPermitions.Any(c => c.Id == permission.Id))
                        permission.isChecked = true;
                return permissions;
            }
            set
            {
                if (value != null)
                    permissions = value;
                else
                    permissions= new List<Permission>();
            }
        }
    }

}

