using System;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class CRMLogsRepository:Repository<CRMLog>
    {
        public CRMLogsRepository(CRM_DbContext _context) : base(_context)
        {
            
        }
        public void Insert(int type, int document, string action, string enterdData, string resultData,
            string userId, string description,string ip)
        {
            CRMLog log=new CRMLog()
            {
                action = action,
                description = description,
                enteredData = enterdData,
                ip = ip,
                logDate = DateTime.Now,
                logDocument = document,
                logType = type,
                id = Guid.NewGuid().ToString(),
                resultData = resultData,
                user_id = userId,
                
            };
            Insert(log);
        }
    }
}