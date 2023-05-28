using System.Collections.Generic;
using System.Linq;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models.Entity;
using Web.ViewModels.Enums;

namespace Web.Models.Repositories
{
    public class ProductOptionValueRepository:Repository<ProductOptionValue>
    {
        public ProductOptionValueRepository(CRM_DbContext _context) : base(_context)
        {
        }

        public IEnumerable<ProductOptionValueViewModels.GetHeaderOption> GetOptionValue(Enums.SiteValue enums)
        {
            int parentId = UTLSiteValues.GetSiteValueId(enums);
            var result = (from pov in GetAll()
                join product in db.Products on pov.product_id equals product.id
                join siteValue in db.SiteValues.Where(x => x.parentId == parentId).ToList() on pov.option_id equals
                siteValue.id
                select new ProductOptionValueViewModels.GetHeaderOption()
                {
                    id = pov.id,
                    productId = pov.product_id,
                    productName = product.title,
                    optionId = pov.option_id,
                    optionValue = siteValue.value,
                    optionName = siteValue.name
                }).ToList();
            return result;
        }

    }
}