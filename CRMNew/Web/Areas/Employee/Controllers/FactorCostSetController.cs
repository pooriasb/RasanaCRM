using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models.Utilities;

namespace Web.Areas.Employee.Controllers
{
    public class FactorCostSetController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public FactorCostSetController()
        {
            this.unitOfWork=new UnitOfWork();
        }
        // GET: Employee/FactorCostSet
        public ActionResult Index()
        {
            ViewBag.FacotrCostSet = unitOfWork.FactorCostSetRepository.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FactorCostSetViewModels.Add model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("Index");
            }
            //first check not duplicate code
            var findCodeId = unitOfWork.FactorCostSetRepository.GetAll(x => x.name == model.name).Select(x => x.id);
            int a = findCodeId.Count();
            if (a != 0)
            {
                UTLAlert.Danger(this, "ممکن است هزینه به صورت منطقی حذف شده باشد.نام هزینه تکراری می باشد");
                return RedirectToAction("Index");
            }

            unitOfWork.FactorCostSetRepository.Insert(model);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FactorCostSetViewModels.Edit model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("Index");
            }
            //first check not duplicate code
            unitOfWork.FactorCostSetRepository.Edit(model);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }




        #region Ajax
        [IsAjax]
        public ActionResult AddPartial()
        {
            return View();
        }
        [IsAjax]
        public JsonResult Delete(int id)
        {
            var find = unitOfWork.FactorCostSetRepository.GetByID(id);
            find.isEnable = false;
            unitOfWork.Save();
            return Json(true);
        }
        [IsAjax]
        public ActionResult EditPartial(int id)
        {
            var find = unitOfWork.FactorCostSetRepository.GetByID(id);
            FactorCostSetViewModels.Edit editViewModel=new FactorCostSetViewModels.Edit()
            {
                id = find.id,
                description = find.description,
                name = find.name,
                isEnable = find.isEnable,
                isInItem = find.isInItem,
                isInFee = find.isInFee,
                isPresent = find.isPresent,
                isIncrese = find.isInCrease,
                isShowCustomer = find.isShowCustomer
            };  
            return View(editViewModel);
        }

        #endregion
    }
}