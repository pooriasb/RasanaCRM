using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Employee.Models.ViewModels
{
    public class DataTableViewModel
    {
        //null
        public class DataItem
        {
            public string customerName { get; set; }
            public int factorId { get; set; }
            public string factorCode { get; set; }
            public string dateTime { get; set; }
            public string totalPrice { get; set; }
        }

        public class DataTableData
        {
            public int draw { get; set; }
            public int recordsTotal { get; set; }
            public int recordsFiltered { get; set; }
            public List<DataItem> data { get; set; }
        }
    }
}