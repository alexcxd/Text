using System;
using System.Collections;

namespace DotNetTest.Gather
{
    /// <summary>
    /// BitVector32和BitArray的区别在于
    /// 1.BitArray可以改变位数
    /// 2.BitVector32是值类型且效率较高
    /// </summary>
    public class BitArrayTest
    {
        /// <summary>
        /// BitArray基本操作
        /// </summary>
        public static void BitArrayTest1()
        {
            var bitArray1 = new BitArray(16);
            bitArray1.SetAll(true);
            bitArray1.Set(1, false);
            bitArray1[5] = false;
            bitArray1[7] = false;

            Console.Write("原位数组1：");
            DisplayBits(bitArray1);

            var bitArray2 = new BitArray(16);
            bitArray2.SetAll(true);
            bitArray2.Set(0, false);
            bitArray2[1] = false;
            bitArray2[4] = false;

            Console.Write("原位数组2：");
            DisplayBits(bitArray2);

            //取反操作
            /*Console.Write("位数组1取反操作后：");
            bitArray1.Not();
            DisplayBits(bitArray1);*/

            //以下操作会修改本身，即修改bitArray,因此使用克隆的方式解决

            //与操作
            var bitArray = bitArray2.Clone() as BitArray ?? new BitArray(16, true);
            Console.Write("位数组1和位数组2与操作后：");
            var bitArray3 = bitArray.And(bitArray1);
            DisplayBits(bitArray3);

            //或操作
            bitArray = bitArray2.Clone() as BitArray ?? new BitArray(16, true);
            Console.Write("位数组1和位数组2或操作后：");
            var bitArray4 = bitArray.Or(bitArray1);
            DisplayBits(bitArray4);

            //异或操作
            bitArray = bitArray2.Clone() as BitArray ?? new BitArray(16, true);
            Console.Write("位数组1和位数组2异或操作后：");
            var bitArray5 = bitArray.Xor(bitArray1);

            //CopyTo
            //使用CopyTo和不同的构造函数实现克隆,
            //会出现问题，例如一个2位的BitArray如果使用byte[]或int[]进行初始化，会导致位数不同
            //克隆推荐使用Clone
            DisplayBits(bitArray5);

            var arrayBool = new bool[bitArray2.Length];
            bitArray2.CopyTo(arrayBool, 0);
            var bitArray6 = new BitArray(arrayBool);

            var arrayByte = new byte[GetArrayLength(bitArray2.Length, 8)];
            bitArray2.CopyTo(arrayByte, 0);
            var bitArray7 = new BitArray(arrayByte);

            var arrayInt = new int[GetArrayLength(bitArray2.Length, 32)];
            bitArray2.CopyTo(arrayInt, 0);
            var bitArray8 = new BitArray(arrayInt);
        }

        /// <summary>
        /// BitVector32基本操作
        /// </summary>
        public static void BitArrayTest2()
        {
            
        }

        public static void DisplayBits(BitArray bitArray)
        {
            foreach (bool b in bitArray)
            {
                Console.Write(b ? 1 : 0);
            }
            Console.WriteLine();
        }

        private static int GetArrayLength(int n, int div)
        {
            if (n <= 0)
            {
                return 0;
            }
            return (n - 1) / div + 1;
        }
    }
}