using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;
using Web.ViewModels.Enums;


namespace Web.Models.Repositories
{
    public class CustomerRepository:Repository<Customer>
    {
        public CustomerRepository(CRM_DbContext dbContext):base(dbContext)
        {

        }

        public void Insert(CustomerViewModels.Add model)
        {
            Customer customer=new Customer()
            {
                address = model.address,
                address2 = model.address2,
                address1 = model.address1,
                dateTime = DateTime.Now,
                city_id = model.city_id,
                province_id = model.province_id,
                code = model.code,
                economicCode = model.economicCode,
                name = model.name,
                nationalCode = model.nationalCode,
                phone = model.phone,
                postCode = model.postCode,
                tell = model.tell1,
                tell1 = model.tell1,
                tell2 = model.tell2,
                fax = model.fax,
                accountNumber = model.accountNumber,
                description = model.description,
            };
            List<CustomersCategoriesBridge> customersCategoriesBridges=new List<CustomersCategoriesBridge>();
            List<CustomersOrganizationsBridge> customerOrganizationsBridges = new List<CustomersOrganizationsBridge>();
            foreach (var i in model.customerCategory_id)
            {
                customersCategoriesBridges.Add(new CustomersCategoriesBridge()
                {
                    category_id = i,
                    customer_id = customer.id,
                });
            }
            foreach (var i in model.organizationUnit_id)
            {
                customerOrganizationsBridges.Add(new CustomersOrganizationsBridge()
                {
                    customer_id = customer.id,
                    organization_id = i,
                });
            }
            customer.CustomersCategoriesBridges = customersCategoriesBridges;
            customer.CustomersOrganizationsBridges = customerOrganizationsBridges;

            //categoryDynamic
            if (model.customerCategoryDynamic != null)
            {
                if (model.customerCategoryDynamic.Count != 0)
                {
                    foreach (var dynamicse in model.customerCategoryDynamic)
                    {
                        customer.CustomersOptionValues.Add(new CustomersOptionValue() { customer_id = customer.id, value_id = dynamicse.selectTagName, option_id = dynamicse.selectTagId, type = Enums.OptionType.DropDown.ToString() });
                    }
                }
            }
            if (model.customerCategoryDynamicString != null)
            {
                if (model.customerCategoryDynamicString.Count != 0)
                {
                    foreach (var dynamicse in model.customerCategoryDynamicString)
                    {
                        if (dynamicse.textBoxValue!=null)
                        {
                            customer.CustomersOptionValues.Add(new CustomersOptionValue() { customer_id = customer.id, option_id = dynamicse.textBoxId, strValue = dynamicse.textBoxValue, type = Enums.OptionType.String.ToString() });
                        }
                        
                    }
                }
            }

            //organizationDynamic
            if (model.customerOrganizationDynamic != null)
            {
                if (model.customerOrganizationDynamic.Count != 0)
                {
                    foreach (var dynamicse in model.customerOrganizationDynamic)
                    {
                        customer.CustomersOptionValues.Add(new CustomersOptionValue() { customer_id = customer.id, value_id = dynamicse.selectTagName, option_id = dynamicse.selectTagId, type = Enums.OptionType.DropDown.ToString() });
                    }
                }
            }
            if (model.customerOrganizationDynamicString != null)
            {
                if (model.customerOrganizationDynamicString.Count != 0)
                {
                    foreach (var dynamicse in model.customerOrganizationDynamicString)
                    {
                        if (dynamicse.textBoxValue!=null)
                        {
                            customer.CustomersOptionValues.Add(new CustomersOptionValue() { customer_id = customer.id, option_id = dynamicse.textBoxId, strValue = dynamicse.textBoxValue, type = Enums.OptionType.String.ToString() });
                        }
                        
                    }
                }
            }






            Insert(customer);
        }

        public void Edit(CustomerViewModels.Edit model,CustomerOptionValueRepository covr)
        {
            var find= GetByID(model.id);
            find.address = model.address;
            find.address2 = model.address2;
            find.address1 = model.address1;
            find.city_id = model.city_id;
            find.province_id = model.province_id;
            find.code = model.code;
            find.economicCode = model.economicCode;
            find.name = model.name;
            find.nationalCode = model.nationalCode;
            find.phone = model.phone;
            find.postCode = model.postCode;
            find.tell = model.tell1;
            find.tell1 = model.tell1;
            find.tell2 = model.tell2;
            find.fax = model.fax;
            find.accountNumber = model.accountNumber;
            find.description = model.description;


            foreach (var i in model.customerCategory_id)
            {
                var ccbFind= find.CustomersCategoriesBridges.Where(x => x.category_id == i).FirstOrDefault();
                if (ccbFind==null)
                {
                    find.CustomersCategoriesBridges.Add(new CustomersCategoriesBridge()
                    {
                        category_id = i,
                        customer_id = find.id,
                    });
                }
            }

            foreach (var i in model.organizationUnit_id)
            {
                var cobFind = find.CustomersOrganizationsBridges.Where(x => x.organization_id == i).FirstOrDefault();
                if (cobFind == null)
                {
                    find.CustomersOrganizationsBridges.Add(new CustomersOrganizationsBridge()
                    {
                        customer_id = find.id,
                        organization_id = i,
                    });
                }
            }

            //categoryDynamic
            if (model.customerCategoryDynamic != null)
            {
                if (model.customerCategoryDynamic.Count != 0)
                {
                    foreach (var dynamicse in model.customerCategoryDynamic)
                    {
                        find.CustomersOptionValues.Add(new CustomersOptionValue() { customer_id = find.id, value_id = dynamicse.selectTagName, option_id = dynamicse.selectTagId, type = Enums.OptionType.DropDown.ToString() });
                    }
                }
            }
            if (model.customerCategoryDynamicString != null)
            {
                if (model.customerCategoryDynamicString.Count != 0)
                {
                    foreach (var dynamicse in model.customerCategoryDynamicString)
                    {
                        if (dynamicse.textBoxValue!=null)
                        {
                            find.CustomersOptionValues.Add(new CustomersOptionValue() { customer_id = find.id, option_id = dynamicse.textBoxId, strValue = dynamicse.textBoxValue, type = Enums.OptionType.String.ToString() });
                        }

                    }
                }
            }

            //organizationDynamic
            if (model.customerOrganizationDynamic != null)
            {
                if (model.customerOrganizationDynamic.Count != 0)
                {
                    foreach (var dynamicse in model.customerOrganizationDynamic)
                    {
                        find.CustomersOptionValues.Add(new CustomersOptionValue() { customer_id = find.id, value_id = dynamicse.selectTagName, option_id = dynamicse.selectTagId, type = Enums.OptionType.DropDown.ToString() });
                    }
                }
            }
            if (model.customerOrganizationDynamicString != null)
            {
                if (model.customerOrganizationDynamicString.Count != 0)
                {
                    foreach (var dynamicse in model.customerOrganizationDynamicString)
                    {
                        if (dynamicse.textBoxValue!=null)
                        {
                            find.CustomersOptionValues.Add(new CustomersOptionValue() { customer_id = find.id, option_id = dynamicse.textBoxId, strValue = dynamicse.textBoxValue, type = Enums.OptionType.String.ToString() });
                        }
                        
                    }
                }
            }

            foreach (var item in model.CustomersOptionValues)
            {
                var covfind = covr.GetByID(item.id);
                if (covfind!=null)
                {
                    if (covfind.value_id!=null)
                    {
                        covfind.value_id = item.value_id;
                    }
                    else if (covfind.strValue!=null)
                    {
                        covfind.strValue = item.strValue;
                    }
                }
            }

            Update(find);
        }
        public float GetMaxCode(Expression<Func<Customer, bool>> where = null)
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
    }

}