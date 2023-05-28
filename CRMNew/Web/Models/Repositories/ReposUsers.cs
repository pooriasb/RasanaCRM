using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Models.Repositories
{
    public class ReposUsers:Repository<User>
    {
        public ReposUsers(CRM_DbContext dbContext):base(dbContext)
        {

        }
       
    }
}