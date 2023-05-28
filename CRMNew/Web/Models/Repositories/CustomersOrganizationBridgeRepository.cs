using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class CustomersOrganizationBridgeRepository:Repository<CustomersOrganizationsBridge>
    {
        public CustomersOrganizationBridgeRepository(CRM_DbContext _context) : base(_context)
        {
        }
    }
}