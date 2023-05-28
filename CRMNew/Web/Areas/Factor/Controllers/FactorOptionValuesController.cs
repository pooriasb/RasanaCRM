using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.ViewModels.Enums;

namespace Web.Areas.Factor.Controllers
{
    public class FactorOptionValuesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public FactorOptionValuesController()
        {
            this.unitOfWork = new UnitOfWork();
        }
        // GET: Factor/FactorOptionValues
        public ActionResult Index()
        {
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.FactorOptionValues);
            ViewBag.SiteValues =
                unitOfWork.SiteValueRepository.GetAll(x => x.id == parentId && x.isDelete == false);
            return View();
        }

    }
}