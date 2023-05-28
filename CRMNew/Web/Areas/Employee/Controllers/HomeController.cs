using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Insfrastructure.ManagePermission.Filters;
using Web.Insfrastructure.UnitOfWork;

namespace Web.Areas.Employee.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public HomeController()
        {
            this.unitOfWork=new UnitOfWork();
        }
        // GET: Employee/Home
        [UserActionFilter]
        public ActionResult Index()
        {
            //test
            return View();
        }
    }
}