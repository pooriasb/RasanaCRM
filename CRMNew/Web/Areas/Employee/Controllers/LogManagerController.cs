using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Insfrastructure.UnitOfWork;

namespace Web.Areas.Employee.Controllers
{
    public class LogManagerController : Controller
    {
        IUnitOfWork unitOfWork;
        public LogManagerController()
        {
            unitOfWork = new UnitOfWork();
        }
        // GET: Employee/LogManager
        public ActionResult Index()
        {
            ViewBag.Logs = unitOfWork.CrmLogsRepository.GetAll().Take(20).ToList() ;
            return View();
        }
    }
}