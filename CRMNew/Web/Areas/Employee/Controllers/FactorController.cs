using Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models.Entity;
using Web.Models.Utilities;
using Web.ViewModels.Enums;

namespace Web.Areas.Employee.Controllers
{
    public class FactorController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public FactorController()
        {
            // tfs tests .... 
            this.unitOfWork = new UnitOfWork();
        }

        public ActionResult AddonList(int parentId)
        {
            var additions = unitOfWork.FactorRepository.GetAll(x => x.parent_id == parentId);

            UTLDateTime utlDateTime=new UTLDateTime();
            StringBuilder stringBuilder=new StringBuilder();
            stringBuilder.Append(" <table class='table'> <thead> <tr> <td>کد متمم</td> <td>تاریخ</td> <td>قیمت کل فاکتور</td> <td>نام صادر کننده</td> <td>جزئیات</td> </tr> </thead> ");

            foreach (var addition in additions)
            {
                stringBuilder.Append(" <tr> ");
                stringBuilder.Append($" <td>{addition.addonCode}</td> <td>{utlDateTime.convertToPersianDate(addition.dateTime.ToString())}</td> <td>{addition.priceTotalFactor}</td> <td>{new UTLWorkFlow().GetUserNameById(addition.user_id)}</td>");
                stringBuilder.Append($" <td> <a href='/Employee/Factor/Detail?factorId={addition.id}' class='btn bg-teal btn-sm'>جزئیات</a> <a href='/Employee/Factor?id={addition.id}' class='btn bg-teal btn-sm'><i class='fa fa-pencil'></i>ویرایش</a>");
                stringBuilder.Append(" </td> </tr>");
            }
            stringBuilder.Append(" </table>");
            return Content(stringBuilder.ToString());
        }
        public ActionResult List()
        {
            return View();
        }
        public ActionResult GetCustomer(string term)
        {
            if (term == null)
                return Json("", JsonRequestBehavior.AllowGet);
            int code = 0;
            IEnumerable<Customer> customers;
            if (int.TryParse(term, out code))
            {
                customers = unitOfWork.CustomerRepository.GetAll(c => c.name.Contains(term) || (c.code != null && c.code == code));
            }
            else
            {
                customers = unitOfWork.CustomerRepository.GetAll(c => c.name.Contains(term));
            }
            List<JsonCustomerResult> result = new List<JsonCustomerResult>();
            if (customers != null)
            {
                foreach (Customer c in customers)
                {
                    result.Add(new JsonCustomerResult()
                    {
                        id = c.id,
                        code = c.code ?? 0,
                        nationalCode = c.nationalCode,
                        postCode = c.postCode,
                        name = c.name
                    });
                }
            }
            else
            {
                result.Add(new JsonCustomerResult() { name = "هیچ فیلدی یافت نشد", nationalCode = "-1" });

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int factorId)
        {
            try
            {
                var find = unitOfWork.FactorRepository.GetByID(factorId);
                if (find!=null)
                {
                    return View(find);
                }
                else
                {
                    UTLAlert.Danger(this,"داده ای یافت نشد");
                    return View();
                }
            }
            catch (Exception e)
            {
                UTLAlert.Danger(this,"خطایی رخ داده است لطفا دوباره تلاش کنید");
                return View();
            }
            
        }
        [HttpGet]
        public PartialViewResult DetailPartial(int factorId)
        {
            try
            {
                var find = unitOfWork.FactorRepository.GetByID(factorId);
                if (find == null)
                {
                    UTLAlert.Danger(this, "داده ای یافت نشد");
                    return PartialView("DetailPartial");
                }
                return PartialView("DetailPartial", find);
            }
            catch (Exception e)
            {
                //log
                return PartialView("DetailPartial");
            }

        }
        public JsonResult CheckCodeFactor(int? code)
        {
            if (code.HasValue)
            {
                if (unitOfWork.FactorRepository.Get(c => c.code == code) != null)
                {

                    return Json(new { success = false, message = "کد فاکتور تکراری است" });
                }

            }
            return Json(new { success = true, massage = "okkk" });
        }
        public JsonResult GetFactorJsonDataTable(int draw, int start, int length)
        {
            string search = Request.QueryString["search[value]"];
            int sortColumn = -1;
            string sortDirection = "asc";


            UTLDateTime utlDateTime = new UTLDateTime();
            var data = unitOfWork.FactorRepository.GetAll().Select(x=>new DataTableViewModel.DataItem(){customerName=x.Customer.name,factorId= x.id,dateTime = utlDateTime.convertToPersianDateTime(x.dateTime.ToString()),factorCode = x.code.ToString()}).ToList();
            if (length == -1)
            {
                length = data.Count();
            }

            // note: we only sort one column at a time
            if (Request.QueryString["order[0][column]"] != null)
            {
                sortColumn = int.Parse(Request.QueryString["order[0][column]"]);
            }
            if (Request.QueryString["order[0][dir]"] != null)
            {
                sortDirection = Request.QueryString["order[0][dir]"];
            }

            DataTableViewModel.DataTableData dataTableData = new DataTableViewModel.DataTableData();
            dataTableData.draw = draw;
            dataTableData.recordsTotal = data.Count();
            int recordsFiltered = 0;
            dataTableData.data = FilterData(ref recordsFiltered, start, length, search, sortColumn, sortDirection,data);
            dataTableData.recordsFiltered = recordsFiltered;

            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }
        private List<DataTableViewModel.DataItem> FilterData(ref int recordFiltered, int start, int length, string search, int sortColumn, string sortDirection,List<DataTableViewModel.DataItem> data)
        {
            List<DataTableViewModel.DataItem> list = new List<DataTableViewModel.DataItem>();
            if (search == null)
            {
                list = data;
            }
            else
            {
                // simulate search
                foreach (DataTableViewModel.DataItem dataItem in data)
                {
                    if (dataItem.customerName.ToUpper().Contains(search.ToUpper()) ||
                        dataItem.factorId.ToString().Contains(search.ToUpper()) ||
                        dataItem.factorCode.Contains(search.ToUpper()))
                    {
                        list.Add(dataItem);
                    }
                }
            }

            // simulate sort
            if (sortColumn == 0)
            {// sort Name
                list.Sort((x, y) => SortString(x.customerName, y.customerName, sortDirection));
            }
            else if (sortColumn==1)
            {
                list.Sort((x, y) => SortString(x.factorCode, y.factorCode, sortDirection));
            }

            recordFiltered = list.Count;

            // get just one page of data
            list = list.GetRange(start, Math.Min(length, list.Count - start));

            return list;
        }
        private int SortString(string s1, string s2, string sortDirection)
        {
            return sortDirection == "asc" ? s1.CompareTo(s2) : s2.CompareTo(s1);
        }



        public ActionResult Index()
        {
            FactorViewModels factorView = new FactorViewModels(unitOfWork);
            int sellFactorStatusParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.SellFactorStatus);
            ViewBag.StatusList = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == sellFactorStatusParentId);
            return View(factorView);
        }
        /// <summary>
        /// متمم فاکتور
        /// </summary>
        /// <param name="parentId">ایدی فاکتور پدر</param>
        /// <returns></returns>
        public ActionResult AddonFactor(int parentId)
        {
            return View();
        }

        #region GetProducts and get valid code 
        public JsonResult GetValidCode(int? code)
        {
            try
            {
                
                if (unitOfWork.FactorRepository.ExsitCode(code))
                {
                    return Json("کد اشتباه است", JsonRequestBehavior.AllowGet);
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                UtlAlertJsonModel utlAlert = new UtlAlertJsonModel { Success = false, Message = ex.Message };
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
            
           
        }
        
        public JsonResult GetProducts(string term)
     {
         
            try
            {
                IEnumerable<Product> products;
                List<JsonProductResult> productResults= new List<JsonProductResult>();
                if (!string.IsNullOrEmpty(term))
                {
                    int code = 0;
                    if (int.TryParse(term, out code))
                    {
                        products = unitOfWork.ProductRepository.Where(c => (c.code == code || c.title.Contains(term.Trim())) && c.isDalete == false && c.isEnable == true).Take(10).ToList();
                    }
                    else
                        products = unitOfWork.ProductRepository.GetAll(c => c.title.Contains(term.Trim()) && c.isDalete == false && c.isEnable == true).Take(10).ToList() ;
                    if(products!=null)
                    {
                   
                        foreach(Product product in products)
                        {
                            List<JsonProductPrice> listPrice = new List<JsonProductPrice>();
                            foreach(var item in product.ProductPrices)
                            {
                                listPrice.Add(new JsonProductPrice() { id = item.id, price = item.price, vahed_name = item.SiteValue.name });
                            }
                            productResults.Add(new JsonProductResult() { Title = product.title, Code = product.code,id=product.id,Description=product.description,ProductPrices=listPrice});

                        }
                       
                    }
                    if (productResults.Count == 0)
                        productResults.Add(new JsonProductResult { Title = "هیچ فیلدی یافت نشد", id = -1, Code = -1 });
                        return Json(productResults, JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
              
            }
            catch
            {
                return Json("خطای سیستم", JsonRequestBehavior.AllowGet);
            }

           
        }
        [HttpPost]
        public ActionResult getProducts(int? id)
        {

            try
            {
                IEnumerable<Product> products;
                List<JsonProductResult> productResults = new List<JsonProductResult>();
                if (id.HasValue)
                {
                   
                    
                        products = unitOfWork.ProductRepository.GetAll(c =>c.id==id).Take(10).ToList();
                    if (products != null)
                    {

                        foreach (Product product in products)
                        {
                            List<JsonProductPrice> listPrice = new List<JsonProductPrice>();
                            foreach (var item in product.ProductPrices)
                            {
                                listPrice.Add(new JsonProductPrice() { id = item.id, price = item.price, vahed_name = item.SiteValue.name });
                            }
                            productResults.Add(new JsonProductResult() { Title = product.title, Code = product.code, id = product.id, Description = product.description, ProductPrices = listPrice });

                        }

                    }
                    if (productResults.Count == 0)
                        productResults.Add(new JsonProductResult { Title = "هیچ فیلدی یافت نشد", id = -1, Code = -1 });
                    return Json(productResults, JsonRequestBehavior.DenyGet);
                }
                return Json("", JsonRequestBehavior.DenyGet);

            }
            catch
            {
                return Json("خطای سیستم", JsonRequestBehavior.DenyGet);
            }


        }

        #endregion

        [HttpPost]
        public ActionResult FactorProductEdit(int? id)
        {
            try
            {
                FactorProductViewModel factorProduct = new FactorProductViewModel(unitOfWork, id);
                return PartialView("_FactorProductEdit", factorProduct);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        public ActionResult FactorProductAdd()
        {
            try
            {
                FactorProductViewModel factorProduct = new FactorProductViewModel(unitOfWork);
                return PartialView("_FactorProductAdd", factorProduct);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }


        public ActionResult Add(List<ResultFactor> resultFactor, Web.Models.Entity.Factor factor, List<FactorCost> FactorCostBase)
        {
            try
            {
                #region SetExpire
                if (factor.expair == null)
                {
                    factor.expireDate = DateTime.Now;
                }
                else
                {
                    int day = int.Parse(factor.expair);
                    factor.expireDate = DateTime.Now.AddDays(day);
                }
                #endregion
                // set user id and owner id
                factor.user_id = User.Identity.GetUserId();
                factor.owner_id = User.Identity.GetUserId();
                factor.status = factor.status;
                if(factor.dateTime==null)
                factor.dateTime = DateTime.Now;
                else
                {
                    string strDatetime = factor.dateTime.Value.ToString().Split(' ')[0];

                    factor.dateTime = CalendarMngr.PersianToJulian(strDatetime);
                    strDatetime = factor.dateTime.Value.ToString().Split(' ')[0] + ' ' + DateTime.Now.ToLongTimeString();
                    factor.dateTime = DateTime.Parse(strDatetime);
                   
                }
                if (resultFactor != null)
                    foreach (ResultFactor item in resultFactor)
                    {
                        factor.FactorItems.Add(item.FactorItem);

                    }
                if (FactorCostBase != null)
                    foreach (FactorCost cost in FactorCostBase)
                    {
                        factor.FactorCosts.Add(cost);
                    }
                unitOfWork.FactorRepository.Insert(factor);
                if (unitOfWork.Save())
                {
                    foreach (ResultFactor item in resultFactor)
                    {
                        if (item.FactorItemCost != null)
                            foreach (FactorItemCost fa in item.FactorItemCost)
                            {

                                fa.factorItem_id = item.FactorItem.id;
                                unitOfWork.FactorItemCostRepository.Insert(fa);

                            }


                    }

                    unitOfWork.Save();
                    this.Success("فاکتور با موفقیت ثبت شد", null, true);
                }



            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("index");
        }

        public ActionResult Edit(int? id)
        {
            FactorViewModels factorView = new FactorViewModels(unitOfWork,id);
            int sellFactorStatusParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.SellFactorStatus);
            ViewBag.StatusList = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == sellFactorStatusParentId);

            return View(factorView);
        }
        [HttpPost]
        public ActionResult Edit(List<ResultFactor> resultFactor, Web.Models.Entity.Factor factor, List<FactorCost> FactorCostBase, List<FactorItemCost> factorItemCostTemp)
        {
            try
            {
                if (factor.expair == null)
                {
                    factor.expireDate = DateTime.Now;
                }
                else
                {
                    int day = int.Parse(factor.expair);
                    factor.expireDate = DateTime.Now.AddDays(day);
                }
                if (factor.dateTime == null)
                    factor.dateTime = DateTime.Now;
                else
                {
                    string strDatetime = factor.dateTime.Value.ToString().Split(' ')[0];

                    factor.dateTime = CalendarMngr.PersianToJulian(strDatetime);
                    strDatetime = factor.dateTime.Value.ToString().Split(' ')[0] + ' ' + DateTime.Now.ToLongTimeString();
                    factor.dateTime = DateTime.Parse(strDatetime);

                }
                if (factorItemCostTemp != null)
                {
                    foreach (var item in factorItemCostTemp)
                    {
                        if (item.id > 0)
                            unitOfWork.FactorItemCostRepository.Update(item);
                        else
                            unitOfWork.FactorItemCostRepository.Insert(item);
                    }
                    unitOfWork.Save();
                }
                factor.user_id = User.Identity.GetUserId();
              //  factor.dateTime = DateTime.Now;
                if (resultFactor != null)
                    foreach (ResultFactor item in resultFactor)
                    {
                        item.FactorItem.factor_id = factor.id;
                        if (item.FactorItem.id != 0)
                        {

                            unitOfWork.FactorItemRepository.Update(item.FactorItem);
                        }
                        else
                        {
                            unitOfWork.FactorItemRepository.Insert(item.FactorItem);

                        }


                        if (item.FactorItemCost != null)
                            foreach (FactorItemCost fa in item.FactorItemCost)
                            {
                                if (fa.id == 0)
                                {


                                    item.FactorItem.FactorItemCosts.Add(fa);
                                }

                                else
                                {

                                    unitOfWork.FactorItemCostRepository.Update(fa);
                                }



                            }
                        unitOfWork.Save();

                    }
                if (FactorCostBase != null)
                    foreach (FactorCost cost in FactorCostBase)
                    {
                        if (cost.id == 0)
                            factor.FactorCosts.Add(cost);
                        else
                            unitOfWork.FactorCostRepository.Update(cost);

                    }

                unitOfWork.FactorRepository.Update(factor);
                if (unitOfWork.Save())
                    this.Success("فاکتور با موفقیت ثبت شد", null, true);
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Edit", factor.id);
        }


        //[HttpPost]
        //public ActionResult Edit(List<ResultFactor> resultFactor, Factor factor, List<FactorCost> FactorCostBase, List<FactorItemCost> factorItemCostTemp)
        //{
        //    try
        //    {
        //        if (factorItemCostTemp != null)
        //        {
        //            foreach(var item in factorItemCostTemp)
        //            {
        //                unitOfWork.FactorItemCostRepository.Update(item);
        //            }
        //            unitOfWork.Save();
        //        }
        //        factor.user_id = User.Identity.GetUserId();
        //        factor.dateTime = DateTime.Now;
        //        if (resultFactor != null)
        //            foreach (ResultFactor item in resultFactor)
        //            {
        //                item.FactorItem.factor_id = factor.id;
        //                if (item.FactorItem.id!=0)
        //                {

        //                    unitOfWork.FactorItemRepository.Update(item.FactorItem);
        //                }
        //                else
        //                {
        //                    unitOfWork.FactorItemRepository.Insert(item.FactorItem);

        //                }


        //                if (item.FactorItemCost != null)
        //                    foreach (FactorItemCost fa in item.FactorItemCost)
        //                    {
        //                        if (fa.id == 0)
        //                        {


        //                            item.FactorItem.FactorItemCosts.Add(fa);
        //                        }

        //                        else
        //                        {

        //                            unitOfWork.FactorItemCostRepository.Update(fa);
        //                        }



        //                    }
        //                unitOfWork.Save();

        //            }
        //        if (FactorCostBase != null)
        //            foreach (FactorCost cost in FactorCostBase)
        //            {
        //                if(cost.id==0)
        //                factor.FactorCosts.Add(cost);
        //                else
        //                   unitOfWork.FactorCostRepository.Update(cost);

        //            }

        //        unitOfWork.FactorRepository.Update(factor);
        //        if (unitOfWork.Save())
        //            this.Success("فاکتور با موفقیت ثبت شد", null, true);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return RedirectToAction("Edit", factor.id);
        //}
        //HACK : Pooria set
        public JsonResult DeleteItemFactor(int id) {

            try
            {
              var factorItemCosts =   unitOfWork.FactorItemCostRepository.GetAll(x => x.factorItem_id == id).ToList();

                FactorItem item = unitOfWork.FactorItemRepository.GetAll(x => x.id == id).SingleOrDefault();
                if (item != null)
                {

                    if (factorItemCosts != null)
                    {
                        foreach (var ItemCost in factorItemCosts)
                        {
                            unitOfWork.FactorItemCostRepository.Delete(ItemCost);

                        }
                    }
                    unitOfWork.FactorItemRepository.Delete(item);
                    unitOfWork.Save();
                }
                return Json(new { success = true, message = "آیتم با موفقیت حذف شد" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "خطا سیستم" });
            }
        }
    }
    public class ResultFactor
    {
        public FactorItem FactorItem { get; set; }
        public List<FactorItemCost> FactorItemCost { get; set; }
       
        
    }
    public class JsonProductResult
    {
        public int id { get; set; }
        public float Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<JsonProductPrice> ProductPrices { get; set; }
       // public IEnumerable<SiteValue> Brand { get; set; }


    }
    public class JsonProductPrice
    {
        public int id { get; set; }

        public string vahed_name { get; set; }
        public string price { get; set; }

    }
    public class JsonCustomerResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public int code { get; set; }
        public string nationalCode { get; set; }
        public string postCode { get; set; }

    }
}