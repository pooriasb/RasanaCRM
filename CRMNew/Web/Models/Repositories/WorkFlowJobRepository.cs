using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;
using Web.ViewModels.Enums;

namespace Web.Models.Repositories
{
    public class WorkFlowJobRepository:Repository<WorkFlowJob>
    {
        public WorkFlowJobRepository(CRM_DbContext _context) : base(_context)
        {
        }

        public void Insert(WorkFlowViewModels.SendJob model,string fromUserId,FactorRepository FR)
        {
            WorkFlowJob workFlowJob = new WorkFlowJob
            {
                createDate = DateTime.Now,
                fromUser_id = fromUserId,
                message = model.message,
                toUser_id = model.userId,
                status = (byte) Enums.WorkFlowStatus.DoNotRead,
                object_id = model.factorId.ToString(),
                workFlow_id = model.id
            };
            var factorFind= FR.GetByID(model.factorId);
            factorFind.owner_id = model.userId;
            FR.Update(factorFind);
            Insert(workFlowJob);
        }

        public void Insert(string fromUser, string message, string toUserId, string objectId, int workFlowId)
        {
            WorkFlowJob workFlowJob = new WorkFlowJob
            {
                createDate = DateTime.Now,
                fromUser_id = fromUser,
                message = message,
                toUser_id = toUserId,
                status = (byte)Enums.WorkFlowStatus.DoNotRead,
                object_id = objectId,
                workFlow_id = workFlowId
            };
            Insert(workFlowJob);
        }
        public void Agree(WorkFlowJob find, WorkFlowViewModels.AgreeDenyWorkFlow model, string userId, IUnitOfWork unitOfWork)
        {
            find.replyDate = DateTime.Now;
            find.status = (byte)Enums.WorkFlowStatus.Agree;
            find.replyMessage = model.replyMessage;
            try
            {
                int factor_id = int.Parse(find.object_id);
                var factorFind = unitOfWork.FactorRepository.Get(x => x.id == factor_id);
                factorFind.owner_id = userId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Deny(WorkFlowJob find, WorkFlowViewModels.AgreeDenyWorkFlow model, string userId, IUnitOfWork unitOfWork)
        {
            find.replyDate = DateTime.Now;
            find.status = (byte)Enums.WorkFlowStatus.Deny;
            find.replyMessage = model.replyMessage;
            try
            {
                int factor_id = int.Parse(find.object_id);
                var factorFind = unitOfWork.FactorRepository.Get(x => x.id == factor_id);
                factorFind.owner_id = userId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}