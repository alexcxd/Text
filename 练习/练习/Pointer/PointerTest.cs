using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Pointer
{
    public unsafe class PointerTest
    {
        //指针注意事项
        //指针不能指向任何引用类型

        public static void Demo1()
        {
            int* a;
            int b = 1;
            a = &b;

            //指针类型间的强制转换
            //以下转换不会出现有意义的值，因为byte只占8字节，double占64字节
            byte aByte = 8;
            byte* pByte = &aByte;
            double* pDouble = (double*)pByte;

            //void指针
            //不存在数据类型的指针
            int* pointerToInt = (int*)1;
            void* pointerToVoid;
            pointerToVoid = (void*)pointerToInt;

            //指针算术的运算
            //根据类型加，例如给int类型的指针加1，会给该值加上4个字节
            //void 指针无法进行运算
            int bInt = 1;
            int* pointerToInt1 = &bInt;
            //加了4字节
            pointerToInt1 += 1;
            //加了4字节
            pointerToInt1++;
            //只加了2字节  ？？
            int* result = pointerToInt1 + 2;

            //sizeof可以获取某个类型的大小
            //只能对值类型使用
            int x1 = sizeof(byte);
            int x7 = sizeof(short);
            int x2 = sizeof(int);
            int x3 = sizeof(long);
            int x4 = sizeof(float);
            int x5 = sizeof(double);
            int x6 = sizeof(char);

            

            Console.ReadKey();
        }

        struct MyStruct
        {
            public long x;
            public float F;
        }

        /// <summary>
        /// 引用类型的指针
        /// </summary>
        public static void Demo2()
        {

            //结构指针
            MyStruct aStruct = new MyStruct();
            MyStruct* pStruct = &aStruct;
            (*pStruct).F = 3.4F;
            (*pStruct).x = 11;

            pStruct->F = 3.5F;
            pStruct->x = 2;

            long* pLong = &(pStruct->x);


            //类成员指针
            Test myClass = new Test();
            /*//会产生编码错误，以下写法
            long* pLong1 = &(myClass.X);*/

            fixed(float* pFloat1 = &(myClass.F))
            fixed(long* pLong1 = &(myClass.X))
            {

            }
        }

        /// <summary>
        /// 指针优化性能
        /// </summary>
        public static void Demo3()
        {
            //创建基于栈的数组
            //关键字stackalloc,在栈上分配一定量的内存
            int* pDecimals = stackalloc int[10];

            *pDecimals = 1;
            var address = pDecimals + 1;
            * (pDecimals + 1) = 2;

            //注意堆栈溢出，pDecimals[100]
            pDecimals[2] = 3;
            pDecimals[3] = 4;
        }
    }
    class Test
    {
        public long X;
        public float F;
    }
}
