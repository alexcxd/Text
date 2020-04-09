using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetTest.AdoTest
{
    public static class AdoMainTest
    {
        public static void Write()
        {

            var orderList = new List<OrderIndexTest>();

            var dateTimeFirst = new DateTime(2019, 11, 20);
            var random = new System.Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            for (var i = 0; i < 1; i++)
            {
                if (i % 6000 == 0)
                {
                    dateTimeFirst = dateTimeFirst.AddDays(1);
                    Console.WriteLine(i);
                }

                var addTime = random.Next(0, 86400);
                var payTime = random.Next(0, 86400 * 4 - 1);
                var orderType = random.Next(0, 2);
                var orderState = random.Next(0, 3);
                var orderMoneyInteger = random.Next(1, 10000);
                var orderMoneyDecimal = random.NextDouble();
                var productId = random.Next(1, 4350);

                if (payTime > 86400 * 3)
                {
                    payTime = 86400 * 3;
                }

                orderList.Add(new OrderIndexTest
                {
                    AddTime = dateTimeFirst.AddSeconds(addTime),
                    OrderNo = GetNewNo("ts") + i,
                    OrderType = orderType,
                    OrderState = orderState,
                    OrderMoney = (decimal)(orderMoneyInteger + orderMoneyDecimal),
                    Remark = $"生成时间 {dateTimeFirst}",
                    ProductId = productId,
                    PayTime = dateTimeFirst.AddSeconds(addTime + payTime),
                    NoUse1 = "",
                    NoUse2 = "",
                    NoUse3 = "",
                });
            }

            orderList = orderList.OrderBy(x => x.AddTime).ToList();

            BulkCopyTest.BulkCopy<OrderIndexTest>(orderList);
        }


        public static string GetNewNo(string head)
        {
            var ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var totalSeconds = Convert.ToInt64(ts.TotalSeconds);
            var fullNo = $"{head}{DateTime.Now.Year.ToString().Substring(2, 2)}{totalSeconds}";
            return fullNo;
        }
    }
}