using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Linq
{
    class LinqMain
    {
        public void Write()
        {
            LinqBasis linqBasis = new LinqBasis();

            //linqBasis.GroupByTest();
            //linqBasis.ZipTest();
            //linqBasis.AggregateFunctionTest();
            linqBasis.ParalleTest();

        }
    }
}
