using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models.Utilities;
using Web.ViewModels.Enums;

namespace Web.Areas.Employee.Controllers
{
    public class ProductPriceController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductPriceController()
        {
            this.unitOfWork = new UnitOfWork();
        }
        // GET: Employee/ProductPrice
        public ActionResult Index(int productId)
        {
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.productUnit);
            var find = (from pp in unitOfWork.ProductPriceRepository.GetAll(x => x.product_id == productId)
                join pu in unitOfWork.SiteValueRepository.GetAll(x=>x.parentId==parentId) on pp.vahed_id equals pu.id
                select new ProductPriceViewModels.EditProductPrice()
                {
                    price = pp.price,
                    productId = pp.product_id,
                    vahedName = pu.name,
                    productPriceId = pp.id
                }).ToList();
            if (find.Count==0)
            {
                ViewBag.ProductId = productId;
            }
            ViewBag.Prices = find;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductPriceViewModels.Edit model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("Index");
            }
            unitOfWork.ProductPriceRepository.Edit(model);
            unitOfWork.Save();
            return RedirectToAction("Index","ProductPrice",new {area="Employee" , productId=model.id});
        }

        #region Ajax
        [IsAjax]
        public ActionResult AddPartial(int index)
        {
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.productUnit);

            var productUnits = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == parentId && x.isEnable && x.isDelete==false);

            ViewBag.ProductUnits = productUnits;
            ViewBag.Index = index;
            return View();
        }
        [IsAjax]
        public ActionResult EditAddPartial(int index)
        {
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.productUnit);

            var productUnits = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == parentId && x.isEnable && x.isDelete == false);

            ViewBag.ProductUnits = productUnits;
            ViewBag.Index = index;
            return View();
        }
        [IsAjax]
        public ActionResult AddSinglePartial(int productId)
        {
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.productUnit);
            var productUnits = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == parentId && x.isEnable && x.isDelete == false);

            ViewBag.ProductUnits = productUnits;
            ViewBag.ProductId = productId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSingle(ProductPriceViewModels.AddSingle model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("Index", "ProductPrice", new { area = "Employee", productId = model.id });
            }
            try
            {
                unitOfWork.ProductPriceRepository.Insert(model, unitOfWork.ProductPriceLogRepository);
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره اطلاعات خطایی رخ داده است");
                    return RedirectToAction("Index", "ProductPrice", new { area = "Employee", productId = model.id });
                }
            }
            catch (Exception e)
            {
                
            }
            return RedirectToAction("Index", "ProductPrice", new { area = "Employee", productId = model.id });
        }

        [IsAjax]
        public ActionResult EditPartial(int id)
        {
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.productUnit);
            var find = (from pp in unitOfWork.ProductPriceRepository.GetAll(x => x.id == id)
                join pu in unitOfWork.SiteValueRepository.GetAll(x=>x.parentId==parentId) on pp.vahed_id equals pu.id
                select new ProductPriceViewModels.Edit()
                {
                    id = pp.id,
                    price = pp.price,
                    vahedId = pp.vahed_id.Value,
                    vahedValue = pu.name
                }).FirstOrDefault();
            return View(find);
        }

        [IsAjax]
        public JsonResult Delete(int id)
        {
            try
            {
                var find = unitOfWork.ProductPriceRepository.GetByID(id);
                if (find!=null)
                {
                    find.isDelete = true;
                    unitOfWork.Save();
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
        #endregion
    }
}