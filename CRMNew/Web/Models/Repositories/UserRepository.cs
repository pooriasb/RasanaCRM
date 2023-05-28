using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class UserRepository:Repository<User>
    {
        public UserRepository(CRM_DbContext _context) : base(_context)
        {
        }
    }
}