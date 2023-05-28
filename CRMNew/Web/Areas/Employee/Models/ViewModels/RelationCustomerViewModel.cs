using System.Collections.Generic;
using System.Web.Mvc;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class RelationCustomerViewModel
    {
        CustomerRelation customerRelation;
        public RelationCustomerViewModel(IUnitOfWork unitOfWork,int? customer_id,CustomerRelation customerRelation=null)
        {
            if(customerRelation==null)
            {
                this.customerRelation = new CustomerRelation();
                this.customerRelation.customer_id = customer_id;
            }else
            {
                this.customerRelation = customerRelation;
            }
         
        }
    
        public CustomerRelation CustomerRelation { get {
                return customerRelation;
            }  }
    }
}