using NUnit.Framework;

namespace SampleCode.Test.Pointer
{
    /// <summary>
    /// 指针
    /// 指针注意事项
    /// 指针不能指向任何引用类型
    /// </summary>
    public unsafe class PointerTest
    {
        /// <summary>
        /// 值类型的指针
        /// </summary>
        [Test]
        public void PointerValueTest()
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

        }

        private struct MyStruct
        {
            public long x;
            public float F;
        }

        /// <summary>
        /// 引用类型的指针
        /// </summary>
        [Test]
        public void PointerReferenceTest()
        {
            //结构指针
            var aStruct = new MyStruct();
            var pStruct = &aStruct;
            (*pStruct).F = 3.4F;
            (*pStruct).x = 11;

            pStruct->F = 3.5F;
            pStruct->x = 2;

            var pLong = &(pStruct->x);

            //类成员指针
            var myClass = new Test();
            /*//会产生编码错误，以下写法
            long* pLong1 = &(myClass.X);*/

            fixed (float* pFloat1 = &(myClass.F))
            fixed (long* pLong1 = &(myClass.X))
            {

            }
        }
        private class Test
        {
            public long X;
            public float F;
        }
    }
}