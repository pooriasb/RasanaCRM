using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Insfrastructure.UnitOfWork;
using Web.ViewModels.Identity;

namespace Web.Controllers
{
    public class ManagePermissionController : Controller
    {
        IUnitOfWork unitOfWork;
        public ManagePermissionController()
        {
            unitOfWork = new UnitOfWork();
        }
        // GET: ManagePermission
        public ActionResult Index()
        {
          //  PermissionViewModels permissionViewModels = new PermissionViewModels(unitOfWork);

            return View();
        }


    }
}