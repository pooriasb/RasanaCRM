using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Insfrastructure.ManagePermission;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        IUnitOfWork unitOfWork;
        public TestController()
        {
            unitOfWork = new UnitOfWork();
        }
        
        public ActionResult Index()
        {
            Factor u = new Factor();
            ManagePermission managePermission = new ManagePermission(this);
            string irem = "false";
            if (managePermission.CheckFild(u, "status", PermissionCodeField.insert))
            {
                irem = "true";
               
            }
            else
            {

            }
            return Content(irem);
        }
     
        //public ActionResult test1(string term)
        //{
        //    if(term==null)
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    int code = 0;
        //    IEnumerable<Customer> customers;
        //    if (int.TryParse(term,out code))
        //    {
        //         customers= unitOfWork.CustomerRepository.GetAll(c => c.name.Contains(term) || (c.code!=null&&c.code==code));
        //    }
        //    else 
        //    {
        //        customers = unitOfWork.CustomerRepository.GetAll(c=>c.name.Contains(term));
        //    }
        //    List<JsonCustomerResult> result = new List<JsonCustomerResult>();
        //  if(customers!=null)
        //    {
        //        foreach(Customer c in customers)
        //        {
        //            result.Add(new JsonCustomerResult()
        //            {
        //                id = c.id,
        //                code = c.code ?? 0,
        //                nationalCode = c.nationalCode,
        //                postCode = c.postCode,
        //                name = c.name
        //            });
        //        }
        //    }else
        //    {
        //        result.Add(new JsonCustomerResult() { name = "هیچ فیلدی یافت نشد",nationalCode="-1" });

        //    }
          
        //    return Json(result,JsonRequestBehavior.AllowGet);
        //}
      
  
    }
   


}