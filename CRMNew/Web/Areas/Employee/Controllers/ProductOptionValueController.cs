using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models.Utilities;
using Web.ViewModels.Enums;

namespace Web.Areas.Employee.Controllers
{
    public class ProductOptionValueController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductOptionValueController()
        {
            this.unitOfWork = new UnitOfWork();
        }
        // GET: Employee/ProductOptionValue
        public ActionResult Index()
        {
            //id is siteValue id where parentId==Enums.SiteValue.productCategory
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.productCategory);
            ViewBag.SiteValues =
                unitOfWork.SiteValueRepository.GetAll(x => x.parentId == parentId && x.isDelete == false);
            return View();
        }

        public ActionResult Add(int id)
        {
            ViewBag.SubCategoryId = id;

            var result = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == id);
            ViewBag.OptionValue = result;
            ViewBag.CategoryName = unitOfWork.SiteValueRepository.GetByID(id).name;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SiteValuesViewModels.AddHeaderOption model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمی باشد");
                return RedirectToAction("Add", "ProductOptionValue", new { area = "Employee", id = model.subCategoryId });
            }
            //first check for not duplicate name 
            var find = unitOfWork.SiteValueRepository.GetAll(x => x.name == model.name && x.value == model.TypeOptopn);
            if (find.Count() != 0)
            {
                UTLAlert.Danger(this, "داده وارد شده تکراری می باشد");
                return RedirectToAction("Add", "ProductOptionValue", new { area = "Employee", id = model.subCategoryId });
            }
            int result = unitOfWork.SiteValueRepository.Insert(model);
            if (result == 0)
            {
                UTLAlert.Danger(this, "تعداد ایتم ها از حد مجاز گذشته است با راهبر سیستم تماس بگیرید");
                return RedirectToAction("Add", "ProductOptionValue", new { area = "Employee", id = model.subCategoryId });
            }
            try
            {
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره داده ها خطایی رخ داده است");
                    return RedirectToAction("Add", "ProductOptionValue", new { area = "Employee", id = model.subCategoryId });
                }
            }
            catch (Exception e)
            {
                //log
            }


            return RedirectToAction("Add", "ProductOptionValue", new { area = "Employee", id = model.subCategoryId });
        }

        [IsAjax]
        public ActionResult OptionValueDeatils(int optionNameId)
        {
            var result = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == optionNameId);
            ViewBag.OptionValues = result;
            ViewBag.OptionName = unitOfWork.SiteValueRepository.Get(x => x.id == optionNameId).name;
            return View();
        }
        #region Ajax
        //[IsAjax]
        //public JsonResult GetOptionNameDetailsAjax(int id)
        //{
        //    var result = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == id)
        //        .Select(x => new {id = x.id, name = x.name});
        //    return Json(result);
        //}


        [IsAjax]
        public JsonResult AddOptionValueAjax(int id, string value)
        {
            //id is optionNameId in siteValue table 
            try
            {
                var find = unitOfWork.SiteValueRepository.Get(x => x.parentId == id && x.name == value);
                if (find != null)
                {
                    return Json("duplicate");
                }
                unitOfWork.SiteValueRepository.Insert(id, value);
                unitOfWork.Save();
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
        [IsAjax]
        public JsonResult DeleteOptionNameAjax(int id)
        {
            try
            {
                unitOfWork.SiteValueRepository.DeletePhysical(id);
                unitOfWork.Save();
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }

        [IsAjax]
        public JsonResult EditOptionNameAjax(int id, string value)
        {
            //id is optionNameId in siteValue table 
            try
            {
                var find = unitOfWork.SiteValueRepository.GetByID(id);
                if (find.name == value)
                {
                    return Json("duplicate");
                }
                find.name = value;
                unitOfWork.Save();
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }

        [IsAjax]
        public JsonResult EditOptionValueAjax(int id, string value)
        {
            try
            {
                var povFind = unitOfWork.ProductOptionValueRepository.GetByID(id);
                if (povFind == null)
                {
                    return Json("not found");
                }
                povFind.strValue = value;
                if (!unitOfWork.Save())
                {
                    return Json(false);
                }
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
        [IsAjax]
        public JsonResult GetDropDownByIdAjax(int optionId)
        {
            var find = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == optionId)
                .Select(x => new { id = x.id, value = x.name });
            return Json(find, JsonRequestBehavior.AllowGet);
        }

        [IsAjax]
        public JsonResult EditOptionValueByIdAjax(int povId, int optionValueId)
        {
            try
            {
                var find = unitOfWork.ProductOptionValueRepository.GetByID(povId);
                find.value_id = optionValueId;
                unitOfWork.Save();
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
            }

        }

        [IsAjax]
        public JsonResult ChangeRequired(int id,bool checkedBox)
        {
            try
            {
                var find = unitOfWork.SiteValueRepository.GetByID(id);
                if (find != null)
                {
                    if (checkedBox)
                    {
                        var json= UTLSiteValues.EditValueField(Enums.SiteValueFieldJson.isRequired.ToString(), "True", find.value);
                        if (json!= "Error")
                            find.value = UTLSiteValues.EditValueField(Enums.SiteValueFieldJson.isRequired.ToString(), "True", find.value);
                        else
                            return Json(false);
                    }
                    else
                    {
                        var json = UTLSiteValues.EditValueField(Enums.SiteValueFieldJson.isRequired.ToString(), "False", find.value);
                        if (json != "Error")
                            find.value = UTLSiteValues.EditValueField(Enums.SiteValueFieldJson.isRequired.ToString(), "False", find.value);
                        else
                            return Json(false);
                    }
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