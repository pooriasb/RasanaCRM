using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Insfrastructure.Utilities.DataTable;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(CRM_DbContext _context) : base(_context)
        {
        }

        public void Insert(ProductViewModels.Add model, string userId)
        {
            try
            {
                Product product = new Product()
                {
                    category_id = model.categoryId,
                    code = model.code,
                    createDate = DateTime.Now,
                    description = model.description,
                    isEnable = model.isEnable,
                    title = model.title,
                    user_id = userId,
                    lastUpdateDate = DateTime.Now,
                    lastUpdateUser_id = userId
                };

                List<ProductPrice> productPrices = new List<ProductPrice>();
                List<ProductPriceLog> productPricesLog = new List<ProductPriceLog>();
                foreach (var productPriceItem in model.productPrices)
                {
                    if (productPriceItem.isDelete=="0")
                    {
                        productPriceItem.price=Regex.Replace(productPriceItem.price, ",", "");
                        productPrices.Add(new ProductPrice() { vahed_id = productPriceItem.vahedId, price = productPriceItem.price, product_id = product.id });
                        productPricesLog.Add(new ProductPriceLog() { vahed_id = productPriceItem.vahedId, price = productPriceItem.price, product_id = product.id, date = DateTime.Now });
                    }
                }

                product.ProductPrices = productPrices;
                //product price log
                product.ProductPriceLogs = productPricesLog;

                if (model.productDynamic!=null)
                {
                    if (model.productDynamic.Count != 0)
                    {
                        foreach (var dynamicse in model.productDynamic)
                        {
                            product.ProductOptionValues.Add(new ProductOptionValue() { product_id = product.id, value_id = dynamicse.selectTagName, option_id = dynamicse.selectTagId });
                        }
                    }
                }
                if (model.productDynamicString!=null)
                {
                    if (model.productDynamicString.Count != 0)
                    {
                        foreach (var dynamicse in model.productDynamicString)
                        {
                            product.ProductOptionValues.Add(new ProductOptionValue() { product_id = product.id, option_id = dynamicse.textBoxId, strValue = dynamicse.textBoxValue });
                        }
                    }
                }
                Insert(product);
                //UTLLog log = new UTLLog();
                //log.AddLog(0, 0, "C", Json.Encode(product),"", userId, "","ip");
            }
            catch (Exception e)
            {
                //log
            }
        }

        public void Edit(ProductViewModels.Edit model, string userId,IUnitOfWork unitOfWork)
        {
            try
            {
                var find = GetByID(model.id);
                if (find.category_id!=model.categoryId)
                {
                    find.category_id = model.categoryId;
                    foreach (var productOptionValue in find.ProductOptionValues.ToList())
                    {
                        unitOfWork.ProductOptionValueRepository.Delete(productOptionValue.id);
                    }
                }
                
                find.description = model.description;
                find.isEnable = model.isEnable;
                find.title = model.title;
                find.lastUpdateDate = DateTime.Now;
                find.lastUpdateUser_id = userId;
                find.code = model.code;
                //price
                foreach (var pp in model.productPrices)
                {
                    pp.price = Regex.Replace(pp.price, ",", "");
                    if (pp.productPriceId==0)
                    {
                        unitOfWork.ProductPriceRepository.Insert(new ProductPrice(){price = pp.price,vahed_id = pp.vahedId,product_id = model.id});
                        unitOfWork.ProductPriceLogRepository.Insert(new ProductPriceLog(){ price = pp.price, vahed_id = pp.vahedId, product_id = model.id ,date = DateTime.Now});
                    }
                    else
                    {
                        var priceFind = unitOfWork.ProductPriceRepository.GetByID(pp.productPriceId);
                        priceFind.price = pp.price;
                    }
                }
                //Dynamic
                if (model.productDynamic != null)
                {
                    if (model.productDynamic.Count != 0)
                    {
                        foreach (var dynamicse in model.productDynamic)
                        {
                            //if (dynamicse.)
                            //{
                                
                            //}
                            unitOfWork.ProductOptionValueRepository.Insert(new ProductOptionValue() { product_id = model.id, value_id = dynamicse.selectTagName, option_id = dynamicse.selectTagId });
                        }
                    }
                }
                //string
                if (model.productDynamicString != null)
                {
                    if (model.productDynamicString.Count != 0)
                    {
                        foreach (var dynamicse in model.productDynamicString)
                        {
                            unitOfWork.ProductOptionValueRepository.Insert(new ProductOptionValue() { product_id = model.id, option_id = dynamicse.textBoxId, strValue = dynamicse.textBoxValue });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //log
            }
        }

        public void DeletePhysical(int id)
        {
            var find = GetByID(id);
            find.isDalete = true;
        }

        public float GetMaxCode(Expression<Func<Product, bool>> where = null)
        {
            var result = GetAll(where).ToList();
            float maxCode;
            if (result.Count != 0)
            {
                maxCode = result.Max(x => x.code);
                return maxCode + 1;
            }
            return 1;
        }

        
    }
}