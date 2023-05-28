**Sell Factor Apies**



**Factor Code**

URL: http://localhost:8080/Factor/SellFactor/code

Method : Post

`<param name="code">Enter a code as int to validate</param>`

Description: 

give you a new factor code if you enter Code  as null  or 

Check your entered code to validate if Exist in database or not

Return : 

if Code = null

Status code = 200 and you can use the code ( HttpStatusCode.OK)

`{
    "statusCode": 200,
    "data": 2
}`

if Code = 2 that not exist in database

Status code = 200 and you can use the code ( HttpStatusCode.OK)

`{
    "statusCode": 200,
    "data": true
}`

if Code = 1 that exist in database

Status Code = 303 and you can not user this code ( HttpStatusCode.SeeOther)

`{
    "statusCode": 303,
    "data": null
}`

in case of Error : 

Status Code = 500 (HttpStatusCode.InternalServerError)

`{
    "statusCode": 500,
    "data": Error Message
}`



```c#

        [System.Web.Http.HttpGet]
        public JsonResult Code(int? code)
        {
            try
            {
                if (code.HasValue)
                {
                    var isExist = unitOfWork.FactorRepository.ExistCode(code.Value);
                    if (!isExist)
                    {
                        return Json(new JsonResultCustom() { statusCode = HttpStatusCode.SeeOther, data = null }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.OK, data = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var generate = unitOfWork.FactorRepository.GetMaxCode();
                    return Json(new JsonResultCustom() { statusCode = HttpStatusCode.OK, data = generate }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
```



<!-------------------------------------------------------------->

**GetCustomer**

URL : http://localhost:8080/Factor/SellFactor/GetCustomer

Method : Get 

`<param name="Value">search Value as string</param>`

Description : 

Search Customers by entered value in Customer name or Customer id 

Returns : 

if Value = 1 

 Status code = 200  ( HttpStatusCode.OK)

`{
    "statusCode": 200,
    "data": [
        {
            "id": 1,
            "name": "شایان صفایی",
            "code": 1
        }
    ]
}`

if Value = "صف" 

Status code = 200 ( HttpStatusCode.OK)

`{
    "statusCode": 200,
    "data": [
        {
            "id": 1,
            "name": "شایان صفایی",
            "code": 1
        }
    ]
}`

if value entered as some thing that not found in database 

Status code = 200 ( HttpStatusCode.OK)

`{
    "statusCode": 200,
    "data": []
}`

if you not enter value 

Status code =400 (  HttpStatusCode.BadRequest )

`{
    "statusCode": 400,
    "data": null
}`

in case of Error : 

Status Code = 500 (HttpStatusCode.InternalServerError)

`{
    "statusCode": 500,
    "data": Error Message
}`







```c#
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
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.BadRequest },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new JsonResultCustom() { statusCode = HttpStatusCode.InternalServerError, data = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
```

<!-------------------------------------------------------------->

**GetCodes**



URL: http://localhost:8080/Factor/SellFactor/GetCodes

Method : Get

<param name="codes">List of Code or Name </param>

Description : 

Get a list of Enum and return List of SiteValue(table) by entered Enum(code or name)

you can get anything that you need from site value by this API 

you just need to know Sitevalue Enums

```C#
   public enum SiteValue
        {
            productCategory,
            brand,
            province,
            productUnit,
            FactorMessage,
            SectionType,
            Discount,
            PaymentMessage,
            OptionType,
            CustomerCategory,
            OrganizationUnit,
            SellFactorStatus,
            BuyFactorStatus,
            FactorOptionValues,
            FactorPresentationType
        }
```



Return: 

if Code = codes=FactorMessage

Status code = 200 ( HttpStatusCode.OK)

`{
    "statusCode": 200,
    "data": [
        {
            "key": "پیام های فاکتور",
            "values": [
                {
                    "id": 5362,
                    "name": "اولین پیام فاکتور",
                    "parentId": 7
                },
                {
                    "id": 5363,
                    "name": "دومین پیام فاکتور",
                    "parentId": 7
                }
            ]
        }
    ]
}`

if you enter a list of enums like this : 

codes=FactorMessage&codes=productCategory



Status code = 200 ( HttpStatusCode.OK)

`{
    "statusCode": 200,
    "data": [
        {
            "key": "پیام های فاکتور",
            "values": [
                {
                    "id": 5362,
                    "name": "اولین پیام فاکتور",
                    "parentId": 7
                },
                {
                    "id": 5363,
                    "name": "دومین پیام فاکتور",
                    "parentId": 7
                }
            ]
        },
        {
            "key": "نوع کالا",
            "values": [
                {
                    "id": 5350,
                    "name": "کاغذ",
                    "parentId": 1
                }
            ]
        }
    ]
}`

if you Enter Codes as not allowed Enums like 

codes=dasdasdas

Status code =400 (  HttpStatusCode.BadRequest )

`{
    "statusCode": 400,
    "data": null
}`



in case of Error : 

Status Code = 500 (HttpStatusCode.InternalServerError)

`{
    "statusCode": 500,
    "data": Error Message
}`



```C#
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
```





****

****

****