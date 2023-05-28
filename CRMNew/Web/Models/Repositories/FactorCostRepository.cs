using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Areas.Factor.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class FactorCostRepository:Repository<FactorCost>
    {
        public FactorCostRepository(CRM_DbContext db):base(db)
        {

        }

        public void Insert(FactorCostViewModels.Add model)
        {
            FactorCost factorCost=new FactorCost()
            {
                factor_id = model.factorId,
                factorCostSet_id = model.factorCostId,
                value = model.value
            };
            Insert(factorCost);
        }
        public void Edit(FactorCostViewModels.Add model)
        {
            var find = Get(x=>x.factorCostSet_id==model.factorCostId && x.factor_id==model.factorId);
            if (find!=null)
            {
                find.value = model.value;
            }
        }
    }
}