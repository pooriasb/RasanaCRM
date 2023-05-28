using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Insfrastructure.UnitOfWork;

namespace Web.Controllers
{
    public class ManageUserController : Controller
    {
        UnitOfWork unitOfWork;
        public ManageUserController()
        {
            unitOfWork = new UnitOfWork();
        }
        // GET: ManageUser
        public ActionResult Index()
        {
            return View();
        }


    }
}