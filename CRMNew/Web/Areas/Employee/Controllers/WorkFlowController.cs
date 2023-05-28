using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Insfrastructure.Utilities.DataTable;
using Web.Models.Entity;
using Web.Models.Utilities;
using Web.ViewModels.Enums;

namespace Web.Areas.Employee.Controllers
{
    public class WorkFlowController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        int sectionTypeParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.SectionType);
        public WorkFlowController()
        {
            this.unitOfWork = new UnitOfWork();
        }
        public WorkFlowController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            this.unitOfWork = new UnitOfWork();
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        // GET: Employee/WorkFlow
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Add()
        {

            ViewBag.SectionTypes = new MultiSelectList(unitOfWork.SiteValueRepository.GetIdNameByParentId(sectionTypeParentId), "id", "name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(WorkFlowViewModels.Add model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمی باشد");
                ViewBag.SectionTypes = new MultiSelectList(unitOfWork.SiteValueRepository.GetIdNameByParentId(sectionTypeParentId), "id", "name");
                return View(model);
            }
            try
            {
                //check duplicate code
                var codeFind = unitOfWork.WorkFlowRepository.Get(x => x.code == model.code);
                if (codeFind != null)
                {
                    UTLAlert.Danger(this, $"کد {model.code} تکراری می باشد", "", true);
                    ViewBag.SectionTypes = new MultiSelectList(unitOfWork.SiteValueRepository.GetIdNameByParentId(sectionTypeParentId), "id", "name");
                    return View(model);
                }
                unitOfWork.WorkFlowRepository.Insert(model, User.Identity.GetUserId());
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "خطایی در ذخیره سازی اطلاعات رخ داده است");
                    ViewBag.SectionTypes = new MultiSelectList(unitOfWork.SiteValueRepository.GetIdNameByParentId(sectionTypeParentId), "id", "name");
                    return View(model);
                }
                UTLAlert.Success(this, "با موفقیت ثبت شد");
                return RedirectToAction("Add");
            }
            catch (Exception e)
            {
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                ViewBag.SectionTypes = new MultiSelectList(unitOfWork.SiteValueRepository.GetIdNameByParentId(sectionTypeParentId), "id", "name");
                return View(model);
                //log
            }

        }

        public ActionResult FactorList()
        {

            string myUserId = User.Identity.GetUserId();
            ViewBag.Factors = unitOfWork.FactorRepository.GetAll2(x => x.Customer != null && x.parent_id==null, x => x.OrderByDescending(y => y.dateTime), x => x.Customer);
            ViewBag.WorkFlowJobs = unitOfWork.WorkFlowJobRepository.GetAll2(null, null, x => x.WorkFlow);


            var workFlows = unitOfWork.WorkFlowRepository.GetAll(x => x.isDelete == false).OrderBy(x => x.code)
                .Select(x => new { id = x.id, name = x.title }).ToList();
            ViewBag.WorkFlows = new MultiSelectList(workFlows, "id", "name");

            return View();
        }
      

        [IsAjax]
        public ActionResult GetList(int factorId)
        {
            ViewBag.WorkFlowJobs = unitOfWork.WorkFlowJobRepository.GetAll2(null, null, x => x.WorkFlow);
            ViewBag.FactorId = factorId.ToString();

            var workFlows = unitOfWork.WorkFlowRepository.GetAll(x => x.isDelete == false).OrderBy(x => x.code)
                .Select(x => new { id = x.id, name = x.title }).ToList();
            ViewBag.WorkFlows = new MultiSelectList(workFlows, "id", "name");
            return View();
        }

        #region WorkFlowDetails

        public ActionResult WorkFlowJobList()
        {
            UTLDateTime utlDateTime = new UTLDateTime();
            string userId = User.Identity.GetUserId();
            var records = unitOfWork.WorkFlowJobRepository.GetAll(x => x.toUser_id == userId || x.fromUser_id == userId)
                .Join(unitOfWork.FactorRepository.GetAll(), workFlowJob => workFlowJob.object_id, factor => factor.id.ToString(), (workFlowJob, factor) => new { workFlowJob, factor })
                .GroupBy(x => x.workFlowJob.createDate.Date).Select(
                x => new WorkFlowJobViewModels.WorkFlowJobList()
                {
                    dateTime = utlDateTime.convertToPersianDate(x.Key.ToString()),
                    WorkFlowJobCirculation = x.Select(y => new WorkFlowJobViewModels.WorkFlowJobCirculation()
                    {
                        id = y.workFlowJob.id,
                        createDate = y.workFlowJob.createDate,
                        createDatePersian = utlDateTime.convertToPersianDate(y.workFlowJob.createDate.ToString()),
                        createTimePersian = utlDateTime.convertToPersianTime(y.workFlowJob.createDate.ToString()),
                        fromUserName = UserManager.FindById(y.workFlowJob.fromUser_id).UserName,
                        toUserId = y.workFlowJob.toUser_id,
                        message = y.workFlowJob.message,
                        object_id = y.workFlowJob.object_id,
                        replyMessage = y.workFlowJob.replyMessage,
                        replyDate = y.workFlowJob.replyDate,
                        replyDatePersian = (y.workFlowJob.replyDate == null ? " " : utlDateTime.convertToPersianDateTime(y.workFlowJob.replyDate.ToString())),
                        status = y.workFlowJob.status,
                        WorkFlow = y.workFlowJob.WorkFlow,
                        workFlow_id = y.workFlowJob.workFlow_id,
                        toUserName = UserManager.FindById(y.workFlowJob.toUser_id).UserName,
                        token_id = y.workFlowJob.token_id,
                        ownerId = y.factor.owner_id
                    }).OrderByDescending(xx => xx.createDate).ToList()
                }).OrderByDescending(x => x.dateTime).ToList();

            var result =
                unitOfWork.WorkFlowJobRepository.GetAll(
                    x => x.toUser_id == userId &&
                         x.status == (byte)Enums.WorkFlowStatus.DoNotRead);
            if (result != null)
            {
                foreach (var workFlowJob in result)
                {
                    workFlowJob.status = (byte)Enums.WorkFlowStatus.Read;
                    unitOfWork.WorkFlowJobRepository.Update(workFlowJob);
                }
                unitOfWork.Save();
            }



            ViewBag.Records = records;
            ViewBag.WorkFlows = new MultiSelectList(unitOfWork.WorkFlowRepository.GetAll().Select(x => new { id = x.id, name = x.title }).ToList(), "id", "name");

            return View();
        }
        public ActionResult WorkFlowJobDetails(int id)
        {
            try
            {
                if (id == null)
                {
                    //error
                    //log
                }
                var workFlowJobFind = unitOfWork.WorkFlowJobRepository.GetByID(id);
                int factorId = Int32.Parse(workFlowJobFind.object_id);
                string ownerId = unitOfWork.FactorRepository.GetByID(factorId).owner_id;
                if (workFlowJobFind.status == (byte)Enums.WorkFlowStatus.DoNotRead)
                {
                    workFlowJobFind.status = (byte)Enums.WorkFlowStatus.Read;
                    unitOfWork.Save();
                }

                UTLDateTime utlDateTime = new UTLDateTime();
                string userId = User.Identity.GetUserId();
                ViewBag.WorkCirculation =
                    unitOfWork.WorkFlowJobRepository.GetAll2(x => x.object_id == workFlowJobFind.object_id, null, x => x.WorkFlow)
                        .Join(unitOfWork.FactorRepository.GetAll(), workFlowJob => workFlowJob.object_id, factor => factor.id.ToString(), (workFlowJob, factor) => new { workFlowJob, factor })
                    .GroupBy(x => x.workFlowJob.createDate.Date).Select(
                        x => new WorkFlowJobViewModels.WorkFlowJobList()
                        {
                            dateTime = utlDateTime.convertToPersianDate(x.Key.ToString()),
                            WorkFlowJobCirculation = x.Select(y => new WorkFlowJobViewModels.WorkFlowJobCirculation()
                            {
                                id = y.workFlowJob.id,
                                createDate = y.workFlowJob.createDate,
                                createDatePersian = utlDateTime.convertToPersianDate(y.workFlowJob.createDate.ToString()),
                                createTimePersian = utlDateTime.convertToPersianTime(y.workFlowJob.createDate.ToString()),
                                fromUserName = UserManager.FindById(y.workFlowJob.fromUser_id).UserName,
                                toUserId = y.workFlowJob.toUser_id,
                                message = y.workFlowJob.message,
                                object_id = y.workFlowJob.object_id,
                                replyMessage = y.workFlowJob.replyMessage,
                                replyDate = y.workFlowJob.replyDate,
                                replyDatePersian = (y.workFlowJob.replyDate == null ? " " : utlDateTime.convertToPersianDate(y.workFlowJob.replyDate.ToString())),
                                status = y.workFlowJob.status,
                                WorkFlow = y.workFlowJob.WorkFlow,
                                workFlow_id = y.workFlowJob.workFlow_id,
                                toUserName = UserManager.FindById(y.workFlowJob.toUser_id).UserName,
                                token_id = y.workFlowJob.token_id,
                                ownerId = y.factor.owner_id
                            }).ToList()
                        }).OrderByDescending(x => x.dateTime).ToList();
                ViewBag.WorkFlowJob = new WorkFlowJobViewModels.WorkFlowJobCirculation()
                {
                    id = workFlowJobFind.id,
                    createDate = workFlowJobFind.createDate,
                    createDatePersian = utlDateTime.convertToPersianDate(workFlowJobFind.createDate.ToString()),
                    createTimePersian = utlDateTime.convertToPersianTime(workFlowJobFind.createDate.ToString()),
                    fromUserName = UserManager.FindById(workFlowJobFind.fromUser_id).UserName,
                    message = workFlowJobFind.message,
                    object_id = workFlowJobFind.object_id,
                    status = workFlowJobFind.status,
                    WorkFlow = workFlowJobFind.WorkFlow,
                    workFlow_id = workFlowJobFind.workFlow_id,
                    toUserName = UserManager.FindById(workFlowJobFind.toUser_id).UserName,
                    token_id = workFlowJobFind.token_id,
                    replyMessage = (workFlowJobFind.replyMessage == null ? " " : workFlowJobFind.replyMessage),
                    toUserId = workFlowJobFind.toUser_id,
                    ownerId = ownerId
                };
                ViewBag.Display = UTLSiteValues.GetSectionTypeName(workFlowJobFind.WorkFlow.sectionType_id);
                ViewBag.WorkFlows = new MultiSelectList(unitOfWork.WorkFlowRepository.GetAll().Select(x => new { id = x.id, name = x.title }).ToList(), "id", "name");
            }
            catch (Exception e)
            {

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WorkFlowJobDetails(WorkFlowViewModels.AgreeDenyWorkFlow model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "اطلاعات وارد شده صحیح نمی باشد", "", true);
                return RedirectToAction("WorkFlowJobDetails", "WorkFlow",
                    new { area = "Employee", id = model.workFlowJobId });
            }
            try
            {
                var find = unitOfWork.WorkFlowJobRepository.GetByID(model.workFlowJobId);

                if (model.btn == Enums.WorkFlowStatus.Agree.ToString())
                    unitOfWork.WorkFlowJobRepository.Agree(find, model, User.Identity.GetUserId(), unitOfWork);
                else if (model.btn == Enums.WorkFlowStatus.Deny.ToString())
                    unitOfWork.WorkFlowJobRepository.Deny(find, model, User.Identity.GetUserId(), unitOfWork);

                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره داده ها خطایی رخ داده است");
                    return RedirectToAction("WorkFlowJobDetails", "WorkFlow",
                        new { area = "Employee", id = model.workFlowJobId });
                }
                //UTLAlert.Success(this, $"workFlow {find.WorkFlow.title} با موفقیت ثبت شد");
                return RedirectToAction("WorkFlowJobDetails", "WorkFlow",
                    new { area = "Employee", id = model.workFlowJobId });
            }
            catch (Exception e)
            {
                //log
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return RedirectToAction("WorkFlowJobDetails", "WorkFlow",
                    new { area = "Employee", id = model.workFlowJobId });
            }
        }
        #endregion


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sendjob(WorkFlowViewModels.SendJob model)
        {
            if (!ModelState.IsValid)
            {
                UTLAlert.Danger(this, "مقادیر وارد شده صحیح نمی باشد");
                return FactorList();
            }
            try
            {
                unitOfWork.WorkFlowJobRepository.Insert(model, User.Identity.GetUserId(), unitOfWork.FactorRepository);
                if (!unitOfWork.Save())
                {
                    UTLAlert.Danger(this, "در ذخیره داده ها خطایی رخ داده است");
                    return FactorList();
                }
            }
            catch (Exception e)
            {
                UTLAlert.Danger(this, "خطایی رخ داده است لطفا دوباره تلاش کنید");
                return FactorList();
                //log
            }
            return RedirectToAction("FactorList");
        }

        //this method for search select2
        public JsonResult GetEmployeeAjax(string term)
        {
            try
            {
                IEnumerable<EmployeeViewModels.GetEmployeeForJson> users;
                if (!string.IsNullOrEmpty(term))
                {
                    users = unitOfWork.UserRepository.GetAll(x => x.UserName.Contains(term)).Take(10).ToList()
                        .Select(x => new EmployeeViewModels.GetEmployeeForJson()
                        {
                            id = x.Id,
                            phone = x.PhoneNumber,
                            userName = x.UserName
                        });

                    if (users == null || users.Count() == 0)
                    {
                        users = new List<EmployeeViewModels.GetEmployeeForJson>()
                        {
                            new EmployeeViewModels.GetEmployeeForJson()
                            {
                                userName = "موردی یافت نشد", id = "empty", phone = "empty"
                            }
                        };
                    }
                    return Json(users, JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("خطای سیستم", JsonRequestBehavior.AllowGet);
            }
        }

        [IsAjax]
        public JsonResult GetMaxCodeByAjax()
        {
            float maxCode = unitOfWork.WorkFlowRepository.GetMaxCode();
            return Json(maxCode);
        }
    }
}