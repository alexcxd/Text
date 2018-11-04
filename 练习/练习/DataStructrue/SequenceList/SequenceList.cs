using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 练习.DataStructrue.SequenceList
{
    /// <summary>
    /// 线性表之顺序表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class SequenceList<T>
    {
        private T[] array;                        //维护的数组
        private int currLength;                     //当前实际长度
        private int count;                      //数组内元素的个数

        /// <summary>
        /// 实现索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                if (index > count) throw new Exception("索引越界");
                return array[index];
            }
            set
            {
                //顺序表原则上两个元素间要相邻
                //这里为了方便，直接给到最后的位置
                if (index - 1 > 0 && array[index - 1] == null)
                {
                    index = count;
                }

                //超限
                if (index >= currLength)
                {
                    EnsureCapacity();
                }

                array[index] = value;
                count++;
            }
        }

        /// <summary>
        /// 扩容
        /// </summary>
        public void EnsureCapacity()
        {
            T[] currArray = new T[currLength * 2];
            array.CopyTo(currArray, 0);
            array = currArray;
            currLength = currLength * 2;
        }

        /// <summary>
        /// 初始化顺序表(自定义长度)(默认10)
        /// </summary>
        /// <returns></returns>
        public void InitList(int length = 10)
        {
            if (array != null) return;
            array = new T[length];
            currLength = length;
            count = 0;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void DestroyList()
        {
            if (array == null) throw new ArgumentNullException();
            array = null;
            count = 0;
            currLength = 0;
        }

        /// <summary>
        /// 清空顺序表
        /// </summary>
        public void ClearList()
        {
            if (array == null) throw new ArgumentNullException();
            InitList();
        }

        /// <summary>
        /// 当前顺序表内元素个数是否为0
        /// </summary>
        /// <returns></returns>
        public bool ListEntry()
        {
            if (array == null) throw new ArgumentNullException();
            return count == 0;
        }

        /// <summary>
        /// 顺序表中的元素个数
        /// </summary>
        /// <returns></returns>
        public int ListLength()
        {
            return count;
        }

        /// <summary>
        /// 返回某个元素的位置，若不存在返回-1
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public int LocateElem(T e)
        {
            for(int i = 0; i < count; ++i)
            {
                if (array[i].Equals(e))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 添加一个元素到尾部
        /// </summary>
        /// <param name="element"></param>
        public void Add(T element)
        {
            //超限
            if (count >= currLength)
            {
                EnsureCapacity();
            }

            array[count] = element;
            count++;
        }

        /// <summary>
        /// 移除一个元素，并保证两两相邻
        /// </summary>
        /// <param name="element"></param>
        public void Remove(T element)
        {
            var temp = false;
            for(int i = 0; i < count; ++i)
            {
                if (array[i].Equals(element))
                {
                    temp = true;
                }
                if (temp)
                {
                    array[i - 1] = array[i];
                }
            }
        }

        /// <summary>
        /// 将某一元素插入到指定位置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="elememt"></param>
        public void PriorElem(int index, T elememt)
        {
            if (index <= 0 || index > count) throw new Exception("超出索引界限");

            count++;
            if(count >= currLength)
            {
                EnsureCapacity();
            }

            T lastEle;
            for (int i = index - 1; i < count; ++i)
            {
                lastEle = array[i];
                if(i == index)
                {
                    array[i] = elememt;
                }
                else
                {
                    array[i] = lastEle;
                }
            }
        }
    }
}
