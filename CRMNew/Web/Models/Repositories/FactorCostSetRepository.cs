using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class FactorCostSetRepository:Repository<FactorCostSet>
    {
        public FactorCostSetRepository(CRM_DbContext _context) : base(_context)
        {
        }

        public void Insert(FactorCostSetViewModels.Add model)
        {
            FactorCostSet factorCostSet=new FactorCostSet()
            {
                description = model.description,
                isEnable = model.isEnable,
                isInCrease = model.isIncrese,
                isInFee = model.isInFee,
                isInItem = model.isInItem,
                isPresent = model.isPresent,
                name = model.name.Trim(),
                isShowCustomer = model.isShowCustomer
            };
            Insert(factorCostSet);
        }

        public void Edit(FactorCostSetViewModels.Edit model)
        {
            var find = GetByID(model.id);
            find.isEnable = model.isEnable;
            find.isInCrease = model.isIncrese;
            find.isInFee = model.isInFee;
            find.isInItem = model.isInItem;
            find.isPresent = model.isPresent;
            find.isShowCustomer = model.isShowCustomer;
            find.name = model.name;
            find.description = model.description;
        }
    }
}