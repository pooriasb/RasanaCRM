using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class ProductPriceRepository : Repository<ProductPrice>
    {
        public ProductPriceRepository(CRM_DbContext _context) : base(_context)
        {
        }

        public void Edit(ProductPriceViewModels.Edit model)
        {
            var find = GetByID(model.id);
            find.price = model.price;
        }

        public void Insert(ProductPriceViewModels.AddSingle model, ProductPriceLogRepository PPLR)
        {
            ProductPrice productPrice = new ProductPrice()
            {
                product_id = model.id,
                price = model.price,
                vahed_id = model.vahedId,
            };
            
            Insert(productPrice);
            PPLR.Insert(new ProductPriceLog() { date = DateTime.Now, price = model.price, product_id = model.id, vahed_id = model.vahedId });
        }
    }

}