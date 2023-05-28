using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Areas.Factor.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class FactorItemRepository:Repository<FactorItem>
    {
        public FactorItemRepository(CRM_DbContext db):base(db)
        {

        }
        public override void Delete(object id)
        {
            try
            {
                FactorItem factorItem = Get(c => c.id == (int)id);
                if (factorItem != null && factorItem.FactorItemCosts != null)
                    db.FactorItemCosts.RemoveRange(factorItem.FactorItemCosts);


                    factorItem.Factor.priceTotalFactor -= factorItem.price;
                    factorItem.Factor.priceTotalItem -= factorItem.price;

                base.Delete(id);
            }
            catch
            {
                throw new Exception("خطا سیستم");
            }
        }

        public int Insert(FactorViewModels.Add model,IUnitOfWork unitOfWork)
        {
            FactorItem factorItem=new FactorItem()
            {
                count = model.count,
                description = model.description,
                garanty = (byte)model.garanty,
                warranty = (byte)model.warranty,
                product_id = model.productId,
                price_id = model.priceId,
                factor_id = model.factorId,
                price = model.price,
                total = model.priceTotalItem,
            };
            Insert(factorItem);
            unitOfWork.Save();
            return factorItem.id;
        }

        public void Edit(FactorItemViewModels.Edit model)
        {
            var find= GetByID(model.factorItemId);
            find.count = model.count;
            find.garanty = (byte)model.garanty;
            find.warranty = (byte)model.warranty;
            find.description = model.description;
            find.price = model.price;
            find.price_id = model.priceId;
            find.product_id = model.productId;
            find.total = model.priceTotalItem;
            Update(find);
        }
        public int Insert(FactorItemViewModels.Edit model)
        {
            FactorItem factorItem = new FactorItem()
            {
                count = model.count,
                description = model.description,
                garanty = (byte)model.garanty,
                warranty = (byte)model.warranty,
                product_id = model.productId,
                price_id = model.priceId,
                factor_id = model.factorId,
                price = model.price,
                total = model.priceTotalItem,
            };
            Insert(factorItem);
            return factorItem.id;
        }
    }
}