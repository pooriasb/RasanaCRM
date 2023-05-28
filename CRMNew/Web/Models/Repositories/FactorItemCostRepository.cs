using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Areas.Factor.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class FactorItemCostRepository:Repository<FactorItemCost>
    {
        public FactorItemCostRepository(CRM_DbContext db):base(db)
        {

        }

        public void Insert(FactorViewModels.Save model)
        {
            List<FactorItemCost> list = new List<FactorItemCost>(); 
            foreach (var item in model.factorCostAfters)
            {
                foreach (var factorItemCostAfter in item.ItemCostAfters)
                {
                    list.AddRange(new List<FactorItemCost>()
                    {
                        new FactorItemCost()
                        {
                            FactorCostSet_id = factorItemCostAfter.costId,
                            factorItem_id = model.factorId,
                            value = factorItemCostAfter.value,
                        }
                    });
                }
            }
            Insert(list);
        }
        public void Edit(FactorViewModels.Edit model)
        {
            foreach (var item in model.factorCostAfter)
            {
                foreach (var item2 in item.ItemCostAfters)
                {
                    var find = GetByID(item2.id);
                    find.value = item2.value;
                    Update(find);
                }
            }
        }
        public void Insert(FactorViewModels.Add model)
        {
            List<FactorItemCost> list = new List<FactorItemCost>();
            foreach (var item in model.FactorItemCosts)
            {
                list.AddRange(new List<FactorItemCost>()
                {
                    new FactorItemCost()
                    {
                        FactorCostSet_id = item.id,
                        factorItem_id = model.factorItemId,
                        value = item.value,
                    }
                });
            }
            Insert(list);
        }
        public void Insert(FactorItemCostViewModels.FactorItemCostAfters model)
        {
            List<FactorItemCost> list = new List<FactorItemCost>();
            foreach (var item in model.ItemCostAfters)
            {
                list.AddRange(new List<FactorItemCost>()
                {
                    new FactorItemCost()
                    {
                        FactorCostSet_id = item.costId,
                        factorItem_id = item.factorItemId,
                        value = item.value,
                    }
                });
            }
            Insert(list);
        }
        public void Insert(FactorItemViewModels.Edit model)
        {
            List<FactorItemCost> list = new List<FactorItemCost>();
            foreach (var item in model.FactorItemCosts)
            {
                list.AddRange(new List<FactorItemCost>()
                {
                    new FactorItemCost()
                    {
                        FactorCostSet_id = item.id,
                        factorItem_id = model.factorItemId,
                        value = item.value,
                    }
                });
            }
            Insert(list);
        }

        public void Edit(FactorItemViewModels.Edit model)
        {
            foreach (var itemCost in model.FactorItemCosts)
            {
                var find= GetByID(itemCost.id);
                find.value = itemCost.value;
                Update(find);
            }
        }
        
    }
}