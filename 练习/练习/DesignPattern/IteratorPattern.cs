using System.Collections.Generic;

namespace 练习.DesignPattern
{
    public class IteratorPattern
    {
        /// <summary>
        /// 迭代器模式
        /// </summary>
        /// 分离集合对象的遍历行为
        /// 应用：聚集对象需要迭代时,举例：IEnumerable接口
        public static void IteratorPatternMain()
        {
            List<int> intList = new List<int>(){1,2,3,4,5,6,7,8,9};
            var myEnumerable = GetEnumerable(intList);
        }

        public static IEnumerable<int> GetEnumerable(List<int> intList)
        {
            for (int i = 0; i < intList.Count; i++)
            {
                yield return intList[i];
            }
        }
    }
}