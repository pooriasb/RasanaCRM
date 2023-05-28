using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Insfrastructure.Utilities.DataTable
{
    /// <summary>
    /// test
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DataTableModels<TEntity>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IEnumerable<TEntity> data { get; set; }

    }
}