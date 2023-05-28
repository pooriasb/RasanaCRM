using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Insfrastructure.Utilities.DataTable;
using Web.Models.Entity;
using Web.Models.Utilities;
using Web.ViewModels.Enums;

namespace Web.Areas.Employee.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductController()
        {
            this.unitOfWork = new UnitOfWork();
        }
        // GET: Employee/Product
        public ActionResult Index()
        {
            return View();
        }

        #region IndexJson
        public JsonResult IndexJson(int draw, int start, int length)
        {
            string search = Request.QueryString["search[value]"];
            int sortColumn = -1;
            string sortDirection = "asc";

            UTLDateTime utlDateTime = new UTLDateTime();
            int productUnitParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.productUnit);
            var data = unitOfWork.ProductRepository.GetAll(x => x.isDalete == false)
                .Select(x => new ProductViewModels.ProductJson()
                {
                    category_id = x.category_id,
                    code = x.code,
                    createDate = utlDateTime.convertToPersianDateTime(x.createDate.ToString()),
                    id = x.id,
                    isEnable = (x.isEnable ? "<i class='fa fa-check'></i>" : "<i class='fa fa-close'></i>"),
                    lastUpdateUser_id = x.lastUpdateUser_id,
                    localId = x.localId,
                    title = x.title,
                    user_id = x.user_id,
                    categoryName = x.SiteValue.name,
                    description = (x.description == null ? " " : x.description),
                    ProductPrices = (from pp in x.ProductPrices
                                     join siteValue in unitOfWork.SiteValueRepository.GetAll(yy => yy.parentId == productUnitParentId) on pp.vahed_id equals siteValue.id
                                     select new ProductPriceViewModels.ProductPriceJson() { price = pp.price, vahed_id = pp.vahed_id.Value, vahedName = siteValue.name }).ToList()
                }).ToList();
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

            DataTableModels<ProductViewModels.ProductJson> dataTableData = new DataTableModels<ProductViewModels.ProductJson>();
            dataTableData.draw = draw;
            dataTableData.recordsTotal = dataCount;
            int recordsFiltered = 0;
            dataTableData.data = IndexJsonFilter(ref recordsFiltered, start, length, search, sortColumn, sortDirection, data);
            dataTableData.recordsFiltered = recordsFiltered;

            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }
        private List<ProductViewModels.ProductJson> IndexJsonFilter(ref int recordFiltered, int start, int length, string search, int sortColumn, string sortDirection, List<ProductViewModels.ProductJson> data)
        {
            List<ProductViewModels.ProductJson> list = new List<ProductViewModels.ProductJson>();
            if (search == null || search == "")
            {
                list = data;
            }
            else
            {
                // simulate search
                try
                {
                    foreach (ProductViewModels.ProductJson dataItem in data)
                    {
                        if (dataItem.code.ToString().Contains(search) ||
                            dataItem.title.ToLower().Contains(search.ToLower()) ||
                            dataItem.categoryName.ToLower().Contains(search.ToLower()) ||
                            dataItem.description.Contains(search.ToLower()))
                        {
                            list.Add(dataItem);
                        }
                    }
                }
                catch (Exception e)
                {
                }

            }

            // simulate sort
            if (sortColumn == 1)
            {//code
                list.Sort((x, y) => UTLDataTable.SortString(x.code.ToString().ToString(), y.code.ToString(), sortDirection));
            }
            else if (sortColumn == 2)
            {//title
                list.Sort((x, y) => UTLDataTable.SortString(x.title, y.title, sortDirection));
            }
            else if (sortColumn == 3)
            {//categoryName
                list.Sort((x, y) => UTLDataTable.SortString(x.categoryName, y.categoryName, sortDirection));
            }
            else if (sortColumn == 4)
            {//description
                list.Sort((x, y) => UTLDataTable.SortString(x.description, y.description, sortDirection));
            }
            recordFiltered = list.Count;

            // get just one page of data
            list = list.GetRange(start, Math.Min(length, list.Count - start));

            return list;
        }
        #endregion
        public ActionResult Details(int id)
        {
            try
            {
                var find = unitOfWork.ProductRepository.GetByID(id);
                ViewBag.Name = find.title;
                var siteValues = unitOfWork.SiteValueRepository.GetAll();
                var dropDowns = (from pov in unitOfWork.ProductOptionValueRepository.GetAll(x => x.product_id == id)
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

                var strings = (from pov in unitOfWork.ProductOptionValueRepository.GetAll(x => x.product_id == id && x.value_id == null)
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
                return View(find);
            }
            catch (Exception e)
            {
                //log
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return View();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductViewModels.Add model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("Index");
            }
            //first check not duplicate code
            var findCodeId = unitOfWork.ProductRepository.GetAll(x => x.code == model.code).Select(x => x.id);
            if (findCodeId.Count() != 0)
            {
                UTLAlert.Danger(this, "مقدار کد وارد شده تکراری می باشد");
                return RedirectToAction("Index");
            }
            try
            {
                unitOfWork.ProductRepository.Insert(model, User.Identity.GetUserId());
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره اطلاعات خطایی رخ داده است لطفا دوباره تلاش کنید");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                //log
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModels.Edit model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمیاشد");
                return RedirectToAction("Index");
            }
            try
            {
                var codeFind = unitOfWork.ProductRepository.Get(x => x.code == model.code && x.id != model.id);
                if (codeFind != null)
                {
                    UTLAlert.Danger(this, "مقدار کد وارد شده تکراری می باشد");
                    return RedirectToAction("Index");
                }
                unitOfWork.ProductRepository.Edit(model, User.Identity.GetUserId(), unitOfWork);
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره اطلاعات خطایی رخ داده است لطفا دوباره تلاش کنید");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                //log
            }

            return RedirectToAction("Index");
        }

        [IsAjax]
        public JsonResult Delete(int id)
        {
            try
            {
                unitOfWork.ProductRepository.DeletePhysical(id);
                unitOfWork.Save();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        #region Dynamic-CRUD

        [IsAjax]
        public ActionResult AddDynamicPartial(int id)
        {
            //id is categoryId in siteValue table
            ViewBag.Options = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == id);
            return View();
        }
        [IsAjax]
        public ActionResult AddDynamicPartial2(int id, int productId)
        {
            //id is categoryId in siteValues table

            var find = unitOfWork.ProductRepository.GetByID(productId);
            var records = unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == id);
            List<SiteValue>list=new List<SiteValue>();
            foreach (var item in records)
            {
                var isDuplicate = find.ProductOptionValues.Where(x => x.option_id == item.id).FirstOrDefault();
                if (isDuplicate==null)
                {
                    list.Add(item);
                }
            }
            ViewBag.Options = list;
            //ViewBag.Options =
            //    unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == id &&
            //                                               find.ProductOptionValues.Any(y => y.option_id != x.id));

            return View();
        }
        public ActionResult EditDynamic(int id)
        {
            // id is productId in productOptionValue table
            var siteValues = unitOfWork.SiteValueRepository.GetAll();
            var dropDowns = (from pov in unitOfWork.ProductOptionValueRepository.GetAll(x => x.product_id == id)
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

            var strings = (from pov in unitOfWork.ProductOptionValueRepository.GetAll(x => x.product_id == id && x.value_id == null)
                           join siteValue in siteValues on pov.option_id equals siteValue.id
                           select new ProductOptionValueViewModels.Pov()
                           {
                               povId = pov.id,
                               povStrValue = pov.strValue,
                               povOptionId = pov.option_id,
                               povOptionName = siteValue.name,
                               isString = true
                           }).ToList();
            dropDowns.AddRange(strings);
            ViewBag.Pov = dropDowns;
            ViewBag.ProductName = unitOfWork.ProductRepository.GetByID(id).title;
            return View();
        }

        public ActionResult EditDynamicPartial(int id)
        {
            // id is productId in productOptionValue table
            var siteValues = unitOfWork.SiteValueRepository.GetAll();
            var dropDowns = (from pov in unitOfWork.ProductOptionValueRepository.GetAll(x => x.product_id == id)
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

            var strings = (from pov in unitOfWork.ProductOptionValueRepository.GetAll(x => x.product_id == id && x.value_id == null)
                           join siteValue in siteValues on pov.option_id equals siteValue.id
                           select new ProductOptionValueViewModels.Pov()
                           {
                               povId = pov.id,
                               povStrValue = pov.strValue,
                               povOptionId = pov.option_id,
                               povOptionName = siteValue.name,
                               isString = true
                           }).ToList();
            dropDowns.AddRange(strings);
            ViewBag.Pov = dropDowns;
            ViewBag.ProductName = unitOfWork.ProductRepository.GetByID(id).title;
            return View();
        }
        #endregion



        [IsAjax]
        public ActionResult AddPartial()
        {
            ViewBag.Categories = new MultiSelectList(unitOfWork.SiteValueRepository.GetCategories(), "CategoryId", "CategoryName");
            return View();
        }



        [IsAjax]
        public ActionResult EditPartial(int id)
        {
            var find = unitOfWork.ProductRepository.GetByID(id);

            ProductViewModels.Edit editViewModel = new ProductViewModels.Edit()
            {
                id = find.id,
                categoryId = find.category_id,
                description = find.description,
                isEnable = find.isEnable,
                title = find.title,
                productPrices = find.ProductPrices.Where(x => x.isDelete == false).Select(x => new ProductPriceViewModels.EditProductPrice() { price = x.price, productId = x.product_id, productPriceId = x.id, vahedName = x.SiteValue.name }).ToList(),
                code = find.code,
            };
            ViewBag.Categories = new MultiSelectList(unitOfWork.SiteValueRepository.GetCategories(), "CategoryId", "CategoryName");

            return View(editViewModel);
        }
        [IsAjax]
        public JsonResult GetMaxCodeByAjax()
        {
            float maxCode = unitOfWork.ProductRepository.GetMaxCode();
            return Json(maxCode);
        }
    }
}