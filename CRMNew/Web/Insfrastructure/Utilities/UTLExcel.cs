using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Web.Models.Entity;
using Web.Models.Repositories;

namespace Web.Insfrastructure.Utilities
{
    public class UTLExcel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">اسم فایل همراه با پسوند</param>
        /// <param name="path">مسیر فایل</param>
        /// <param name="context">دیتا بیس</param>
        public void ImportSiteValuesData(string fileName, string path, SiteValueRepository SVR)
        {
            var workbook = new XLWorkbook(System.Web.Hosting.HostingEnvironment.MapPath(path + fileName));
            var ws1 = workbook.Worksheet(1);
            int rowCount = ws1.RowsUsed().Count();

            List<SiteValue> list = new List<SiteValue>();
            for (int i = 2; i <= rowCount; i++)
            {
                SiteValue siteValue = new SiteValue
                {
                    id = ws1.Row(i).Cell(1).GetValue<int>(),
                    name = (ws1.Row(i).Cell(4).IsEmpty() ? "" : ws1.Row(i).Cell(4).GetValue<string>()),
                    value = (ws1.Row(i).Cell(5).IsEmpty() ? "" : ws1.Row(i).Cell(5).GetValue<string>()),
                    isDelete = ws1.Row(i).Cell(6).GetValue<bool>(),
                    isEnable = ws1.Row(i).Cell(7).GetValue<bool>(),
                    description = (ws1.Row(i).Cell(8).IsEmpty() ? "" : ws1.Row(i).Cell(8).GetValue<string>()),
                };
                if (!ws1.Row(i).Cell(3).IsEmpty())
                    siteValue.parentId = ws1.Row(i).Cell(3).GetValue<int>();

                if (!ws1.Row(i).Cell(2).IsEmpty())
                    siteValue.code = ws1.Row(i).Cell(2).GetValue<int>();

                list.Add(siteValue);
            }
            SVR.Insert(list);
        }
    }
}