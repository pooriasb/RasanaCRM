using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Web.Models.Entity;
using Web.Models.Repositories;
using Web.ViewModels.Enums;

namespace Web.Insfrastructure.Utilities
{
    public class UTLSiteValues
    {
        public static int GetSiteValueId(Enums.SiteValue siteValue)
        {
            switch (siteValue)
            {
                case Enums.SiteValue.productCategory: return 1;
                case Enums.SiteValue.brand: return 2;
                case Enums.SiteValue.province: return 3;
                case Enums.SiteValue.OrganizationUnit: return 4;
                case Enums.SiteValue.productUnit: return 5;
                case Enums.SiteValue.Discount: return 6;
                case Enums.SiteValue.FactorMessage: return 7;
                case Enums.SiteValue.SectionType: return 8;
                case Enums.SiteValue.CustomerCategory: return 9;
                case Enums.SiteValue.PaymentMessage: return 10;
                case Enums.SiteValue.SellFactorStatus: return 11;
                case Enums.SiteValue.BuyFactorStatus: return 12;
                case Enums.SiteValue.FactorOptionValues: return 13;
                case Enums.SiteValue.FactorPresentationType: return 14;
                default:
                    {
                        return 0;
                    }
            }

        }

        public static IEnumerable<SiteValue> GetOrganizations()
        {
            UnitOfWork.IUnitOfWork unitOfWork = new UnitOfWork.UnitOfWork();
            int organizationUnitPatentId = GetSiteValueId(Enums.SiteValue.OrganizationUnit);
            return unitOfWork.SiteValueRepository.GetAll(x => x.isDelete == false && x.parentId == organizationUnitPatentId);
        }
        public static int GetSectionTypeId(Enums.SectionType sectionType)
        {
            switch (sectionType)
            {
                case Enums.SectionType.Product: return 5348;
                case Enums.SectionType.Factor: return 5349;
                default:
                    {
                        return 0;
                    }
            }
        }
        public static string GetSectionTypeName(int id)
        {
            if (id == GetSectionTypeId(Enums.SectionType.Factor))
            {
                return Enums.SectionType.Factor.ToString();
            }
            else if (id == GetSectionTypeId(Enums.SectionType.Product))
            {
                return Enums.SectionType.Product.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetValueField(string key, string jsonValue)
        {
            try
            {
                List<ValueStructure> values = JsonConvert.DeserializeObject<List<ValueStructure>>(jsonValue);
                var valueFind = values.Where(x => x.key == key).FirstOrDefault();
                if (valueFind != null)
                {
                    return valueFind.value;
                }
                else
                {
                    return "Not found";
                }
            }
            catch (Exception e)
            {
                return "Error";
            }
        }
        public static List<ValueStructure> GetValuesField(string jsonValue)
        {
            try
            {
                List<ValueStructure> values = JsonConvert.DeserializeObject<List<ValueStructure>>(jsonValue);
                return values;
            }
            catch (Exception e)
            {
                return new List<ValueStructure>();
            }
        }
        public static string SetValueField(string key, string value, string jsonValues = null)
        {
            //id is siteValue id table
            try
            {
                if (jsonValues != null)
                {
                    List<ValueStructure> values = JsonConvert.DeserializeObject<List<ValueStructure>>(jsonValues);

                    var keyFind = values.Where(x => x.key == key).FirstOrDefault();
                    if (keyFind == null)
                    {
                        values.Add(new ValueStructure()
                        {
                            key = key,
                            value = value,
                        });
                        return JsonConvert.SerializeObject(values);
                    }
                    else
                    {
                        return "Duplicate";
                    }
                }
                else
                {
                    List<ValueStructure> values = new List<ValueStructure>();
                    values.Add(new ValueStructure()
                    {
                        key = key,
                        value = value,
                    });
                    return JsonConvert.SerializeObject(values);
                }

            }
            catch (Exception e)
            {
                return "Error";
            }
        }
        public static string EditValueField(string key, string value, string jsonValues)
        {
            //id is siteValue id table
            try
            {
                List<ValueStructure> values = JsonConvert.DeserializeObject<List<ValueStructure>>(jsonValues);

                var keyFind = values.Where(x => x.key == key).FirstOrDefault();
                if (keyFind != null)
                {
                    keyFind.value = value;
                    return JsonConvert.SerializeObject(values);
                }
                else
                {
                    return "NotFound";
                }
            }
            catch (Exception e)
            {
                return "Error";
            }
        }
    }

    public class ValueStructure
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}