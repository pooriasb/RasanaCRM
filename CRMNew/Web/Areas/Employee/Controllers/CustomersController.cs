using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Insfrastructure.Utilities.DataTable;
using Web.Models.Entity;
using Web.Models.Utilities;
using Web.ViewModels.Enums;

namespace Web.Areas.Employee.Controllers
{
    public class CustomersController : Controller
    {
        #region Infrastracture
        IUnitOfWork unitOfWork;
        public CustomersController()
        {
            unitOfWork = new UnitOfWork();
        }
        // GET: Employee/Customers
        public ActionResult Index()
        {

            return View();
        }
        #endregion
        #region IndexJson

        public JsonResult IndexJson(int draw, int start, int length)
        {
            string search = Request.QueryString["search[value]"];
            int sortColumn = -1;
            string sortDirection = "asc";

            UTLDateTime utlDateTime = new UTLDateTime();
            var data = unitOfWork.CustomerRepository.GetAll2(x => x.isDelete == false, null, x => x.CustomerRelations)
                .Select(x => new CustomerViewModels.CustomerJson()
                {
                    id = x.id,
                    //accountNumber = x.accountNumber,
                    //address = x.address,
                    //address1 = x.address1,
                    //address2 = x.address2,
                    name = x.name,
                    code = x.code,
                    //economicCode =  x.economicCode,
                    //fax =  x.fax,
                    //localId = x.localId.Value,
                    nationalCode = (x.nationalCode == null ? "خالی" : x.nationalCode),
                    phone = (x.phone == null ? "خالی" : x.phone),
                    //postCode = x.postCode,
                    tell = (x.tell == null ? "خالی" : x.tell),
                    //tell1 = x.tell1,
                    //tell2 = x.tell2,
                    province_id = x.province_id,
                    city_id = x.city_id,
                    cityName = unitOfWork.SiteValueRepository.GetByID(x.city_id).name,
                    provinceName = x.SiteValue.name,
                    dateTime = x.dateTime,
                    dateTimePersian = utlDateTime.convertToPersianDateTime(x.dateTime.ToString()),
                    description = (x.description == null ? "خالی" : x.description),
                    CustomerRelationJsons = x.CustomerRelations.Select(y => new CustomerRelationViewModels.CustomerRelationJson()
                    {
                        customerRelationId = y.id,
                        family = y.family,
                        fax = (y.fax == null ? "...." : y.fax),
                        Job = (y.Job == null ? "...." : y.Job),
                        name = (y.name == null ? "...." : y.name),
                        phone = (y.phone == null ? "...." : y.phone),
                        nationalCode = (y.nationalCode == null ? "...." : y.nationalCode),
                        tell = (y.tell == null ? "...." : y.tell),
                        postCode = (y.postCode == null ? "...." : y.postCode)
                    }).ToList()
                }).OrderByDescending(x => x.dateTime).ToList();
            int dataCount = data.Count();
            if (length == -1)
                length = dataCount;

            // note: we only sort one column at a time
            if (Request.QueryString["order[0][column]"] != null)
            {
                sortColumn = int.Parse(Request.QueryString["order[0][column]"]);
            }
            if (Request.QueryString["order[0][dir]"] != null)
            {
                sortDirection = Request.QueryString["order[0][dir]"];
            }

            DataTableModels<CustomerViewModels.CustomerJson> dataTableData = new DataTableModels<CustomerViewModels.CustomerJson>();
            dataTableData.draw = draw;
            dataTableData.recordsTotal = dataCount;
            int recordsFiltered = 0;
            dataTableData.data = IndexJsonFilter(ref recordsFiltered, start, length, search, sortColumn, sortDirection, data);
            dataTableData.recordsFiltered = recordsFiltered;

            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }
        private List<CustomerViewModels.CustomerJson> IndexJsonFilter(ref int recordFiltered, int start, int length, string search, int sortColumn, string sortDirection, List<CustomerViewModels.CustomerJson> data)
        {
            List<CustomerViewModels.CustomerJson> list = new List<CustomerViewModels.CustomerJson>();
            if (search == null || search == "")
            {
                list = data;
            }
            else
            {
                // simulate search
                try
                {
                    foreach (var dataItem in data)
                    {
                        if (dataItem.code.ToString().Contains(search) ||
                            dataItem.name.ToLower().Contains(search.ToLower()) ||
                            // dataItem.phone.ToLower().Contains(search.ToLower()) ||
                            dataItem.provinceName.ToLower().Contains(search.ToLower()) ||
                            dataItem.cityName.ToLower().Contains(search.ToLower()))
                        {
                            list.Add(dataItem);
                        }
                    }
                }
                catch (Exception e)
                {
                    var xx = e.Message;
                }

            }

            // simulate sort
            if (sortColumn == 1)
            {//code
                list.Sort((x, y) => UTLDataTable.SortInteger(x.code.ToString().ToString(), y.code.ToString(), sortDirection));
            }
            else if (sortColumn == 2)
            {//name
                list.Sort((x, y) => UTLDataTable.SortString(x.name, y.name, sortDirection));
            }
            else if (sortColumn == 3)
            {//provinceName
                list.Sort((x, y) => UTLDataTable.SortString(x.provinceName, y.provinceName, sortDirection));
            }
            else if (sortColumn == 4)
            {//cityname
                list.Sort((x, y) => UTLDataTable.SortString(x.cityName, y.cityName, sortDirection));
            }
            else if (sortColumn == 5)
            {//phone
                list.Sort((x, y) => UTLDataTable.SortString(x.phone, y.phone, sortDirection));
            }
            else if (sortColumn == 6)
            {//tell
                list.Sort((x, y) => UTLDataTable.SortString(x.tell, y.tell, sortDirection));
            }
            //else if (sortColumn == 7)
            //{//description
            //    list.Sort((x, y) => UTLDataTable.SortString(x.description, y.description, sortDirection));
            //}
            recordFiltered = list.Count;

            // get just one page of data
            list = list.GetRange(start, Math.Min(length, list.Count - start));

            return list;
        }

        #endregion
        #region  Addpartioal - CustomerAdd - Detail - EditPartial - Edit


        [IsAjax]
        public ActionResult AddPartial()
        {
            int provinceParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.province);
            int customerCategoryParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.CustomerCategory);
            int organizationUnitParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.OrganizationUnit);
            ViewBag.Provinces = new MultiSelectList(unitOfWork.SiteValueRepository.GetIdNameByParentId(provinceParentId), "id", "name");
            ViewBag.CustomerCategories = new MultiSelectList(unitOfWork.SiteValueRepository.GetIdNameByParentId(customerCategoryParentId), "id", "name");
            ViewBag.OrganizationUnits = new MultiSelectList(unitOfWork.SiteValueRepository.GetIdNameByParentId(organizationUnitParentId), "id", "name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CustomerViewModels.Add model)
        {
            if (!ModelState.IsValid)
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        Console.Write(error.ErrorMessage);
                    }
                }
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمی باشد");
                return RedirectToAction("Index", "Customers", new { area = "Employee" });
            }
            var findCodeId = unitOfWork.CustomerRepository.GetAll(x => x.code == model.code).Select(x => x.id);
            if (findCodeId.Count() != 0)
            {
                UTLAlert.Danger(this, "مقدار کد وارد شده تکراری می باشد");
                return RedirectToAction("Index", "Customers", new { area = "Employee" });
            }
            try
            {
                unitOfWork.CustomerRepository.Insert(model);
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره اطلاعات خطایی رخ داده است لطفا دوباره تلاش کنید");
                    return RedirectToAction("Index", "Customers", new { area = "Employee" });
                }
            }
            catch (Exception e)
            {
                //log
                new UTLLog(unitOfWork).AddLog((int)Enums.Log.Error, 1, "C", JsonConvert.SerializeObject(model).ToString(), "Error", User.Identity.GetUserId(), e.Message);
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("Index", "Customers", new { area = "Employee" });
            }
            return RedirectToAction("Index", "Customers", new { area = "Employee" });

        }
        public ActionResult Detail(int? id)
        {
            try
            {
                Customer customer = unitOfWork.CustomerRepository.GetByID(id);
                var siteValues = unitOfWork.SiteValueRepository.Where(x => x.isDelete == false);



                //var yy=(from cov in unitOfWork.CustomerOptionValueRepository.Where(x=>x.customer_id==id)
                //    join ccbr in unitOfWork.CustomersCategoriesBridgeRepository.Entity().AsQueryable() on cov.customer_id equals ccbr.customer_id
                //    join siteValue in unitOfWork.SiteValueRepository.Where(x=>x.isDelete==false) on ccbr.category_id equals siteValue.id
                //    join sv in siteValues on ccbr.category_id equals sv.parentId 
                //    select new {categoryName=siteValue.name,category_id=ccbr.category_id,optionName=})

                var dropDowns = (from pov in unitOfWork.CustomerOptionValueRepository.Where(x => x.customer_id == id)
                    join siteValue in siteValues on pov.option_id equals siteValue.id
                    join sv in siteValues on pov.value_id equals sv.id
                    select new ProductOptionValueViewModels.Pov()
                    {
                        povId = pov.id,
                        povValueId = pov.value_id.Value,
                        povValueName = sv.name,
                        povStrValue = pov.strValue,
                        povOptionId = pov.option_id,
                        povOptionName = siteValue.name,
                        isString = false
                    }).ToList();

                var strings = (from pov in unitOfWork.CustomerOptionValueRepository.GetAll(x => x.customer_id == id && x.value_id == null)
                    join siteValue in siteValues on pov.option_id equals siteValue.id
                    select new ProductOptionValueViewModels.Pov()
                    {
                        povId = pov.id,
                        povStrValue = pov.strValue,
                        povOptionId = pov.option_id,
                        povOptionName = siteValue.name,
                        isString = true
                    }).ToList();
                ViewBag.DropDowns = dropDowns;
                ViewBag.Strings = strings;
                return View(customer);
            }
            catch
            {
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("Index", "Customers", new { area = "Employee" });
            }

        }
        [IsAjax]
        public ActionResult EditPartial(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var find = unitOfWork.CustomerRepository.GetByID(id);
                    int provinceParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.province);
                    int customerCategoryParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.CustomerCategory);
                    int organizationUnitParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.OrganizationUnit);
                    ViewBag.Provinces = new MultiSelectList(unitOfWork.SiteValueRepository.GetIdNameByParentId(provinceParentId), "id", "name");
                    ViewBag.Cities = new MultiSelectList(unitOfWork.SiteValueRepository.GetIdNameByParentId(find.province_id.Value), "id", "name");

                    var categories = unitOfWork.SiteValueRepository.GetIdNameByParentId(customerCategoryParentId);
                    ViewBag.Categories = new MultiSelectList(categories,"id","name");

                    var organizations = unitOfWork.SiteValueRepository.GetIdNameByParentId(organizationUnitParentId);
                    ViewBag.Organizations = new MultiSelectList(organizations,"id","name");
                    var result = categories.Where(x => !find.CustomersCategoriesBridges.Any(y => y.category_id == x.id))
                        .ToList();
                    var result2 = organizations
                        .Where(x => !find.CustomersOrganizationsBridges.Any(y => y.organization_id == x.id)).ToList();
                    ViewBag.CustomerCategories = new MultiSelectList(result, "id", "name");
                    ViewBag.OrganizationUnits = new MultiSelectList(result2, "id", "name");
                    CustomerViewModels.Edit ViewMdoel = new CustomerViewModels.Edit()
                    {
                        address = find.address,
                        address2 = find.address2,
                        address1 = find.address1,
                        city_id = find.city_id,
                        province_id = find.province_id,
                        code = find.code,
                        economicCode = find.economicCode,
                        name = find.name,
                        nationalCode = find.nationalCode,
                        phone = find.phone,
                        postCode = find.postCode,
                        tell = find.tell,
                        tell1 = find.tell1,
                        tell2 = find.tell2,
                        fax = find.fax,
                        accountNumber = find.accountNumber,
                        id = find.id,
                        customerCategory_id = find.CustomersCategoriesBridges.Select(x => x.category_id).ToList(),
                        organizationUnit_id = find.CustomersOrganizationsBridges.Select(x => x.organization_id).ToList(),
                        //cityName = unitOfWork.SiteValueRepository.GetByID(find.city_id).name
                    };


                    var category = unitOfWork.CustomersCategoriesBridgeRepository
                        .GetAll2(x => x.customer_id == id, null, x => x.SiteValue);

                    var organization = unitOfWork.CustomersOrganizationBridgeRepository
                        .GetAll2(x => x.customer_id == id, null, x => x.SiteValue);
                    ViewBag.OrganizationUnitsName = organization.Select(x => x.SiteValue.name);
                    ViewBag.CategoriesName = category.Select(x => x.SiteValue.name);


                    var cov = unitOfWork.CustomerOptionValueRepository.GetAll(x => x.customer_id == id);
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

                    ViewBag.NewDynamicCategory = list3.Where(x => x.parentId == 9).Select(x => x.id).ToList();
                    ViewBag.NewDynamicOrganization = list3.Where(x => x.parentId == 4).Select(x => x.id).ToList();

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
        public ActionResult Edit(CustomerViewModels.Edit model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمی باشد");
                    return RedirectToAction("Index", "Customers", new { area = "Employee" });
                }
                var find = unitOfWork.CustomerRepository.Get(x => x.code == model.code && x.id != model.id);
                if (find != null)
                {
                    UTLAlert.Danger(this, "کد تکراری می باشد");
                    return RedirectToAction("Index", "Customers", new { area = "Employee" });
                }
                unitOfWork.CustomerRepository.Edit(model,unitOfWork.CustomerOptionValueRepository);
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
        #endregion
        #region Dynamic
        [IsAjax]
        public ActionResult AddDynamicPartial(List<int> id)
        {
            List<SiteValue> list=new List<SiteValue>();
            foreach (var i in id)
            {
                list.AddRange(unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == i));
            }

            ViewBag.Options = list;

            return View();
        }
        [IsAjax]
        public ActionResult EditDynamicPartial(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var siteValues = unitOfWork.SiteValueRepository.Entity().AsQueryable();
                    var dropDowns = (from cov in unitOfWork.CustomerOptionValueRepository.GetAll(x => x.customer_id == id && x.type==Enums.OptionType.DropDown.ToString())
                        join siteValue in siteValues on cov.option_id equals siteValue.id
                        join sv in siteValues on cov.value_id equals sv.id
                        select new ProductOptionValueViewModels.Pov()
                        {
                            povId = cov.id,
                            povValueId = cov.value_id.Value,
                            povValueName = sv.name,
                            povStrValue = cov.strValue,
                            povOptionId = cov.option_id,
                            povOptionName = siteValue.name,
                            isString = false,
                            isRequired = UTLSiteValues.GetValuesField(siteValue.value).FirstOrDefault(x=>x.key==Enums.SiteValueFieldJson.isRequired.ToString()).value=="True",
                            type = Enums.OptionType.DropDown.ToString(),
                        }).ToList();

                    var strings = (from pov in unitOfWork.CustomerOptionValueRepository.GetAll(x => x.customer_id == id && x.type==Enums.OptionType.String.ToString())
                        join siteValue in siteValues on pov.option_id equals siteValue.id
                        select new ProductOptionValueViewModels.Pov()
                        {
                            povId = pov.id,
                            povStrValue = pov.strValue,
                            povOptionId = pov.option_id,
                            povOptionName = siteValue.name,
                            isString = true,
                            type = Enums.OptionType.String.ToString()
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
        [IsAjax]
        public JsonResult EditOptionValueAjax(int id, string value)
        {
            try
            {
                var povFind = unitOfWork.CustomerOptionValueRepository.GetByID(id);
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
        public JsonResult EditOptionValueByIdAjax(int povId, int optionValueId)
        {
            try
            {
                var find = unitOfWork.CustomerOptionValueRepository.GetByID(povId);
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
        public ActionResult AddOrganizationDynamicPartial(List<int> id)
        {
            //var session = Session["AddOrganizationDynamicPartialId"];
            //if (id.Count != 0)
            //{
            //    var i = id.LastOrDefault();
            //    string savePart = "";
            //    if (session != null)
            //    {
            //        string[] parts = session.ToString().Split(',');
            //        var find = parts.ToList().Where(x => x == i.ToString()).FirstOrDefault();

            //        if (find == null)
            //        {
            //            savePart += $"{i},";
            //            Session["AddOrganizationDynamicPartialId"] += savePart;
            //            var xx = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == i);

            //            ViewBag.Options = xx;
            //        }
            //        else
            //        {
            //            ViewBag.Options = new List<SiteValue>();
            //        }
            //    }
            //    else
            //    {
            //        var xx = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == i);
            //        savePart += $"{i},";
            //        Session["AddOrganizationDynamicPartialId"] = savePart;
            //        ViewBag.Options = xx;
            //    }
            //}

            List<SiteValue> list = new List<SiteValue>();
            foreach (var i in id)
            {
                list.AddRange(unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == i));
            }
            ViewBag.Options = list;
            return View();
        }


        #endregion
        #region Delete-CheckCode-GetCities-GetMaxCodeByAjax
        [IsAjax]
        public JsonResult Delete(int id)
        {
            try
            {
                var find = unitOfWork.CustomerRepository.GetByID(id);
                find.isDelete = true;
                unitOfWork.Save();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        [IsAjax]
        public JsonResult CheckCode(int? code)
        {
            try
            {
                var find = unitOfWork.CustomerRepository.GetAll(x => x.code == code.Value);
                if (find != null)
                {
                    return Json(false);
                }
                return Json(true);
            }
            catch (Exception e)
            {
                new UTLLog(unitOfWork).AddLog((int)Enums.Log.Error, 1, "R", code.Value.ToString(), "Error",
                    User.Identity.GetUserId(), e.Message);
                return Json(false);
            }

        }
        [IsAjax]
        public JsonResult GetCities(int? parentId)
        {
            try
            {
                if (parentId.Value != 0)
                {
                    return Json(unitOfWork.SiteValueRepository.GetAll(c => c.parentId == parentId)
                        .Select(c => new { name = c.name, id = c.id }).ToList());
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
        public JsonResult GetMaxCodeByAjax()
        {
            float maxCode = unitOfWork.CustomerRepository.GetMaxCode();
            return Json(maxCode);
        }
        #endregion
    
    }

}