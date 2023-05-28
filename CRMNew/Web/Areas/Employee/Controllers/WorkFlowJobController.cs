using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models.Utilities;
using Web.ViewModels.Enums;

namespace Web.Areas.Employee.Controllers
{
    public class WorkFlowJobController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public WorkFlowJobController()
        {
            this.unitOfWork=new UnitOfWork();
        }
        
        // GET: Employee/WorkFlowJob
        [IsAjax]
        public ActionResult AgreeDenyJobAjax(int id,string situation,string value)
        {
            try
            {
                if (id == 0 || situation == null || value == null)
                {
                    return Json(false);
                }
                var find = unitOfWork.WorkFlowJobRepository.GetByID(id);
                find.status = (byte)int.Parse(situation);
                find.replyMessage = value;
                find.replyDate = DateTime.Now;

                if (!unitOfWork.Save())
                {
                    return Json(false);
                }
                return Json(true);
            }
            catch (Exception e)
            {
                new UTLLog().AddLog((int)Enums.Log.Error,0,"C",$"id :{id} situation :{situation} value :{value}","Error",User.Identity.GetUserId(),e.Message);
                return Json(false);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Referral(int workFlowJobId,string userId,string referralMessage, int id)
        {
            //id is workflow id
            try
            {
                if (workFlowJobId == 0 || userId == null || referralMessage == null)
                {
                    UTLAlert.Danger(this,"مقادیر وارد شده صحیح نمی باشد");
                    return RedirectToAction("WorkFlowJobList", "WorkFlow", new {area = "Employee"});
                }
                var find = unitOfWork.WorkFlowJobRepository.GetByID(workFlowJobId);
                //find.status = (byte) Enums.WorkFlowStatus.Referral;
                int factor_id = int.Parse(find.object_id);
                var factorFind = unitOfWork.FactorRepository.Get(x => x.id == factor_id);
                factorFind.owner_id = userId;

                // new workFlowJob
                unitOfWork.WorkFlowJobRepository.Insert(User.Identity.GetUserId(),referralMessage,userId,factor_id.ToString(),id);

                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمی باشد");
                    return RedirectToAction("WorkFlowJobList", "WorkFlow", new { area = "Employee" });
                }
                UTLAlert.Success(this, "با موفقیت ثبت شد");
                return RedirectToAction("WorkFlowJobList", "WorkFlow", new { area = "Employee" });
            }
            catch (Exception e)
            {
                new UTLLog().AddLog((int)Enums.Log.Error, 0, "C", $"id :{workFlowJobId} userId :{userId} referralMessage :{referralMessage}", "Error", User.Identity.GetUserId(), e.Message);
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("WorkFlowJobList", "WorkFlow", new { area = "Employee" });
            }
        }
    }
}