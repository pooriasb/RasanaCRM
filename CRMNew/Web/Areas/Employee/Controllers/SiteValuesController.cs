using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using jsTree3.Models;
using Newtonsoft.Json;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models.Entity;
using Web.Models.Utilities;
using Web.ViewModels.Enums;

namespace Web.Areas.Employee.Controllers
{
    public class SiteValuesController : Controller
    {
        private readonly UnitOfWork unitOfWork;

        public SiteValuesController()
        {
            this.unitOfWork = new UnitOfWork();
        }

        #region SiteValue-CRUD
        // GET: Employee/SiteValues
        public ActionResult Index(int? parentId = 1)
        {
            //var result = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == (parentId == 0 ? null : parentId));
            var result = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == parentId);
            ViewBag.list = result;
            ViewBag.SiteValueName = unitOfWork.SiteValueRepository.GetByID(parentId).name;
            SiteValuesViewModels.Add siteValue = new SiteValuesViewModels.Add() { parentId = parentId };
            return View(siteValue);
        }
        [HttpPost]
        public ActionResult IndexReturnJson(int? parentId = 1)
        {
            try
            {
                var result = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == parentId);
                ViewBag.list = result;
                ViewBag.SiteValueName = unitOfWork.SiteValueRepository.GetByID(parentId).name;
                SiteValuesViewModels.Add data = new SiteValuesViewModels.Add() { parentId = parentId };
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { statusCode = 200, data }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = ex.Message }, JsonRequestBehavior.DenyGet);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SiteValuesViewModels.Add model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            //first check not duplicate code
            var findCodeId = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == model.parentId && x.code == model.code).Select(x => x.id);
            if (findCodeId.Count() != 0)
            {
                UTLAlert.Danger(this, "مقدار کد وارد شده تکراری می باشد");
                return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            if (model.parentId == 0 || model.parentId == null)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            try
            {
                unitOfWork.SiteValueRepository.Insert(model);
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره داده ها خطایی رخ داده است");
                    return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
                }
            }
            catch (Exception e)
            {
                UTLAlert.Danger(this, "در ذخیره داده ها خطایی رخ داده است");
                return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
                //log
            }

            return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SiteValuesViewModels.Edit model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده برای ویرایش صحیح نمیاشد");
                return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            var findCodeId = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == model.parentId && x.code == model.code).Select(x => x.id);
            if (findCodeId.Count() != 0)
            {
                UTLAlert.Danger(this, "مقدار کد وارد شده تکراری می باشد");
                return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            if (model.parentId == 0 || model.parentId == null)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            try
            {
                unitOfWork.SiteValueRepository.Edit(model);
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره داده ها خطایی رخ داده است");
                    return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
                }
            }
            catch (Exception e)
            {
                //log
            }

            return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
        }

        [IsAjax]
        public ActionResult AddPartial(int? parentId)
        {
            SiteValuesViewModels.Add siteValue = new SiteValuesViewModels.Add() { parentId = parentId };
            return View(siteValue);
        }
        [IsAjax]
        public ActionResult EditPartial(int id)
        {
            var find = unitOfWork.SiteValueRepository.GetByID(id);
            SiteValuesViewModels.Edit siteValue =
                new SiteValuesViewModels.Edit()
                {
                    id = find.id,
                    parentId = find.parentId,
                    name = find.name,
                    code = find.code.Value,
                    isEnable = find.isEnable,
                    value = find.value,
                    description = find.description,
                    isDelete = find.isDelete
                };
            return View(siteValue);
        }
        [IsAjax]
        public JsonResult Delete(int id)
        {
            try
            {
                unitOfWork.SiteValueRepository.DeletePhysical(id);
                unitOfWork.Save();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }

        }

        [IsAjax]
        public JsonResult GetMaxCodeWithParentByAjax(int? parentId)
        {
            int maxCode = unitOfWork.SiteValueRepository.GetMaxCode(x => x.parentId == parentId);
            return Json(maxCode);
        }


        #endregion

        //تخفیف
        #region Discount

        public ActionResult AddDiscount()
        {
            try
            {
                int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.Discount);
                var find = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == parentId).FirstOrDefault();

                var discounts = unitOfWork.SiteValueRepository.GetDiscounts(find.value);

                SiteValuesViewModels.ShowDiscount show = new SiteValuesViewModels.ShowDiscount()
                {
                    name = find.name,
                    description = find.description,
                    isEnable = find.isEnable,
                    json = discounts
                };

                ViewBag.Discounts = show;
            }
            catch (Exception e)
            {
                //UTLAlert.Danger(this,"خطلایی رخ داده است لطفا دوباره تلاش کنید");
                //log
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDiscount(SiteValuesViewModels.AddDiscount model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده برای ویرایش صحیح نمیاشد");
                return RedirectToAction("AddDiscount");
            }
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.Discount);

            // check for not duplicate 
            var find = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == parentId).FirstOrDefault();
            if (find != null)
            {
                var result = unitOfWork.SiteValueRepository.GetDiscounts(find.value);
                if (!unitOfWork.SiteValueRepository.SearchDiscounts(result, model.left, model.right))
                {
                    UTLAlert.Danger(this, $"مقادیر {model.left} و {model.right} تکراری می باشد");
                    return RedirectToAction("AddDiscount");
                }
                result.Add(new SiteValuesViewModels.AddDiscountJson()
                {
                    value = model.value,
                    left = model.left,
                    right = model.right
                });
                find.value = JsonConvert.SerializeObject(result);
            }
            else
            {
                model.parentId = parentId;
                unitOfWork.SiteValueRepository.Insert(model, JsonConvert.SerializeObject(model.json));
            }
            if (!unitOfWork.Save())
            {
                UTLAlert.Danger(this, "در ذخیره اطلاعات خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("AddDiscount");
            }
            return RedirectToAction("AddDiscount", "SiteValues", new { area = "Employee" });
        }

        [IsAjax]
        public ActionResult AddDiscountPartial()
        {
            return View();
        }
        [IsAjax]
        public ActionResult EditDiscountPartial(string innerJson)
        {
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.Discount);
            var find = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == parentId).FirstOrDefault();
            if (find != null)
            {
                var result = unitOfWork.SiteValueRepository.GetDiscounts(find.value);
                var desrialize = JsonConvert.DeserializeObject<SiteValuesViewModels.AddDiscountJson>(innerJson);
                var find2 = result.ToList().Where(x => x.left == desrialize.left && x.right == desrialize.right).FirstOrDefault();

                SiteValuesViewModels.EditDiscount editViewModel = new SiteValuesViewModels.EditDiscount()
                {
                    left = find2.left,
                    right = find2.right,
                    value = find2.value,
                    oldJson = innerJson
                };
                return View(editViewModel);
            }
            else
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمی باشد");
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDiscount(SiteValuesViewModels.EditDiscount model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده برای ویرایش صحیح نمیاشد");
                return RedirectToAction("AddDiscount");
            }
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.Discount);

            // check for not duplicate 
            var find = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == parentId).FirstOrDefault();
            if (find != null)
            {
                var result = unitOfWork.SiteValueRepository.GetDiscounts(find.value);
                var desrialize = JsonConvert.DeserializeObject<SiteValuesViewModels.AddDiscountJson>(model.oldJson);
                if (!unitOfWork.SiteValueRepository.SearchDiscounts(result, model.left, model.right))
                {
                    UTLAlert.Danger(this, $"مقادیر {model.left} و {model.right} تکراری می باشد");
                    return RedirectToAction("AddDiscount");
                }
                foreach (var item in result.ToList())
                {
                    if (item.left == desrialize.left && item.right == desrialize.right)
                        result.Remove(item);
                }
                result.Add(new SiteValuesViewModels.AddDiscountJson()
                {
                    value = model.value,
                    left = model.left,
                    right = model.right
                });

                find.value = JsonConvert.SerializeObject(result);
            }
            if (!unitOfWork.Save())
            {
                UTLAlert.Danger(this, "در ذخیره اطلاعات خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("AddDiscount");
            }
            return RedirectToAction("AddDiscount", "SiteValues", new { area = "Employee" });
        }
        [IsAjax]
        public JsonResult DeleteDiscount(string innerJson)
        {
            try
            {
                int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.Discount);
                var find = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == parentId).FirstOrDefault();
                if (find != null)
                {
                    var result = unitOfWork.SiteValueRepository.GetDiscounts(find.value);
                    var desrialize = JsonConvert.DeserializeObject<SiteValuesViewModels.AddDiscountJson>(innerJson);
                    foreach (var item in result.ToList())
                    {
                        if (item.left == desrialize.left && item.right == desrialize.right)
                            result.Remove(item);
                    }
                    find.value = JsonConvert.SerializeObject(result);
                    if (!unitOfWork.Save())
                    {
                        return Json(false);
                    }
                }
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
                //log
            }
        }

        #endregion

        #region SectionType-CRU

        public ActionResult SectionType()
        {
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.SectionType);
            var result = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == parentId);
            ViewBag.List = result;
            SiteValuesViewModels.AddSectionType siteValue = new SiteValuesViewModels.AddSectionType() { parentId = parentId };
            return View(siteValue);
        }
        [IsAjax]
        public ActionResult AddSectionTypePartial(int? parentId)
        {
            SiteValuesViewModels.AddSectionType siteValue = new SiteValuesViewModels.AddSectionType() { parentId = parentId };
            return View(siteValue);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSectionType(SiteValuesViewModels.AddSectionType model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("SectionType", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            //first check not duplicate code
            var findCode = unitOfWork.SiteValueRepository.Get(x => x.parentId == model.parentId && x.code == model.code);
            if (findCode != null)
            {
                UTLAlert.Danger(this, "مقدار کد وارد شده تکراری می باشد");
                return RedirectToAction("SectionType", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            if (model.parentId == 0 || model.parentId == null)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("SectionType", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            try
            {
                //unitOfWork.SiteValueRepository.Insert(model);
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره داده ها خطایی رخ داده است");
                    return RedirectToAction("SectionType", "SiteValues", new { area = "Employee", parentId = model.parentId });
                }
            }
            catch (Exception e)
            {
                UTLAlert.Danger(this, "در ذخیره داده ها خطایی رخ داده است");
                return RedirectToAction("SectionType", "SiteValues", new { area = "Employee", parentId = model.parentId });
                //log
            }

            return RedirectToAction("SectionType", "SiteValues", new { area = "Employee", parentId = model.parentId });
        }
        [IsAjax]
        public ActionResult EditSectionTypePartial(int id)
        {
            var find = unitOfWork.SiteValueRepository.GetByID(id);
            SiteValuesViewModels.EditSectionType siteValue =
                new SiteValuesViewModels.EditSectionType()
                {
                    id = find.id,
                    parentId = find.parentId,
                    name = find.name,
                    code = find.code.Value,
                    isEnable = find.isEnable,
                    description = find.description,
                    isDelete = find.isDelete
                };
            if (find.value.Substring(9, 4) == "True")
            {
                siteValue.workFlow = true;
            }
            else
            {
                siteValue.workFlow = false;
            }
            return View(siteValue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSectionType(SiteValuesViewModels.EditSectionType model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده برای ویرایش صحیح نمیاشد");
                return RedirectToAction("SectionType", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            var findCode = unitOfWork.SiteValueRepository.Get(x => x.parentId == model.parentId && x.code == model.code);
            if (findCode != null)
            {
                UTLAlert.Danger(this, "مقدار کد وارد شده تکراری می باشد");
                return RedirectToAction("SectionType", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            if (model.parentId == 0 || model.parentId == null)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("SectionType", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            try
            {
                // unitOfWork.SiteValueRepository.Edit(model);
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره داده ها خطایی رخ داده است");
                    return RedirectToAction("SectionType", "SiteValues", new { area = "Employee", parentId = model.parentId });
                }
            }
            catch (Exception e)
            {
                //log
            }

            return RedirectToAction("SectionType", "SiteValues", new { area = "Employee", parentId = model.parentId });
        }
        #endregion




        #region Locations
        [AllowAnonymous]
        public ActionResult LoadExcel()
        {
            try
            {
                UTLExcel utlExcel = new UTLExcel();
                utlExcel.ImportSiteValuesData("SiteValues.xlsx", "~/Uploads/Excels/Migration/", unitOfWork.SiteValueRepository);
                unitOfWork.Save();
                UTLAlert.Success(this, "با موفقیت ثبت شد");
                return RedirectToAction("Locations", "SiteValues", new { area = "Employee" });
            }
            catch (Exception e)
            {
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("Locations", "SiteValues", new { area = "Employee" });
            }
        }
        public ActionResult Locations()
        {
            int provinceParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.province);

            ViewBag.Locations = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == provinceParentId && x.isDelete == false);
            return View();
        }

        [IsAjax]
        public JsonResult CheckProvinceName(string value)
        {
            try
            {
                int provinceParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.province);
                var find =
                    unitOfWork.SiteValueRepository.GetAll(x => x.parentId == provinceParentId && x.name == value);
                if (find.Count() == 0)
                {
                    return Json("ok");
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
        [IsAjax]
        public JsonResult AddProvinceLocation(string value)
        {
            try
            {
                int provinceParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.province);
                var find = unitOfWork.SiteValueRepository.Get(x => x.name == value && x.parentId == provinceParentId);
                if (find == null)
                {
                    unitOfWork.SiteValueRepository.Insert(provinceParentId, value);
                    if (!unitOfWork.Save())
                    {
                        return Json(false);
                    }
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }

        [IsAjax]
        public ActionResult GetCitiesPartial(int id)
        {
            var find = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == id && x.isDelete == false);
            ViewBag.Cities = find;
            ViewBag.ProvinceId = id;
            return View();
        }

        [IsAjax]
        public JsonResult AddCityLocation(int id, string value)
        {
            try
            {
                var find = unitOfWork.SiteValueRepository.Get(x => x.parentId == id && x.name == value);
                if (find == null)
                {
                    unitOfWork.SiteValueRepository.Insert(id, value);
                    if (!unitOfWork.Save())
                    {
                        return Json(false);
                    }
                    return Json(true);
                }

                return Json(false);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
        [IsAjax]
        public JsonResult EditlocationAjax(int id, string value)
        {
            try
            {
                var find = unitOfWork.SiteValueRepository.GetByID(id);
                if (find != null)
                {
                    find.name = value;
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





        #region Organization

        public ActionResult Organization()
        {
            int organizationParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.OrganizationUnit);
            var result = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete==false && x.value!=null);
            result = result
                .Where(x => UTLSiteValues.GetValueField(Enums.SiteValueFieldJson.FieldDynamic.ToString(), x.value) ==
                            "False").ToList();
            ViewBag.list = result;
            ViewBag.SiteValueName = unitOfWork.SiteValueRepository.GetByID(organizationParentId).name;
            SiteValuesViewModels.Add siteValue = new SiteValuesViewModels.Add() { parentId = organizationParentId };

            ViewBag.JsTree = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == organizationParentId && x.isDelete==false);








            return View(siteValue);
        }
        public ActionResult OrganizationPartial(int? id)
        {
            SiteValuesViewModels.Add siteValue = new SiteValuesViewModels.Add() { parentId = id };
            return View(siteValue);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrganizationAdd(SiteValuesViewModels.Add model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            //first check not duplicate code
            var findCodeId = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == model.parentId && x.code == model.code).Select(x => x.id);
            if (findCodeId.Count() != 0)
            {
                UTLAlert.Danger(this, "مقدار کد وارد شده تکراری می باشد");
                return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            if (model.parentId == 0 || model.parentId == null)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
            }
            try
            {
                model.value = UTLSiteValues.SetValueField(Enums.SiteValueFieldJson.FieldDynamic.ToString(), "False");
                unitOfWork.SiteValueRepository.Insert(model);
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره داده ها خطایی رخ داده است");
                    return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
                }
            }
            catch (Exception e)
            {
                UTLAlert.Danger(this, "در ذخیره داده ها خطایی رخ داده است");
                return RedirectToAction("Index", "SiteValues", new { area = "Employee", parentId = model.parentId });
                //log
            }

            return RedirectToAction("Organization", "SiteValues", new { area = "Employee", parentId = model.parentId });
        }

        [IsAjax]
        public JsonResult GetNodes()
        {
            int organizationParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.OrganizationUnit);
            var nodes = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == organizationParentId).Select(
                x => new List<JsTree3Node>()
                {
                    new JsTree3Node()
                    {
                    id = x.id.ToString(),
                    state = new State(true, false, false),
                    text = x.name,
                    children = x.SiteValues1.Select(y => new JsTree3Node()
                    {
                        id = y.id.ToString(),
                        state = new State(false, false, false),
                        text = y.name,
                    }).ToList(),
                    icon = "fa fa-times"
                    }
                }).ToList();
            //var nodes = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == organizationParentId);

            //var root = new JsTree3Node() // Create our root node and ensure it is opened
            //{
            //    id = "salam",
            //    text = "Root Node",
            //    state = new State(true, false, false)
            //};

            //// Create a basic structure of nodes
            //var children = new List<JsTree3Node>();
            //foreach (var item in nodes)
            //{
            //    var node = JsTree3Node.NewNode(item.id.ToString());
            //    node.state = new State(false, false, false);

            //    foreach (var siteValue in item.SiteValues1)
            //    {
            //        node.children.Add(JsTree3Node.NewNode(siteValue.id.ToString()));
            //    }
            //    children.Add(node);
            //}

            //// Add the sturcture to the root nodes children property
            //root.children = children;

            return Json(nodes, JsonRequestBehavior.AllowGet);
        }



        #endregion



    }
}