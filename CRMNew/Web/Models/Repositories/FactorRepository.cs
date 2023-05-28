using System;
using System.Linq;
using System.Linq.Expressions;
using Web.Areas.Factor.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Models.Repositories
{
    public class FactorRepository : Repository<Factor>
    {
        public FactorRepository(CRM_DbContext _context) : base(_context)
        {
        }
        public bool ExsitCode(int? code)
        {
            try
            {
                if (code.HasValue)
                {
                    Factor factor = this.Get(c => c.code == code);
                    if (factor != null)
                        return false;
                    return true;
                }
                return false;
            }
            catch
            {
                throw new System.Exception("خطای سیستم");
            }

        }
        public bool ExistCode(int? code)
        {
            try
            {
                if (code.HasValue)
                {
                    Factor factor = this.Get(c => c.code == code);
                    if (factor != null)
                        return false;
                    return true;
                }
                return false;
            }
            catch
            {
                throw new System.Exception("خطای سیستم");
            }

        }
        public float GetMaxCode(Expression<Func<Factor, bool>> where = null)
        {
            var result = GetAll(where).ToList();
            float maxCode;
            if (result.Count != 0)
            {
                maxCode = result.Max(x => x.code.Value);
                return maxCode + 1;
            }
            return 1;
        }
        public int GetMaxAddonCode(Expression<Func<Factor, bool>> where = null)
        {
            var result = GetAll(where).ToList();
            int maxCode;
            if (result.Count != 0)
            {
                maxCode = result.Max(x => x.addonCode.Value);
                return maxCode + 1;
            }
            return 1;
        }
        public bool ExistAddonCode(int factorId, int addonCode)
        {
            try
            {
                var factors = Where(x => x.parent_id == factorId);
                var find = factors.Where(x => x.addonCode == addonCode).ToList();
                if (find != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                throw new System.Exception("خطای سیستم");
            }

        }

        public int Insert(FactorViewModels.Add model, string user_id,IUnitOfWork unitOfWork)
        {
            //set status
            Factor factor = new Factor()
            {
                priceTotalItem = model.priceTotalItem,
                priceTotalFactor = model.priceTotalItem,
                priceCredit = 0,
                user_id = user_id,
            };
            Insert(factor);
            unitOfWork.Save();
            return factor.id;
        }
        public void Insert(FactorViewModels.Save model, string user_id, FactorCostRepository FCR,IUnitOfWork unitOfWork)
        {
            UTLDateTime utlDateTime = new UTLDateTime();
            var find = GetByID(model.factorId);
            find.code = model.code;
            find.customer_id = model.customerId;
            find.presentation = model.presentationId;
            find.dateTime = utlDateTime.ConvertToDateTime(model.date);
            find.description = model.factorDescription;
            find.expair = utlDateTime.convertToPersianDate(DateTime.Now.AddDays(double.Parse(model.expireDate.ToString())).ToString());
            find.isRasmi = model.isRasmi;
            find.owner_id = user_id;
            find.paymentDescription = model.paymentDescription;
            find.person = model.person;
            
            
            find.priceTotalFactor = model.priceTotalFactor;
            find.placeOfDelivery = model.deliveryPlace;
            find.priceCredit = model.expireDate;
            if (model.paymentTypeId!=0)
            {
                find.paymentType = model.paymentTypeId;
            }
            if (model.statusId != 0)
            {
                find.status = model.statusId;
            }
            if (model.addonCode.HasValue)
            {
                find.addonCode = model.addonCode;
                find.parent_id = model.parentId;
            }
            if (model.addFactorCost!=null)
            {
                foreach (var item in model.addFactorCost)
                {
                    FCR.Insert(item);
                    unitOfWork.Save();
                }
            }
            if (model.factorCostAfters != null)
            {
                foreach (var item in model.factorCostAfters)
                {
                        unitOfWork.FactorItemCostRepository.Insert(item);
                        unitOfWork.Save();
                }
            }

            Update(find);
        }
        public void InsertAddon(FactorViewModels.Edit model, string user_id, FactorCostRepository FCR, IUnitOfWork unitOfWork)
        {
            UTLDateTime utlDateTime = new UTLDateTime();
            var find = GetByID(model.factorId);
            find.code = model.code;
            find.customer_id = model.customerId;
            find.presentation = model.presentationId;
            find.dateTime = utlDateTime.ConvertToDateTime(model.date);
            find.description = model.factorDescription;
            find.expair = utlDateTime.convertToPersianDate(DateTime.Now.AddDays(double.Parse(model.expireDate.ToString())).ToString());
            find.isRasmi = model.isRasmi;
            find.owner_id = user_id;
            find.paymentDescription = model.paymentDescription;
            find.person = model.person;


            find.priceTotalFactor = model.priceTotalFactor;
            find.placeOfDelivery = model.deliveryPlace;
            find.priceCredit = model.expireDate;
            if (model.paymentTypeId != 0)
            {
                find.paymentType = model.paymentTypeId;
            }
            if (model.statusId != 0)
            {
                find.status = model.statusId;
            }
            if (model.addonCode.HasValue)
            {
                find.addonCode = model.addonCode;
                find.parent_id = model.parentId;
            }
            if (model.addFactorCost != null)
            {
                foreach (var item in model.addFactorCost)
                {
                    FCR.Insert(item);
                    unitOfWork.Save();
                }
            }
            if (model.factorCostAfter != null)
            {
                foreach (var item in model.factorCostAfter)
                {
                    unitOfWork.FactorItemCostRepository.Insert(item);
                    unitOfWork.Save();
                }
            }

            Update(find);
        }
        public void Edit(FactorViewModels.Edit model, string user_id, FactorItemRepository FIR, FactorItemCostRepository FICR,
            FactorCostRepository FCR, FactorOptionValueRepository FOVR)
        {
            UTLDateTime utlDateTime = new UTLDateTime();
            var find = GetByID(model.factorId);
            find.code = model.code;
            find.customer_id = model.customerId;
            find.presentation = model.presentationId;
            find.dateTime = utlDateTime.ConvertToDateTime(model.date);
            find.description = model.factorDescription;
            find.isRasmi = model.isRasmi;
            find.owner_id = user_id;
            find.paymentDescription = model.paymentDescription;
            find.person = model.person;
            find.status = model.statusId;
            find.paymentType = model.paymentTypeId;
            find.priceTotalFactor = model.priceTotalFactor;
            find.placeOfDelivery = model.deliveryPlace;
            find.priceCredit = model.expireDate;
            //FOVR.Edit(model);

            //foreach (var factorItem in model.FactorItem)
            //{
            //    if (factorItem.factorItemId.HasValue)
            //    {
            //        var factorItemFind = find.FactorItems.Where(x => x.id == factorItem.factorItemId).FirstOrDefault();
            //        if (factorItemFind != null)
            //        {
            //            FIR.Edit(factorItem);
            //            FICR.Edit(factorItem);
            //        }
            //    }
            //    else
            //    {
            //        var factorItemId= FIR.Insert(factorItem);
            //        factorItem.factorItemId = factorItemId;
            //        FICR.Insert(factorItem);
            //    }
            //}


            foreach (var item in model.addFactorCost)
            {
                FCR.Edit(item);
            }

            //Update(find);
        }
    }
}