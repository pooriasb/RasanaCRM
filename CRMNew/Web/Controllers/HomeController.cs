using Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Web.Insfrastructure.ManagePermission.Filters;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models.Entity;
using Web.Models.Utilities;
using Web.ViewModels.Identity;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork unitOfWork;
        public HomeController()
        {
            unitOfWork = new UnitOfWork();

        }
        PermissionViewModels perViewModel;

        public ActionResult Index()
        {
            //unitOfWork.SiteValueRepository.GetAll();
            //  ApplicationDbInitializer.InitializeIdentityForEF(new ApplicationDbContext());
            // IEnumerable<ApplicationUser> uu=  unitOfWork.Users.GetAll();
            // unitOfWork.Customers.Insert(new Customer());
            // User use = unitOfWork.Users.GetAll().First();
            //  unitOfWork.SiteValueRepository.GetAll();
            //perViewModel = new PermissionViewModels(unitOfWork);


            return View();

        }
        //dfdfdf
        [UserActionFilter]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            //lkklkljljj
            return View();
        }
        [UserActionFilter]
        public ActionResult Contact(int a = 1)
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [UserActionFilter]
        public ActionResult Test()
        {
            //UTLAlert.Danger(this,"<a href='#'>salam</a>","", false);
            //   UTLAlert.Success(this, "من پیامم", "fa fa-battery-full", false);
            return View();
        }
        public ActionResult test2()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetDate()
        {
            try
            {
                var data = Common.ToPersianDateString(DateTime.Now);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new
                {
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }

}