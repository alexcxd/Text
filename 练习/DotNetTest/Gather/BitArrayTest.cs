using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

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
            var bv1 = new BitVector32();

            //CreateMask为参数时默认返回1，有参数时返回参数左移一位后的结果
            //用于创建访问特定位的掩码
            var bit1 = BitVector32.CreateMask();
            var bit2 = BitVector32.CreateMask(bit1);
            var bit3 = BitVector32.CreateMask(bit2);
            var bit4 = BitVector32.CreateMask(bit3);
            var bit5 = BitVector32.CreateMask(bit4);

            //[]中的参数为掩码，
            //若等于true则将掩码和bv中32位二进制进行或操作
            //若等于false则将掩码和bv中32位二进制进行与操作
            bv1[bit1] = true;
            bv1[bit2] = false;
            bv1[bit3] = true;
            bv1[bit4] = true;
            bv1[bit5] = true;
            Console.WriteLine(bv1);

            //将 0000 0000 0000 0000 0001 1101 和 1010 1011 1100 1101 1110 1111进行或操作
            bv1[0xabcdef] = true;
            Console.WriteLine(bv1);

            var received = 0x79abcdef;
            var bv2 =  new BitVector32(received);
            Console.WriteLine(bv2);

            //CreateSection用于创建
            var sectionA = BitVector32.CreateSection(0xfff);
            var sectionB = BitVector32.CreateSection(0xff, sectionA);
            var sectionC = BitVector32.CreateSection(0xf, sectionB);
            var sectionD = BitVector32.CreateSection(0x7, sectionC);
            var sectionE = BitVector32.CreateSection(0x7, sectionD);
            var sectionF = BitVector32.CreateSection(0x3, sectionE);

            Console.WriteLine($"SectionA:{IntToBinaryString(bv2[sectionA])}");
            Console.WriteLine($"SectionB:{IntToBinaryString(bv2[sectionB])}");
            Console.WriteLine($"SectionC:{IntToBinaryString(bv2[sectionC])}");
            Console.WriteLine($"SectionD:{IntToBinaryString(bv2[sectionD])}");
            Console.WriteLine($"SectionE:{IntToBinaryString(bv2[sectionE])}");
            Console.WriteLine($"SectionF:{IntToBinaryString(bv2[sectionF])}");
        }

        public static void DisplayBits(BitArray bitArray)
        {
            foreach (bool b in bitArray)
            {
                Console.Write(b ? 1 : 0);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 通过类型位数获得所系数组容量大小
        /// </summary>
        /// <param name="n">字节数组长度</param>
        /// <param name="div">类型长度</param>
        /// <returns></returns>
        private static int GetArrayLength(int n, int div)
        {
            if (n <= 0)
            {
                return 0;
            }
            return (n - 1) / div + 1;
        }
        
        /// <summary>
        /// 将十进制装换为二进制的方法
        /// </summary>
        /// <param name="bits">十进制</param>
        /// <param name="removeTrailingZere">是否移除0</param>
        /// <returns>二进制</returns>
        public static string IntToBinaryString(int bits, bool removeTrailingZere = true)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < 32; i++)
            {
                sb.Append((bits & 0x80000000) != 0 ? "1" : "0");
                bits = bits << 1; 
            }

            var s = sb.ToString();
            return removeTrailingZere ? s.TrimStart('0') : s;
        }
    }
}