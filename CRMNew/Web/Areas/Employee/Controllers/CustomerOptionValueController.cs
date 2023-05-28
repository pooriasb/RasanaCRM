using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public class CustomerOptionValueController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CustomerOptionValueController()
        {
            this.unitOfWork = new UnitOfWork();
        }
        // GET: Employee/CustomerOptionValue
        public ActionResult Index(int type = 1)
        {
            ViewBag.Type = type;
            if (type == 1)
            {
                // customer category
                int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.CustomerCategory);
                ViewBag.SiteValues =
                    unitOfWork.SiteValueRepository.GetAll(x => x.parentId == parentId && x.isDelete==false);
                ViewBag.Title = "لیست گروه مشتریان";
            }
            else if (type == 2)
            {
                //// customer organization
                //int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.OrganizationUnit);
                //ViewBag.SiteValues =
                //    unitOfWork.SiteValueRepository.GetAll(x => x.parentId == parentId && x.isDelete == false);
                ViewBag.Title = "لیست سازمان ها";
            }
            return View();
        }
        #region IndexJson

        public JsonResult IndexJson(int draw, int start, int length)
        {
            string search = Request.QueryString["search[value]"];
            int sortColumn = -1;
            string sortDirection = "asc";

            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.OrganizationUnit);
            var data = unitOfWork.SiteValueRepository.GetAll(x => x.parentId == parentId && x.isDelete == false)
                .Select(x => new CustomerOptionValueViewModels.IndexJson()
                {
                    id = x.id,
                    name = x.name,
                    SiteValue = x.SiteValues1.Select(y=>new CustomerOptionValueViewModels.IndexJson()
                    {
                        id=y.id,
                        name = y.name,
                    }).ToList()
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

            DataTableModels<CustomerOptionValueViewModels.IndexJson> dataTableData = new DataTableModels<CustomerOptionValueViewModels.IndexJson>();
            dataTableData.draw = draw;
            dataTableData.recordsTotal = dataCount;
            int recordsFiltered = 0;
            dataTableData.data = IndexJsonFilter(ref recordsFiltered, start, length, search, sortColumn, sortDirection, data);
            dataTableData.recordsFiltered = recordsFiltered;

            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }
        private List<CustomerOptionValueViewModels.IndexJson> IndexJsonFilter(ref int recordFiltered, int start, int length, string search, int sortColumn, string sortDirection, List<CustomerOptionValueViewModels.IndexJson> data)
        {
            List<CustomerOptionValueViewModels.IndexJson> list = new List<CustomerOptionValueViewModels.IndexJson>();
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
                        if (dataItem.name.ToString().Contains(search))
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
            recordFiltered = list.Count;

            // get just one page of data
            list = list.GetRange(start, Math.Min(length, list.Count - start));

            return list;
        }

        #endregion
    }
}