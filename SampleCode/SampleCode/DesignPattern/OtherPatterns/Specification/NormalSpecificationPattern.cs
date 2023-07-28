using System;
using System.Collections.Generic;

namespace SampleCode.DesignPattern.OtherPatterns.Specification
{
    /// <summary>
    /// 规约模式-普通实现
    /// 通过接口"Invoice Specification"抽象类"Invoice"的验证规则，需要什么规则，直接实现并调用
    /// </summary>
    public class NormalSpecificationPattern
    {
        /// <summary>
        /// 验证方法
        /// </summary>
        public void NormalSpecificationPatternMain()
        {
            var invoiceList = new List<Invoice>()
            {
                new Invoice(DateTime.Today.AddDays(1)),
                new Invoice(DateTime.Today.AddDays(-1)),
            };

            var specification = new DelinquentInvoiceSpecification(DateTime.Today);
            foreach (var invoice in invoiceList)
            {
                if (invoice.IsSatisfiedBy(specification))
                {
                    // 验证是否会进入
                }
            }
        }


        #region 普通规约模式

        /// <summary>
        /// 规约接口的定义
        /// </summary>
        public interface IInvoiceSpecification
        {
            bool IsSatisfiedBy(Invoice candidate);
        }

        /// <summary>
        /// 规约具体实现
        /// </summary>
        public class DelinquentInvoiceSpecification : IInvoiceSpecification
        {
            private DateTime currentDate;

            public DelinquentInvoiceSpecification(DateTime currentDate)
            {
                this.currentDate = currentDate;
            }

            /// <summary>
            /// 验证逻辑 - 业务对象时间是否大于规约时间
            /// </summary>
            /// <param name="candidate"></param>
            /// <returns></returns>
            public bool IsSatisfiedBy(Invoice candidate)
            {
                return candidate.DueDate > currentDate;
            }
        }

        /// <summary>
        /// 业务对象
        /// </summary>
        public class Invoice
        {
            #region 构造

            public Invoice() { }

            public Invoice(DateTime dueDate) { DueDate = dueDate; }

            public Invoice(int amount, DateTime dueDate)
            {
                DueDate = dueDate;
                Amount = amount;
            }

            #endregion

            public int Amount { get; set; }
            public DateTime DueDate { get; set; }

            public bool IsSatisfiedBy(IInvoiceSpecification specification)
            {
                return specification.IsSatisfiedBy(this);
            }
        }

        #endregion
    }
}