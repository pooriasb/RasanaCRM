using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ViewModels.Enums
{
    public class Enums
    {
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
        public enum OptionType
        {
            DropDown,
            String,
        }
        public enum SiteValueFieldJson
        {
            isRequired,
            OptionType,
            FieldDynamic
        }
        //public struct SiteValues3
        //{
        //    public int DropDown { get { return 4; } }
        //}
        public enum WorkFlowStatus
        {
            DoNotRead = 1,
            Read = 2,
            Agree = 3,
            Deny = 4,
            Referral=5
        }
        public enum SectionType
        {
            Product,
            Factor,
        }
        public enum Log
        {
            Error = 1,
            Ajax = 2,
            ActionResult = 3,
        }
        public enum Factor
        {
            Draft=1,
            PreFactor
        }
    }
}