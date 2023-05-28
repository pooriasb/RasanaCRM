using Identity;
using Microsoft.AspNet.Identity.Owin;
using Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;
using Web.ViewModels.Identity;
using Microsoft.AspNet.Identity;
using System;

namespace CRM.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {
        #region Controller Insfrastructure
        IUnitOfWork unitOfWork;
        public UsersAdminController()
        {
            unitOfWork = new UnitOfWork();
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion

        #region Index-Details-Create
        [HttpGet]
        public ActionResult Index()
        {

            return View(UserManager.Users.ToList());


        }

        //
        // GET: /Users/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }
        #endregion

        #region Edit
        //
        // GET: /Users/Edit/1
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            return View(new EditUserViewModel()
            {
                Id = user.Id,
               
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

             

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }
        #endregion
        #region Delete
        //
        // GET: /Users/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion
        #region Permision
        public ActionResult Permission(string id)
        {
            PermissionViewModels permissionViewModels = new PermissionViewModels(unitOfWork, id);

            return View(permissionViewModels);

        }
        [HttpPost]
        public ActionResult Permission(List<Permission> permissions, string id)
        {
            ICollection<Permission> oldPermissions = new List<Permission>();
            IEnumerable<Permission> allpermissions;
            User user = null;
            try
            {
                user = unitOfWork.UserRepository.Get(c => c.Id == id);
                allpermissions = unitOfWork.PermissionRepository.GetAll();
            }
            catch
            {
                return View();
            }

            Policy policy = null;
            if (user.Policies != null && user.Policies.Any())
            {
                try
                {
                    policy = user.Policies.FirstOrDefault();
                }
                catch
                {
                    policy = null;
                }

            }

            if (policy == null)
            {
                policy = new Policy();
                user.Policies.Add(policy);
            }
            if (policy.Permissions != null)
            {
                try
                {
                    oldPermissions = policy.Permissions;
                }
                catch
                {
                    oldPermissions = null;
                }

            }
            else
            {
                policy.Permissions = new List<Permission>();
            }

            if (oldPermissions != null)
            {


                foreach (Permission item in permissions)
                {
                    if (item.isChecked)
                    {
                        if (!oldPermissions.Where(c => c.Id == item.Id).Any())
                            policy.Permissions.Add(allpermissions.Where(c => c.Id == item.Id).FirstOrDefault());
                    }
                    else
                    {
                        if (oldPermissions.Where(c => c.Id == item.Id).Any())
                        {
                            policy.Permissions.Remove(allpermissions.Where(c => c.Id == item.Id).FirstOrDefault());
                        }
                    }
                }



            }
            else
            {
                foreach (Permission item in permissions)
                    if (item.isChecked)
                        policy.Permissions.Add(unitOfWork.PermissionRepository.Get(c => c.Id == item.Id));
            }
            var arryCode = permissions.Where(c => c.isChecked).Select(c => c.Code).ToArray();
            Array.Sort(arryCode);
            string strCodes = string.Join(",",arryCode);
            policy.Catch = strCodes;

            unitOfWork.Save();
            return RedirectToAction("index");

        }
        #endregion
    }
}
