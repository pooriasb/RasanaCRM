using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Web.Areas.Factor.Models.ViewModels;
using Web.Insfrastructure.UnitOfWork;
using Web.Insfrastructure.Utilities;
using Web.Models;
using Web.Models.Entity;
using Web.Models.Repositories;
using Web.ViewModels.Enums;

namespace Web.Areas.Factor.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    public class SellFactorController : Controller
    {

        private readonly IUnitOfWork unitOfWork;

        public SellFactorController()
        {
            this.unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// تابع کد برای احراز کد کاربر و همچنین دریافت اخرین کد از دیتابیس
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult Code(int? code)
        {
            try
            {
                if (code.HasValue)
                {
                    var isExist = unitOfWork.FactorRepository.ExistCode(code.Value);
                    if (!isExist)
                    {
                        return Json(new JsonResultCustom() { statusCode = HttpStatusCode.SeeOther, data = null }, JsonRequestBehavior.DenyGet);
                    }
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.OK, data = true }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    var generate = unitOfWork.FactorRepository.GetMaxCode(x => x.code != null);
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.OK, data = generate }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        [System.Web.Http.HttpGet]
        public JsonResult GetCustomer(string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int code = 0;
                    IEnumerable<Customer> customers;
                    if (int.TryParse(value, out code))
                    {
                        customers = unitOfWork.CustomerRepository.Where(c => c.name.Contains(value) || (c.code != null && c.code == code));
                    }
                    else
                    {
                        customers = unitOfWork.CustomerRepository.Where(c => c.name.Contains(value));
                    }
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.OK, data = customers.Select(x => new { id = x.id, name = x.name, code = x.code }) },
                        JsonRequestBehavior.DenyGet);
                }
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.BadRequest },
                    JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        [System.Web.Http.HttpGet]
        public JsonResult GetCodes(List<Enums.SiteValue> codes)
        {
            try
            {
                if (codes.Any())
                {
                    List<SiteValue> siteValues = new List<SiteValue>();
                    foreach (var siteValue in codes)
                    {
                        var parentId = UTLSiteValues.GetSiteValueId(siteValue);

                        var finds = unitOfWork.SiteValueRepository
                            .Where(x => x.isDelete == false && x.parentId == parentId).AsEnumerable();
                        siteValues.AddRange(finds);
                    }
                    //return result
                    return Json(
                        new JsonResultCustom()
                        {
                            statusCode = HttpStatusCode.OK,
                            data = siteValues.GroupBy(x => x.SiteValue1.name)
                                .Select(x => new
                                {
                                    key = x.Key,
                                    values = x.Select(y => new { id = y.id, name = y.name, parentId = y.parentId })
                                })
                        }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.BadRequest },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult GetProduct(string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int code = 0;
                    IEnumerable<Product> products;
                    if (int.TryParse(value, out code))
                    {
                        products = unitOfWork.ProductRepository.GetAll2(c => c.title.Contains(value) || c.code == code, null, x => x.ProductPrices);
                    }
                    else
                    {
                        products = unitOfWork.ProductRepository.GetAll2(c => c.title.Contains(value), null, x => x.ProductPrices);
                    }
                    //return
                    return Json(
                        new JsonResultCustom()
                        {
                            statusCode = HttpStatusCode.OK,
                            data = products.Select(x => new
                            {
                                id = x.id,
                                name = x.title,
                                code = x.code,
                                prices = x.ProductPrices.Where(y => y.isDelete == false).AsEnumerable()
                                    .Select(y => new { id = y.id, price = y.price, vahedName = y.SiteValue.name }),

                            })
                        }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.BadRequest },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [System.Web.Http.HttpGet]
        public JsonResult Initionalize()
        {
            try
            {
                List<Enums.SiteValue> enumList = new List<Enums.SiteValue>()
                {
                    Enums.SiteValue.FactorMessage,
                    Enums.SiteValue.PaymentMessage,
                     // Enums.SiteValue.SellFactorStatus,
                    //Enums.SiteValue.FactorOptionValues,
                    Enums.SiteValue.FactorPresentationType,
                };
                List<SiteValue> dynamics = new List<SiteValue>();
                foreach (var siteValue in enumList)
                {
                    var parentId = UTLSiteValues.GetSiteValueId(siteValue);
                    var finds = unitOfWork.SiteValueRepository
                        .Where(x => x.isDelete == false && x.parentId == parentId).AsEnumerable();
                    dynamics.AddRange(finds);
                }

                var factorCosts = unitOfWork.FactorCostSetRepository.GetAll(x => x.isEnable);
                var dateTime = UTLDateTime.getCurrentDate();
                var code = unitOfWork.FactorRepository.GetMaxCode(x => x.code != null);
                var siteVals = new CRM_DbContext().SiteValues.Select(c => new DynumicTemp()
                {
                    Id = c.id,
                    Name = c.name
                }).ToList();
                return Json(
                    new JsonResultCustom()
                    {
                        statusCode = HttpStatusCode.OK,
                        data = new
                        {

                            code,
                            dateTime,
                            factorCosts = factorCosts.Select(x => new { x.id, x.isEnable, x.isInCrease, x.isInFee, x.isInItem, x.isShowCustomer, x.isPresent, x.name }),
                            dynamics = dynamics.GroupBy(x => x.SiteValue1.id)
                                .Select(x => new
                                {
                                    key = x.Key,
                                    values = x.Select(y => new
                                    {
                                        y.id,
                                        y.name,
                                        y.code,
                                        type = UTLSiteValues.GetValueField(Enums.SiteValue.OptionType.ToString(), y.value),
                                        isRequired = UTLSiteValues.GetValueField(Enums.SiteValueFieldJson.isRequired.ToString(), y.value),
                                    })
                                }),
                            siteVals
                        }
                    }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Initionalize2()
        {
            try
            {
                var factorMessageParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.FactorMessage);
                var factorMessage = unitOfWork.SiteValueRepository
                    .Where(x => x.isDelete == false && x.parentId == factorMessageParentId).AsEnumerable().Select(y => new
                    {
                        y.id,
                        y.name,
                        y.code,
                    });

                var paymentMessageParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.PaymentMessage);
                var paymentMessage = unitOfWork.SiteValueRepository
                    .Where(x => x.isDelete == false && x.parentId == paymentMessageParentId).AsEnumerable().Select(y => new
                    {
                        y.id,
                        y.name,
                        y.code,
                    });

                var sellFactorStatusParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.SellFactorStatus);
                var sellFactorStatus = unitOfWork.SiteValueRepository
                    .Where(x => x.isDelete == false && x.parentId == sellFactorStatusParentId).AsEnumerable().Select(y => new
                    {
                        y.id,
                        y.name,
                        y.code,
                    });

                var factorOptionValuesParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.FactorOptionValues);
                var factorOptionValues = unitOfWork.SiteValueRepository
                    .Where(x => x.isDelete == false && x.parentId == factorOptionValuesParentId).AsEnumerable().Select(
                        y => new
                        {
                            y.id,
                            y.name,
                            y.code,
                            type = UTLSiteValues.GetValueField(Enums.SiteValue.OptionType.ToString(), y.value),
                            isRequired =
                            UTLSiteValues.GetValueField(Enums.SiteValueFieldJson.isRequired.ToString(), y.value),
                        });

                var factorPresentationTypeParentId = UTLSiteValues.GetSiteValueId(Enums.SiteValue.FactorPresentationType);
                var factorPresentationType = unitOfWork.SiteValueRepository
                    .Where(x => x.isDelete == false && x.parentId == factorPresentationTypeParentId).AsEnumerable().Select(y => new
                    {
                        y.id,
                        y.name,
                        y.code,
                    });


                var factorCosts = unitOfWork.FactorCostSetRepository.GetAll(x => x.isEnable);
                var dateTime = UTLDateTime.getCurrentDate();
                var code = unitOfWork.FactorRepository.GetMaxCode(x => x.code != null);
                return Json(
                    new JsonResultCustom()
                    {
                        statusCode = HttpStatusCode.OK,
                        data = new
                        {
                            code,
                            dateTime,
                            factorCosts = factorCosts.Select(x => new { x.id, x.isEnable, x.isInCrease, x.isInFee, x.isInItem, x.isShowCustomer, x.isPresent, x.name }),
                            factorMessage,
                            paymentMessage,
                            sellFactorStatus,
                            factorPresentationType,
                            factorOptionValues
                        }
                    }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [System.Web.Http.HttpPost]
        public JsonResult FactorItem(int? id, FactorViewModels.Add model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(
                        new JsonResultCustom()
                        {
                            statusCode = HttpStatusCode.BadRequest,
                            data = "مقادیر وارد شده صحیح نمی باشد"
                        }, JsonRequestBehavior.DenyGet);
                }
                if (id.HasValue)
                {
                    //Edit
                    var factorFind = unitOfWork.FactorRepository.GetByID(id);
                    factorFind.priceTotalFactor += model.priceTotalItem;
                    model.factorId = factorFind.id;
                    //FactorItem
                    int factorItemId = unitOfWork.FactorItemRepository.Insert(model, unitOfWork);
                    model.factorItemId = factorItemId;
                    //FactorItemCost
                    if (model.FactorItemCosts != null)
                    {
                        unitOfWork.FactorItemCostRepository.Insert(model);
                        if (!unitOfWork.Save())
                        {
                            return Json(
                                new JsonResultCustom()
                                {
                                    statusCode = HttpStatusCode.InternalServerError,
                                    data = "در خیره سازی اطلاعات خطایی رخ داده است"
                                }, JsonRequestBehavior.DenyGet);
                        }
                    }
                    return Json(
                        new JsonResultCustom()
                        {
                            statusCode = HttpStatusCode.OK,
                            data = new { factorId = factorFind.id, factorItemId = factorItemId }
                        }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    //Add
                    //Factor
                    var factorId = unitOfWork.FactorRepository.Insert(model, User.Identity.GetUserId(), unitOfWork);
                    model.factorId = factorId;
                    //FactorItem
                    int factorItemId = unitOfWork.FactorItemRepository.Insert(model, unitOfWork);
                    model.factorItemId = factorItemId;
                    //FactorItemCost
                    if (model.FactorItemCosts != null)
                    {
                        unitOfWork.FactorItemCostRepository.Insert(model);
                        if (!unitOfWork.Save())
                        {
                            return Json(
                                new JsonResultCustom()
                                {
                                    statusCode = HttpStatusCode.InternalServerError,
                                    data = "در خیره سازی اطلاعات خطایی رخ داده است"
                                }, JsonRequestBehavior.DenyGet);
                        }
                    }
                    return Json(
                        new JsonResultCustom()
                        {
                            statusCode = HttpStatusCode.OK,
                            data = new { factorId = factorId, factorItemId = factorItemId }
                        }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        [System.Web.Http.HttpPost]
        public JsonResult EditFactorItem(FactorItemViewModels.Edit model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(
                        new JsonResultCustom()
                        {
                            statusCode = HttpStatusCode.BadRequest,
                            data = "مقادیر وارد شده صحیح نمی باشد"
                        }, JsonRequestBehavior.DenyGet);
                }
                unitOfWork.FactorItemRepository.Edit(model);
                if (model.FactorItemCosts != null)
                    unitOfWork.FactorItemCostRepository.Edit(model);
                if (!unitOfWork.Save())
                {
                    return Json(
                        new JsonResultCustom()
                        {
                            statusCode = HttpStatusCode.InternalServerError,
                            data = "در خیره سازی اطلاعات خطایی رخ داده است"
                        }, JsonRequestBehavior.DenyGet);
                }
                return Json(
                    new JsonResultCustom()
                    {
                        statusCode = HttpStatusCode.OK,
                    }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [System.Web.Http.HttpPost]
        public JsonResult RemoveFactorItem(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    unitOfWork.FactorItemRepository.Delete(id);
                    if (!unitOfWork.Save())
                    {
                        return Json(
                            new JsonResultCustom()
                            {
                                statusCode = HttpStatusCode.InternalServerError,
                                data = "در خیره سازی اطلاعات خطایی رخ داده است"
                            }, JsonRequestBehavior.DenyGet);
                    }
                    return Json(
                        new JsonResultCustom()
                        {
                            statusCode = HttpStatusCode.OK,
                        }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.BadRequest, data = "پارامتر ورودی خالی می باشد" }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        [System.Web.Http.HttpPost]
        public JsonResult Add(FactorViewModels.Save model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return Json(
                //        new JsonResultCustom()
                //        {
                //            statusCode = HttpStatusCode.BadRequest,
                //            data = "مقادیر وارد شده صحیح نمی باشد"
                //        }, JsonRequestBehavior.DenyGet);
                //}

                // اگر فاکتور فاکتور متمم بود
                if (model.parentId.HasValue && model.addonCode.HasValue)
                {
                    //check duplicate addonCode
                    var addonCodeFind = unitOfWork.FactorRepository.ExistAddonCode(model.parentId.Value, model.addonCode.Value);
                    if (addonCodeFind)
                    {
                        unitOfWork.FactorRepository.Insert(model, User.Identity.GetUserId(), unitOfWork.FactorCostRepository, unitOfWork);
                        unitOfWork.FactorOptionValueRepository.Insert(model);
                    }
                    else
                    {
                        return Json(new JsonResultCustom() { statusCode = HttpStatusCode.SeeOther, data = "مقدار کد متتم تکراری می باشد" }, JsonRequestBehavior.DenyGet);
                    }
                }
                unitOfWork.FactorRepository.Insert(model, User.Identity.GetUserId(), unitOfWork.FactorCostRepository, unitOfWork);
                //if (model.addDynamics != null)
                //    unitOfWork.FactorOptionValueRepository.Insert(model);
                if (model.factorCostAfters != null)
                    unitOfWork.FactorItemCostRepository.Insert(model);
                if (!unitOfWork.Save())
                {
                    return Json(
                        new JsonResultCustom()
                        {
                            statusCode = HttpStatusCode.InternalServerError,
                            data = "در خیره سازی اطلاعات خطایی رخ داده است"
                        }, JsonRequestBehavior.DenyGet);
                }
                return Json(
                    new JsonResultCustom()
                    {
                        statusCode = HttpStatusCode.OK,
                    }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [System.Web.Http.HttpGet]
        public JsonResult GetDropDownItems(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var items = unitOfWork.SiteValueRepository.Where(x => x.isDelete == false && x.parentId == id).AsEnumerable()
                        .Select(x => new { id = x.id, x.name, x.code });
                    if (items.Count() != 0)
                    {
                        return Json(new JsonResultCustom() { statusCode = HttpStatusCode.OK, data = items }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new JsonResultCustom() { statusCode = HttpStatusCode.BadRequest, data = "داده ای یافت نشد" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.BadRequest, data = "داده وارد شده صحیح نمی باشد" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Edit

        public class ProductEditDTO
        {

            //x.id,
            //                        x.count,
            //                        x.garanty,
            //                        x.warranty,
            //                        x.description,
            //                        x.price,
            //                        x.price_id,
            //                        x.product_id,
            //                        x.total,

            public int id { get; set; }
            public int count { get; set; }
            public int warranty { get; set; }
            public int garanty { get; set; }
            public long price { get; set; }
            public int price_id { get; set; }
            public int product_id { get; set; }
            public long total { get; set; }
            public string vahedName { get; set; }
            public List<FactorItemCostsDTO> factorCost { get; set; }

        }
        public class FactorItemCostsDTO
        {
            public int id { get; set; }
            public int? factorItem_id { get; set; }
            public int? factorCostSet_id { get; set; }
            public bool isInFee { get; set; }
            public bool isIncrease { get; set; }
            public bool isPersent { get; set; }
            public string value { get; set; }

        }
        /// <summary>
        /// نمایش اولیه اطلاعات فاکتور برای ویرایش
        /// </summary>
        /// <param name="id">ایدی فاکتور</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public JsonResult EditInitionalize(int? id)
        {
            try
            {
                if (id.HasValue)
                {

                    var factor = unitOfWork.FactorRepository.GetByID(id);
                    UTLDateTime utlDateTime = new UTLDateTime();
                    //////////////////////////////////////////////////////
                    List<ProductEditDTO> Products = new List<ProductEditDTO>();
                    var items = factor.FactorItems.ToList();

                    foreach (var item in items)
                    {
                        List<FactorItemCostsDTO> costs = new List<FactorItemCostsDTO>();
                        var itemcosts = item.FactorItemCosts.ToList();
                        foreach (var cost in itemcosts)
                        {

                            costs.Add(new FactorItemCostsDTO()
                            {
                                factorCostSet_id = cost.FactorCostSet_id,
                                factorItem_id = cost.factorItem_id,
                                id = cost.id,
                                value = cost.value,
                                isInFee = cost.FactorCostSet.isInFee,
                                isIncrease = cost.FactorCostSet.isInCrease,
                                isPersent = cost.FactorCostSet.isPresent


                            });

                        }

                        Products.Add(new ProductEditDTO()
                        {
                            count = item.count,
                            garanty = (int)item.garanty,
                            id = item.id,
                            price = item.price,
                            price_id = item.price_id,
                            total = item.total,
                            warranty = (int)item.warranty,
                            product_id = item.product_id,
                            vahedName = item.ProductPrice.SiteValue.name,
                            factorCost = costs
                        });






                    }

                    //    items.ForEach(x =>
                    //{
                    //    List<FactorItemCostsDTO> costs = null;
                    //    var itemCosts=x.FactorItemCosts.ToList();
                    //    itemCosts.ForEach(y =>
                    //       {
                    //          costs.Add( new FactorItemCostsDTO()
                    //           {
                    //               factorCostSet_id = y.FactorCostSet_id,
                    //               factorItem_id = y.factorItem_id,
                    //               id = y.id,
                    //               value = y.value

                    //           });
                    //       }
                    //     );
                    //    Products.Add(new ProductEditDTO()
                    //    {
                    //        count = x.count,
                    //        garanty = (int)x.garanty,
                    //        id = x.id,
                    //        price = x.price,
                    //        price_id = x.price_id,
                    //        total = x.total,
                    //        warranty = (int)x.warranty,
                    //        product_id = x.product_id,
                    //        vahedName = x.ProductPrice.SiteValue.name,
                    //        factorCost = costs
                    //    });
                    //});


                    /////////////////////////////////////////////////

                    return Json(
                    new JsonResultCustom()
                    {
                        statusCode = HttpStatusCode.OK,
                        data = new
                        {
                            FactorItemsNew = Products,
                            factor = new
                            {
                                factor.id,
                                factor.code,
                                factor.addonCode,
                                dateTime = utlDateTime.convertToPersianDate(factor.dateTime.Value.Date.ToString()),
                                factor.description,
                                factor.customer_id,
                                customerName = factor.Customer.name,
                                expair = factor.priceCredit,
                                factor.isRasmi,
                                factor.expireDate,
                                factor.parent_id,
                                factor.person,
                                factor.paymentDescription,
                                factor.priceTotalItem,
                                factor.priceTotalFactor,
                                factor.priceCredit,
                                deliveryPlace = factor.placeOfDelivery,
                                presentationId = factor.presentation,
                                presentationName = factor.SiteValue1.name,
                                statusId = factor.status,
                                statusName = factor.SiteValue.name,
                                paymentTypeId = factor.paymentType,
                                paymentName = factor.SiteValue2.name
                            },
                            factorItem = factor.FactorItems.AsEnumerable().Select(x => new
                            {
                                x.id,
                                x.count,
                                x.garanty,
                                x.warranty,
                                x.description,
                                x.price,
                                x.price_id,
                                x.product_id,
                                x.total,
                                selectedProduct = GetProduct(x.product_id),

                                vahedName = x.ProductPrice.SiteValue.name,
                                itemCost = x.FactorItemCosts.AsEnumerable().Select(y => new
                                {
                                    y.id,
                                    y.factorItem_id,
                                    y.value,
                                    y.FactorCostSet_id
                                })
                            }),
                            factorCost = factor.FactorCosts.AsEnumerable().Select(x => new
                            {
                                x.id,
                                x.factorCostSet_id,
                                x.value,
                                x.FactorCostSet.name
                            }),
                            factorOptionValue = factor.FactorOptionValues.AsEnumerable().Select(x => new
                            {
                                x.id,
                                optionName = x.SiteValue.name,
                                x.option_id,
                                valueName = x.SiteValue1.name,
                                x.value_id,
                                x.strValue,
                                x.type
                            })
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.BadRequest },
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// تایید ویرایش فاکتور
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult EditConfirm(FactorViewModels.Edit model, int? parentId, int? addonCode)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return Json(
                //        new JsonResultCustom()
                //        {
                //            statusCode = HttpStatusCode.BadRequest,
                //            data = "مقادیر وارد شده صحیح نمی باشد"
                //        }, JsonRequestBehavior.DenyGet);
                //}
                unitOfWork.FactorRepository.Edit(model, User.Identity.GetUserId(), unitOfWork.FactorItemRepository,
                    unitOfWork.FactorItemCostRepository, unitOfWork.FactorCostRepository,
                    unitOfWork.FactorOptionValueRepository);
                if (model.factorCostAfter != null)
                    unitOfWork.FactorItemCostRepository.Edit(model);
                if (!unitOfWork.Save())
                {
                    return Json(
                        new JsonResultCustom()
                        {
                            statusCode = HttpStatusCode.InternalServerError,
                            data = "در خیره سازی اطلاعات خطایی رخ داده است"
                        }, JsonRequestBehavior.DenyGet);
                }
                return Json(
                    new JsonResultCustom()
                    {
                        statusCode = HttpStatusCode.OK,
                    }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion
        #region Addition
        /// <summary>
        /// احراز کد متمم و همچنین دریافت اخرین کد متمم براساس کد فاکتور
        /// </summary>
        /// <param name="factorId">ایدی فاکتور</param>
        /// <param name="addonCode">کد متمم</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult AddonCode(int? factorId, int? addonCode)
        {
            try
            {
                if (addonCode.HasValue && factorId.HasValue)
                {
                    var isExist = unitOfWork.FactorRepository.ExistAddonCode(factorId.Value, addonCode.Value);
                    if (!isExist)
                    {
                        return Json(new JsonResultCustom() { statusCode = HttpStatusCode.SeeOther, data = null }, JsonRequestBehavior.DenyGet);
                    }
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.OK, data = true }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    var generate = unitOfWork.FactorRepository.GetMaxAddonCode(x => x.parent_id == factorId.Value);
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.OK, data = generate }, JsonRequestBehavior.DenyGet);
                }

            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        /// <summary>
        /// ویرایش کد متمم فاکتور
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="addonCode"></param>
        /// <param name="addonCodeNew"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult EditAddonCode(int? factorId, int? addonCode, int? addonCodeNew)
        {
            try
            {
                if (addonCode.HasValue && factorId.HasValue)
                {
                    var isExist = unitOfWork.FactorRepository.ExistAddonCode(factorId.Value, addonCodeNew.Value);
                    if (!isExist)
                    {
                        return Json(new JsonResultCustom() { statusCode = HttpStatusCode.SeeOther, data = null }, JsonRequestBehavior.DenyGet);
                    }
                    else
                    {
                        var find = unitOfWork.FactorRepository.GetByID(addonCode);
                        find.addonCode = addonCodeNew.Value;
                        unitOfWork.Save();
                        return Json(new JsonResultCustom() { statusCode = HttpStatusCode.OK, data = "با موفقیت ویرایش شد" }, JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    var generate = unitOfWork.FactorRepository.GetMaxAddonCode(x => x.parent_id == factorId.Value);
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.OK, data = generate }, JsonRequestBehavior.DenyGet);
                }

            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult Addition()
        {
            return View();
        }

        #endregion
        public static object GetProduct(int? id)
        {
            try
            {

                var res = new CRM_DbContext().Products.FirstOrDefault(c => c.id == id);
                return new
                {
                    id = res.id,
                    name = res.title
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public JsonResult GetProductPrices(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var items = unitOfWork.ProductPriceRepository.GetAll(x => x.product_id == id).Select(x => new
                    {
                        id = x.id,
                        vahedName = x.SiteValue.name,
                        price = x.price
                    }).ToList();
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.OK, data = items }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.BadRequest, data = "داده ای یافت نشد" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception e)
            {

                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.AllowGet);

            }



        }

    }



    /// <summary>
    /// ساختار یکسان برای پاسخ به درخواست ها در توابع Api
    /// </summary>
    public class JsonResultCustom
    {
        public HttpStatusCode statusCode { get; set; }
        public dynamic data { get; set; }
    }
}