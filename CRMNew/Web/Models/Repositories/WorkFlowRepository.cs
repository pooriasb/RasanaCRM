using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class WorkFlowRepository:Repository<WorkFlow>
    {
        public WorkFlowRepository(CRM_DbContext _context) : base(_context)
        {
        }

        public void Insert(WorkFlowViewModels.Add model, string userId)
        {
            WorkFlow workFlow = new WorkFlow()
            {
                title = model.title.Trim(),
                sectionType_id = model.id,
                user_id = userId,
                code = model.code
            };
            Insert(workFlow);
        }
        public float GetMaxCode(Expression<Func<WorkFlow, bool>> where = null)
        {
            var result = GetAll(where).ToList();
            float maxCode;
            if (result.Count != 0)
            {
                maxCode = result.Max(x => x.code);
                return maxCode + 1;
            }
            return 1;
        }
    }
}