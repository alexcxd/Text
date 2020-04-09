using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DotNetTest.AdoTest
{
    public static class BulkCopyTest
    {

        #region 批量新增数据

        /// <summary>
        /// 批量新增数据
        /// </summary>
        public static void BulkCopy(IList<OrderIndexTest> datas)
        {
            var dt = GetTableSchema1();
            using (var conn = new SqlConnection("server=.;Database=TSQL2012;Integrated Security=SSPI;"))
            {
                var bulkCopy = new SqlBulkCopy(conn)
                {
                    DestinationTableName = "OrderIndexTest", 
                    BatchSize = dt.Rows.Count
                };
                conn.Open();
                for (int i = 0; i < datas.Count; i++)
                {
                    var data = datas[i];
                    var dr = dt.NewRow();

                    dr[1] = data.AddTime;
                    dr[2] = data.OrderNo;
                    dr[3] = data.OrderType;
                    dr[4] = data.OrderState;
                    dr[5] = data.OrderMoney;
                    dr[6] = data.Remark;
                    dr[7] = data.ProductId;
                    dr[8] = data.PayTime;
                    dr[9] = data.NoUse1;
                    dr[10] = data.NoUse2;
                    dr[11] = data.NoUse3;
                    dr[12] = data.NoUse4;
                    dr[13] = data.NoUse5;

                    dt.Rows.Add(dr);
                }
                bulkCopy.WriteToServer(dt);
            }
        }

        static DataTable GetTableSchema1()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id", typeof(int)),
                new DataColumn("AddTime", typeof(DateTime)),
                new DataColumn("OrderNo", typeof(string)),
                new DataColumn("OrderType", typeof(int)),
                new DataColumn("OrderState", typeof(int)),
                new DataColumn("OrderMoney", typeof(decimal)),
                new DataColumn("Remark", typeof(string)),
                new DataColumn("ProductId", typeof(int)),
                new DataColumn("PayTime", typeof(DateTime)),
                new DataColumn("NoUse1", typeof(string)),
                new DataColumn("NoUse2", typeof(string)),
                new DataColumn("NoUse3", typeof(string)),
                new DataColumn("NoUse4", typeof(string)),
                new DataColumn("NoUse5", typeof(string)),
            });

            return dt;
        }

        #endregion

        #region 批量新增数据 泛型


        /// <summary>
        /// 批量新增数据
        /// </summary>
        public static void BulkCopy<T>(IList<T> datas)
        {
            var dt = GetTableSchema1<T>();
            var type = typeof(T);
            var fields = type.GetProperties();

            using (var conn = new SqlConnection("server=.;Database=TSQL2012;Integrated Security=SSPI;"))
            {
                var bulkCopy = new SqlBulkCopy(conn)
                {
                    DestinationTableName = "OrderIndexTest",
                    BatchSize = dt.Rows.Count
                };
                foreach (var filed in fields)
                {
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(filed.Name, filed.Name));
                }

                conn.Open();
                for (var i = 0; i < datas.Count; i++)
                {
                    var data = datas[i];
                    var dr = dt.NewRow();

                    for (var j = 0; j < fields.Length; j++)
                    {
                        var filed = fields[j];
                        var fi = type.GetProperty(filed.Name);
                        dr[j] = fi.GetValue(data);
                    }
                    dt.Rows.Add(dr);
                }
                bulkCopy.WriteToServer(dt);
            }
        }

        public static DataTable GetTableSchema1<T>()
        {
            var dt = new DataTable();
            var type = typeof(T);
            var fields = type.GetProperties();
            foreach (var filed in fields)
            {
                dt.Columns.Add(filed.Name, filed.PropertyType);
            }

            return dt;
        }
        #endregion
    }

    public class OrderIndexTest
    {
        public int Id { get; set; }

        public DateTime AddTime { get; set; }

        public string OrderNo { get; set; }

        public int OrderType { get; set; }

        public int OrderState { get; set; }

        public decimal OrderMoney { get; set; }

        public string Remark { get; set; }

        public int ProductId { get; set; }

        public DateTime PayTime { get; set; }

        public string NoUse1 { get; set; }

        public string NoUse2 { get; set; }

        public string NoUse3 { get; set; }

        public string NoUse4 { get; set; }

        public string NoUse5 { get; set; }
    }
}