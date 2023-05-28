using System.Collections.Generic;
using System.Web.Mvc;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Models.Entity;
using Web.ViewModels.Enums;

namespace Web.Models.Repositories
{
    public class CustomersRelationRepository : Repository<CustomerRelation>
    {
        public CustomersRelationRepository(CRM_DbContext dbContext) : base(dbContext)
        {

        }

        public void Insert(CustomerRelationViewModels.Add model)
        {
            CustomerRelation customerRelation = new CustomerRelation()
            {
                customer_id = model.customer_id,
                Job = model.Job,
                family = model.family,
                name = model.name,
                nationalCode = model.nationalCode,
                fax = model.fax,
                phone = model.phone,
                postCode = model.postCode,
                tell = model.tell,
            };

            //categoryDynamic
            if (model.customerCategoryDynamic != null)
            {
                if (model.customerCategoryDynamic.Count != 0)
                {
                    foreach (var dynamicse in model.customerCategoryDynamic)
                    {
                        customerRelation.CustomersOptionValues.Add(new CustomersOptionValue() { customerRelation_id = customerRelation.id, value_id = dynamicse.selectTagName, option_id = dynamicse.selectTagId,type = Enums.OptionType.DropDown.ToString()});
                    }
                }
            }
            if (model.customerCategoryDynamicString != null)
            {
                if (model.customerCategoryDynamicString.Count != 0)
                {
                    foreach (var dynamicse in model.customerCategoryDynamicString)
                    {
                        if (dynamicse.textBoxValue != null)
                        {
                            customerRelation.CustomersOptionValues.Add(new CustomersOptionValue() { customerRelation_id = customerRelation.id, option_id = dynamicse.textBoxId, strValue = dynamicse.textBoxValue,type = Enums.OptionType.String.ToString()});
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
                        customerRelation.CustomersOptionValues.Add(new CustomersOptionValue() { customerRelation_id = customerRelation.id, value_id = dynamicse.selectTagName, option_id = dynamicse.selectTagId, type = Enums.OptionType.DropDown.ToString() });
                    }
                }
            }
            if (model.customerOrganizationDynamicString != null)
            {
                if (model.customerOrganizationDynamicString.Count != 0)
                {
                    foreach (var dynamicse in model.customerOrganizationDynamicString)
                    {
                        if (dynamicse.textBoxValue != null)
                        {
                            customerRelation.CustomersOptionValues.Add(new CustomersOptionValue()
                            {
                                customerRelation_id = customerRelation.id,
                                option_id = dynamicse.textBoxId,
                                strValue = dynamicse.textBoxValue,
                                type = Enums.OptionType.String.ToString()
                            });
                        }
                    }
                }
            }
            Insert(customerRelation);
        }

        public void Edit(CustomerRelationViewModels.Edit model,CustomerOptionValueRepository covr)
        {
            var find = GetByID(model.id);
            find.Job = model.Job;
            find.family = model.family;
            find.name = model.name;
            find.nationalCode = model.nationalCode;
            find.fax = model.fax;
            find.phone = model.phone;
            find.postCode = model.postCode;
            find.tell = model.tell;
            find.customer_id = model.customer_id;

            //categoryDynamic
            if (model.customerCategoryDynamic != null)
            {
                if (model.customerCategoryDynamic.Count != 0)
                {
                    foreach (var dynamicse in model.customerCategoryDynamic)
                    {
                        find.CustomersOptionValues.Add(new CustomersOptionValue() { customerRelation_id = find.id, value_id = dynamicse.selectTagName, option_id = dynamicse.selectTagId, type = Enums.OptionType.DropDown.ToString() });
                    }
                }
            }
            if (model.customerCategoryDynamicString != null)
            {
                if (model.customerCategoryDynamicString.Count != 0)
                {
                    foreach (var dynamicse in model.customerCategoryDynamicString)
                    {
                        if (dynamicse.textBoxValue != null)
                        {
                            find.CustomersOptionValues.Add(new CustomersOptionValue() { customerRelation_id = find.id, option_id = dynamicse.textBoxId, strValue = dynamicse.textBoxValue, type = Enums.OptionType.String.ToString() });
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
                        find.CustomersOptionValues.Add(new CustomersOptionValue() { customerRelation_id = find.id, value_id = dynamicse.selectTagName, option_id = dynamicse.selectTagId, type = Enums.OptionType.DropDown.ToString() });
                    }
                }
            }
            if (model.customerOrganizationDynamicString != null)
            {
                if (model.customerOrganizationDynamicString.Count != 0)
                {
                    foreach (var dynamicse in model.customerOrganizationDynamicString)
                    {
                        if (dynamicse.textBoxValue != null)
                        {
                            find.CustomersOptionValues.Add(new CustomersOptionValue()
                            {
                                customerRelation_id = find.id,
                                option_id = dynamicse.textBoxId,
                                strValue = dynamicse.textBoxValue,
                                type = Enums.OptionType.String.ToString()
                            });
                        }
                    }
                }
            }



            foreach (var item in model.CustomersOptionValues)
            {
                var covfind = covr.GetByID(item.id);
                if (covfind != null)
                {
                    if (covfind.type==Enums.OptionType.DropDown.ToString())
                    {
                        covfind.value_id = item.value_id;
                    }
                    else if (covfind.type == Enums.OptionType.String.ToString())
                    {
                        covfind.strValue = item.strValue;
                    }
                }
            }
            Update(find);
        }
    }
}