using System;
using System.Collections.Generic;

namespace SampleCode.DesignPattern.OtherPatterns.Specification
{
    /// <summary>
    /// 规约模式-声明式实现
    /// 使用逻辑运算对"SPECIFICATION"进行组合，逻辑运算指"AND"、"OR"、"NOT"等。
    /// </summary>
    public class CompositeSpecificationPattern
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        public void CompositeSpecificationPatternMain()
        {
            var invoiceList = new List<Invoice>()
            {
                new Invoice(1, DateTime.Today.AddDays(1)),
                new Invoice(10, DateTime.Today.AddDays(1)),
                new Invoice(1, DateTime.Today.AddDays(-1)),
                new Invoice(10, DateTime.Today.AddDays(-1)),
            };

            var dateSpecification = new DateInvoiceSpecification(DateTime.Today);
            var amountSpecification = new AmountInvoiceSpecification(5);
            var specification = dateSpecification.And(amountSpecification);

            foreach (var invoice in invoiceList)
            {
                if (invoice.IsSatisfiedBy(specification))
                {
                    // 验证是否会进入
                }
            }
        }


        #region 规约模式-声明式实现

        /// <summary>
        /// 规约接口的定义
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface ISpecification<T> where T : class
        {
            /// <summary>
            ///  验证逻辑 
            /// </summary>
            /// <param name="param"></param>
            /// <returns></returns>
            bool IsSatisfiedBy(T param);
            ISpecification<T> And(ISpecification<T> other);
            ISpecification<T> Or(ISpecification<T> other);
            ISpecification<T> Not();
        }

        /// <summary>
        /// 规约具体实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract class CompositeSpecification<T> : ISpecification<T> where T : class
        {
            public ISpecification<T> And(ISpecification<T> other)
            {
                return new AndSpecification<T>(this, other);
            }

            public ISpecification<T> Not()
            {
                return new NotSpecification<T>(this);
            }

            public ISpecification<T> Or(ISpecification<T> other)
            {
                return new OrSpecification<T>(this, other);
            }

            /// <summary>
            /// 验证逻辑
            /// </summary>
            /// <param name="param"></param>
            /// <returns></returns>
            public abstract bool IsSatisfiedBy(T param);
        }

        /// <summary>
        /// AND实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class AndSpecification<T> : CompositeSpecification<T> where T : class
        {

            private readonly ISpecification<T> b;
            private readonly ISpecification<T> a;

            public AndSpecification(ISpecification<T> a, ISpecification<T> b)
            {
                this.a = a;
                this.b = b;
            }


            public override bool IsSatisfiedBy(T param)
            {
                return a.IsSatisfiedBy(param) && b.IsSatisfiedBy(param);
            }
        }

        /// <summary>
        /// OR实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class OrSpecification<T> : CompositeSpecification<T> where T : class
        {

            private readonly ISpecification<T> b;
            private readonly ISpecification<T> a;

            public OrSpecification(ISpecification<T> a, ISpecification<T> b)
            {
                this.a = a;
                this.b = b;
            }


            public override bool IsSatisfiedBy(T param)
            {
                return a.IsSatisfiedBy(param) || b.IsSatisfiedBy(param);
            }
        }

        /// <summary>
        /// NOT实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class NotSpecification<T> : CompositeSpecification<T> where T : class
        {

            private readonly ISpecification<T> a;

            public NotSpecification(ISpecification<T> a)
            {
                this.a = a;
            }


            public override bool IsSatisfiedBy(T param)
            {
                return !a.IsSatisfiedBy(param);
            }
        }

        /// <summary>
        /// 大于指定日期
        /// </summary>
        public class DateInvoiceSpecification : CompositeSpecification<Invoice>
        {
            private DateTime currentDate;

            public DateInvoiceSpecification(DateTime currentDate)
            {
                this.currentDate = currentDate;
            }

            /// <summary>
            /// 验证逻辑 - 业务对象时间是否大于规约时间
            /// </summary>
            /// <param name="candidate"></param>
            /// <returns></returns>
            public override bool IsSatisfiedBy(Invoice candidate)
            {
                return candidate.DueDate > currentDate;
            }
        }


        /// <summary>
        /// 大于指定金额
        /// </summary>
        public class AmountInvoiceSpecification : CompositeSpecification<Invoice>
        {
            private readonly int amount;

            public AmountInvoiceSpecification(int amount)
            {
                this.amount = amount;
            }

            /// <summary>
            /// 验证逻辑 - 业务对象时间是否大于规约时间
            /// </summary>
            /// <param name="candidate"></param>
            /// <returns></returns>
            public override bool IsSatisfiedBy(Invoice candidate)
            {
                return candidate.Amount > amount;
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

            public bool IsSatisfiedBy(ISpecification<Invoice> specification)
            {
                return specification.IsSatisfiedBy(this);
            }
        }

        #endregion
    }
}