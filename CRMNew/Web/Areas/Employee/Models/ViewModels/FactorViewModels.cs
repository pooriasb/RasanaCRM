using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class FactorViewModels
    {
        private Web.Models.Entity.Factor _factor;
        IEnumerable<FactorCostSet> factorCosts;
        IEnumerable<FactorCostSet> factorCostBase;
        IUnitOfWork unitOfWork;
        int? maxCode;
        public FactorViewModels(IUnitOfWork unitOfWork,int? id=null)
        {
            unitOfWork = unitOfWork ?? new UnitOfWork();
            try
            {
                maxCode = unitOfWork.FactorRepository.Entity().Max(c => c.code);
                if (maxCode != null)
                    maxCode++;
                else
                    maxCode = 1;
              
            }
            catch
            {
                maxCode = 1;
 
            }
            try
            {
                factorCosts = unitOfWork.FactorCostSetRepository.GetAll(c => c.isEnable == true && c.isInFee == false&&c.isInItem==true);
                factorCostBase = unitOfWork.FactorCostSetRepository.GetAll(c => c.isEnable == true&&c.isInItem==false);
            }
            catch
            {
               // throw new Exception("خطای سیستمی");
            }
            _factor = new Web.Models.Entity.Factor() { code = maxCode,dateTime=DateTime.Now };
            if(id.HasValue)
            _factor = unitOfWork.FactorRepository.Get(c => c.id == id);
            if(factor.expireDate.HasValue)
            {
                factor.expair = (factor.expireDate.Value.DayOfYear - DateTime.Now.DayOfYear).ToString();
            }
        }
        public Web.Models.Entity.Factor factor { get { return _factor; }  }
      
        public IEnumerable<FactorCostSet> FactorCost { get { return factorCosts ?? new List<FactorCostSet>(); } }
        public IEnumerable<FactorCostSet> FactorCostBase { get { return factorCostBase ?? new List<FactorCostSet>(); } }
    }
    public class FactorProductViewModel
    {
        IUnitOfWork unitOfWork;
        IEnumerable<FactorCostSet> factorCosts;
    
        public FactorItem factorItem{ get; set; }
        public IEnumerable<FactorCostSet> FactorCost { get { return factorCosts??new List<FactorCostSet>(); } }
        public FactorProductViewModel(IUnitOfWork unitOfWork,int? id=null)
        {
            this.unitOfWork = unitOfWork ?? new UnitOfWork();
            try
            {
                factorCosts = unitOfWork.FactorCostSetRepository.GetAll(c => c.isEnable == true &&c.isInItem==true);
                if (id.HasValue)
                    factorItem = unitOfWork.FactorItemRepository.Get(c => c.id == id);
                else
                    factorItem = new FactorItem();
              }
            catch
            {
                throw new Exception("خطای سیستمی");
            }

        
        }
    }

    public class FactorViewModelResult
    {
        public IEnumerable<FactorItemCost> factorItemCosts { get; set; }
        public IEnumerable<FactorCost> factorCost { get; set; }
        public List<FactorProductsItem> productItems { get; set; }
        public Web.Models.Entity.Factor Factor { get; set; }
        public List<FactorItem> FactorItems { get; set; }
    }
    public class FactorProductsItem
    {
        public Product product { get; set; }
        public ProductPrice productPrice { get; set; }
    }
}