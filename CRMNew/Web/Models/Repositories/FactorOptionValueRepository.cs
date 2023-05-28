using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Areas.Factor.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class FactorOptionValueRepository : Repository<FactorOptionValue>
    {
        public FactorOptionValueRepository(CRM_DbContext _context) : base(_context)
        {
        }

        public void Insert(FactorViewModels.Save model)
        {
            List<FactorOptionValue> list = new List<FactorOptionValue>();
            foreach (var item in model.addDynamics)
            {
                list.Add(new FactorOptionValue()
                {
                    factor_id = model.factorId,
                    option_id = item.optionId,
                    value_id = item.valueId,
                    strValue = (item.strValue == null ? null : item.strValue),
                    type = item.type,
                });
            }
            Insert(list);
        }
        public void Edit(FactorViewModels.Edit model)
        {
            List<FactorOptionValue> list = new List<FactorOptionValue>();
            foreach (var item in model.dynamicsEdit)
            {
                if (item.id.HasValue)
                {
                    var find = GetByID(item.id);
                    if (find != null)
                    {
                        find.factor_id = model.factorId;
                        find.option_id = item.optionId;
                        find.value_id = item.valueId;
                        find.strValue = (item.strValue == null ? null : item.strValue);
                        find.type = item.type;
                        Update(find);
                    }
                }
                else
                {
                    list.Add(new FactorOptionValue()
                    {
                        factor_id = model.factorId,
                        option_id = item.optionId,
                        value_id = item.valueId,
                        strValue = (item.strValue == null ? null : item.strValue),
                        type = item.type,
                    });
                }
            }
            Insert(list);
        }
    }
}