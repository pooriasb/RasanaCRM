using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Web.Areas.Employee.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models.Entity;
using Web.ViewModels.Enums;

namespace Web.Models.Repositories
{
    public class SiteValueRepository : Repository<SiteValue>
    {
        public SiteValueRepository(CRM_DbContext _context) : base(_context)
        {
        }
        public void Insert(SiteValuesViewModels.Add model)
        {
            SiteValue siteValue;
            siteValue = new SiteValue()
            {
                id = GetMaxId(),
                parentId = model.parentId,
                code = model.code,
                name = model.name.Trim(),
                value = model.value,
                description = model.description,
                isEnable = model.isEnable
            };
            Insert(siteValue);
        }
        public void Insert(SiteValuesViewModels.AddSectionType model)
        {
            SiteValue siteValue;
            siteValue = new SiteValue()
            {
                id = GetMaxId(),
                parentId = model.parentId,
                code = model.code,
                name = model.name.Trim(),
                description = model.description,
                isEnable = model.isEnable
            };
            if (model.workFlow)
            {
                siteValue.value = "workFlow=True";
            }
            else
            {
                siteValue.value = "workFlow=False";
            }
            Insert(siteValue);
        }
        public void Edit(SiteValuesViewModels.EditSectionType model)
        {
             var find= GetByID(model.id);
            find.description = model.description;
            find.code = model.code;
            find.isEnable = model.isEnable;
            find.name = model.name;
            if (model.workFlow)
            {
                find.value = "workFlow=True";
            }
            else
            {
                find.value = "workFlow=False";
            }
        }
        public void Edit(SiteValuesViewModels.Edit model)
        {
            SiteValue siteValue = new SiteValue()
            {
                parentId = model.parentId,
                name = model.name,
                id = model.id,
                code = model.code,
                description = model.description,
                isEnable = model.isEnable,
                value = model.value
            };
            Update(siteValue);
        }

        public void DeletePhysical(int id)
        {
            var find = GetByID(id);
            find.isDelete = true;
        }


        public int Insert(SiteValuesViewModels.AddHeaderOption model)
        {
            int maxId = GetMaxId(1501, 1700, x => x.id > 1500 && x.id < 1700);
            if (maxId == 0)
                return 0;
            SiteValue siteValue = new SiteValue()
            {
                id = maxId,
                parentId = model.subCategoryId,
                name = model.name,
                value = UTLSiteValues.SetValueField(Enums.SiteValue.OptionType.ToString(), model.TypeOptopn),
                code = GetMaxCode(x => x.parentId == model.subCategoryId),
                isEnable = true
            };
                siteValue.value = UTLSiteValues.SetValueField(Enums.SiteValueFieldJson.isRequired.ToString(), model.isRequired.ToString(), siteValue.value);
            siteValue.value = UTLSiteValues.SetValueField(Enums.SiteValueFieldJson.FieldDynamic.ToString(), "True", siteValue.value);
            Insert(siteValue);
            return 1;
        }

        public void Insert(int id, string value)
        {
            SiteValue siteValue = new SiteValue()
            {
                id = GetMaxId(),
                code = GetMaxCode(x => x.parentId == id),
                parentId = id,
                isEnable = true,
                name = value,
            };
            Insert(siteValue);
        }

        public void Insert(SiteValuesViewModels.AddDiscount model, string json)
        {
            try
            {
                SiteValue siteValue = new SiteValue()
                {
                    id = GetMaxId(),
                    code = GetMaxCode(x => x.parentId == model.parentId),
                    name = "تخفیف",
                    value = json,
                    parentId = model.parentId,
                    description = model.description,
                    isEnable = true
                };
                Insert(siteValue);

                //log
            }
            catch (Exception e)
            {
                //log
            }
        }

        public bool SearchDiscounts(List<SiteValuesViewModels.AddDiscountJson> jsons, float left, float right)
        {
            var find = jsons.Where(x => x.left == left && x.right == right).FirstOrDefault();
            if (find == null)
                return true;
            else
                return false;
        }
        public List<SiteValuesViewModels.AddDiscountJson> GetDiscounts(string json)
        {
            var deserialize = JsonConvert.DeserializeObject<List<SiteValuesViewModels.AddDiscountJson>>(json);
            return deserialize;
        }

        public IEnumerable<SiteValuesViewModels.Gategories> GetCategories()
        {
            int parentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.productCategory);
            var categories =
                GetAll(x => x.parentId == parentId && x.isEnable && x.isDelete == false
                ).Select(x => new SiteValuesViewModels.Gategories
                {
                    CategoryId = x.id,
                    CategoryName = x.name
                }).ToList();
            return categories;
        }

        public IEnumerable<SiteValuesViewModels.IdName> GetIdNameByParentId(int parentId)
        {
            var result = GetAll(x => x.parentId == parentId && x.isDelete == false).Select(
                x => new SiteValuesViewModels.IdName()
                {
                    id = x.id,
                    name = x.name
                }).ToList();
            return result;
        }
        public int GetMaxId(Expression<Func<SiteValue, bool>> where = null)
        {
            var result = GetAll(where).ToList();

            if (result.Count != 0)
            {
                int maxId = result.Max(x => x.id);
                if (maxId <= 5000)
                    return 5001;
                return maxId + 1;
            }
            return 5001;
        }
        public int GetMaxId(int min, int max, Expression<Func<SiteValue, bool>> where = null)
        {
            var result = GetAll(where).ToList();

            if (result.Count != 0)
            {
                int maxId = result.Max(x => x.id);
                if (maxId >= min && maxId <= max)
                    return maxId + 1;
                return 0;
            }
            return min;
        }
        public int GetMaxCode(Expression<Func<SiteValue, bool>> where = null)
        {
            var result = GetAll(where).ToList();
            int maxCode;
            if (result.Count != 0)
            {
                maxCode = result.Max(x => x.code??0);
                return maxCode + 1;
            }
            return 1;
        }

    }

    public class selectItem
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}