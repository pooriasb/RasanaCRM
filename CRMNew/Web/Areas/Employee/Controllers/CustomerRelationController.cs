using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models.Entity;
using Web.Models.Utilities;
using Web.ViewModels.Enums;

namespace Web.Areas.Employee.Controllers
{
    public class CustomerRelationController : Controller
    {
        UnitOfWork unitOfWork;
        public CustomerRelationController()
        {
            unitOfWork = new UnitOfWork();
        }



        [IsAjax]
        public ActionResult AddPartial(int id)
        {
            //id is customer id
            var category = unitOfWork.CustomersCategoriesBridgeRepository
                .GetAll2(x => x.customer_id == id, null, x => x.SiteValue);

            var organization = unitOfWork.CustomersOrganizationBridgeRepository
                .GetAll2(x => x.customer_id == id, null, x => x.SiteValue);

            ViewBag.CustomerId = id;
            ViewBag.CategoriesName = category.Select(x => x.SiteValue.name);
            ViewBag.OrganizationUnitsName = organization.Select(x => x.SiteValue.name);
            ViewBag.OrganizationUnits = organization.Select(x => x.organization_id).ToList();
            ViewBag.Categories = category.Select(x => x.category_id).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CustomerRelationViewModels.Add model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمی باشد");
                return RedirectToAction("Index", "Customers", new { area = "Employee" });
            }
            try
            {
                unitOfWork.CustomerRelationRepository.Insert(model);
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره اطلاعات خطایی رخ داده است لطفا دوباره تلاش کنید");
                    return RedirectToAction("Index", "Customers", new { area = "Employee" });
                }
                UTLAlert.Success(this, "با موفقیت ثبت شد");
            }
            catch (Exception e)
            {
                //log
                new UTLLog(unitOfWork).AddLog((int)Enums.Log.Error, 1, "C", JsonConvert.SerializeObject(model).ToString(), "Error", User.Identity.GetUserId(), e.Message);
            }
            return RedirectToAction("Index", "Customers", new { area = "Employee" });
        }





        // GET: Employee/CustomerRelation
        public ActionResult Index(int? customerId)
        {


            if (customerId.HasValue)
            {
                ViewBag.customerRelations = unitOfWork.CustomerRelationRepository.GetAll(c => c.customer_id == customerId);
            }

            return View();
        }

        public ActionResult Details(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    CustomerRelation customerRelation = unitOfWork.CustomerRelationRepository.GetByID(id.Value);
                    if (customerRelation != null)
                        return PartialView("_CustomerRelationdetails", customerRelation);
                }
            }
            catch
            {

            }
            return Json("failure");
        }


        [IsAjax]
        public ActionResult EditPartial(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var find = unitOfWork.CustomerRelationRepository.GetByID(id);

                    var category = unitOfWork.CustomersCategoriesBridgeRepository
                        .GetAll2(x => x.customer_id == find.customer_id, null, x => x.SiteValue);

                    var organization = unitOfWork.CustomersOrganizationBridgeRepository
                        .GetAll2(x => x.customer_id == find.customer_id, null, x => x.SiteValue);
                    ViewBag.OrganizationUnitsName = organization.Select(x => x.SiteValue.name);
                    ViewBag.CategoriesName = category.Select(x => x.SiteValue.name);


                    var cov = unitOfWork.CustomerOptionValueRepository.GetAll(x => x.customerRelation_id == find.id);
                    //گرفتن همه ی فیلد های داینامیک بر اساس گروه مشتریان و واحد سازمانی
                    List<SiteValue> list = new List<SiteValue>();
                    List<SiteValue> list2 = new List<SiteValue>();
                    List<SiteValue> list3 = new List<SiteValue>();
                    foreach (var item in category)
                    {
                        list.AddRange(unitOfWork.SiteValueRepository.GetAll(
                            x => x.isDelete == false && x.id == item.category_id));
                    }
                    foreach (var item in cov)
                    {
                        list2.AddRange(unitOfWork.SiteValueRepository.GetAll(
                            x => x.isDelete == false && x.id == item.SiteValue.parentId));
                    }
                    foreach (var item in organization)
                    {
                        list.AddRange(unitOfWork.SiteValueRepository.GetAll(
                            x => x.isDelete == false && x.id == item.organization_id));
                    }
                    list3 = list;
                    foreach (var siteValue in list2)
                    {
                        list3.Remove(siteValue);
                    }

                    ViewBag.NewDynamicCategory = list3.Where(x=>x.parentId==9).Select(x => x.id).ToList();
                    ViewBag.NewDynamicOrganization = list3.Where(x => x.parentId == 4).Select(x => x.id).ToList();
                    
                    CustomerRelationViewModels.Edit ViewMdoel = new CustomerRelationViewModels.Edit()
                    {
                        name = find.name,
                        nationalCode = find.nationalCode,
                        phone = find.phone,
                        postCode = find.postCode,
                        tell = find.tell,
                        fax = find.fax,
                        id = find.id,
                        Job = find.Job,
                        family = find.family,
                        customer_id = find.customer_id,
                    };
                    //ViewBag.NewDynamic = list;
                    return View(ViewMdoel);
                }
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("Index", "Customers", new { area = "Employee" });
            }
            catch (Exception e)
            {
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("Index", "Customers", new { area = "Employee" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerRelationViewModels.Edit model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمی باشد");
                    return RedirectToAction("Index", "Customers", new { area = "Employee" });
                }
                unitOfWork.CustomerRelationRepository.Edit(model, unitOfWork.CustomerOptionValueRepository);
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره اطلاعات خطایی رخ داده است لطفا دوباره تلاش کنید");
                    return RedirectToAction("Index", "Customers", new { area = "Employee" });
                }
                UTLAlert.Success(this, "با موفقیت ثبت شد");
                return RedirectToAction("Index", "Customers", new { area = "Employee" });
            }
            catch
            {
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("Index", "Customers", new { area = "Employee" });
            }
        }
        [IsAjax]
        public JsonResult Delete(int id)
        {
            try
            {
                unitOfWork.CustomerRelationRepository.Delete(id);
                unitOfWork.Save();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        #region Dynamic
        [IsAjax]
        public ActionResult AddOrganizationDynamicPartial(List<int> id)
        {
            List<SiteValue> list = new List<SiteValue>();
            foreach (var i in id)
            {
                list.AddRange(unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == i));
            }
            ViewBag.Options = list;
            return View();
        }
        [IsAjax]
        public ActionResult AddDynamicPartial(List<int> id)
        {
            List<SiteValue> list = new List<SiteValue>();
            foreach (var i in id)
            {
                list.AddRange(unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == i));
            }

            ViewBag.Options = list;

            return View();
        }
        public ActionResult EditDynamicPartial(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var siteValues = unitOfWork.SiteValueRepository.Entity().AsQueryable();
                    var dropDowns = (from cov in unitOfWork.CustomerOptionValueRepository.GetAll(x => x.customerRelation_id == id && x.type == Enums.OptionType.DropDown.ToString())
                                     join siteValue in siteValues on cov.option_id equals siteValue.id
                                     select new ProductOptionValueViewModels.Pov()
                                     {
                                         povId = cov.id,
                                         povValueId = (cov.value_id.HasValue ? cov.value_id.Value : 1),
                                         povValueName = " ",
                                         povStrValue = cov.strValue,
                                         povOptionId = cov.option_id,
                                         povOptionName = siteValue.name,
                                         isString = false,
                                         isRequired = UTLSiteValues.GetValuesField(siteValue.value).FirstOrDefault(x => x.key == Enums.SiteValueFieldJson.isRequired.ToString()).value == "True",
                                         type = Enums.OptionType.DropDown.ToString()

                                     }).ToList();

                    var strings = (from pov in unitOfWork.CustomerOptionValueRepository.GetAll(x => x.customerRelation_id == id && x.type == Enums.OptionType.String.ToString())
                                   join siteValue in siteValues on pov.option_id equals siteValue.id
                                   select new ProductOptionValueViewModels.Pov()
                                   {
                                       povId = pov.id,
                                       povStrValue = pov.strValue,
                                       povOptionId = pov.option_id,
                                       povOptionName = siteValue.name,
                                       isString = true,
                                       type = Enums.OptionType.String.ToString(),
                                   }).ToList();
                    dropDowns.AddRange(strings);
                    ViewBag.COV = dropDowns;

                    ViewBag.Dynamics = (from dropDown in dropDowns
                                        join sv in siteValues on dropDown.povOptionId equals sv.parentId
                                        select new CustomerViewModels.EditDynamicSelect()
                                        {
                                            optionName = dropDown.povOptionName,
                                            optionId = dropDown.povOptionId,
                                            values = sv
                                        }).ToList();



                    return View();
                }
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("Index", "Customers", new { area = "Employee" });
            }
            catch (Exception e)
            {
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("Index", "Customers", new { area = "Employee" });
            }

        }

        #endregion
    }
}
