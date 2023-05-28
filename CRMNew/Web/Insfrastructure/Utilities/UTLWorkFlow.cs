using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;
using Web.ViewModels.Enums;

namespace Web.Insfrastructure.Utilities
{
    public class UTLWorkFlow
    {
        //public static string GetNameStatus(byte code)
        //{
        //    switch (enums)
        //    {
        //            case Enums.WorkFlowStatus.Read:return"خواننده شده";
        //    }
        //}
        private readonly IUnitOfWork unitOfWork;

        public UTLWorkFlow()
        {
            this.unitOfWork = new UnitOfWork.UnitOfWork();
        }

        public IEnumerable<WorkFlowJob> GetMessages(string userId)
        {
            return unitOfWork.WorkFlowJobRepository.GetAll(x => x.toUser_id == userId && x.status==(int)Enums.WorkFlowStatus.DoNotRead);
        }

        public string GetUserNameById(string userId)
        {
            return unitOfWork.UserRepository.GetByID(userId).UserName;
        }
    }
}