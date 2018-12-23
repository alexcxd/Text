namespace 练习.DataStructrue.String
{
    /*
     * 原理：通过对模式串进行预处理（）再进行匹配
     * 
     * 
     */


    /// <summary>
    /// KMP算法（字符匹配算法）
    /// </summary>
    public class KmpAlgorithm
    {
        /// <summary>
        /// 获取next数组
        /// </summary>
        /// <param name="ps">模式串</param>
        /// <returns></returns>
        public static int[] GetNext(string ps)
        {
            char[] p = ps.ToCharArray();
            int[] next = new int[p.Length];

            next[0] = -1;
            int j = 0;
            int k = -1;

            while (j < p.Length - 1)
            {
                if (k == -1 || p[j] == p[k])
                {
                    next[++j] = ++k;
                }
                else
                {
                    k = next[k];
                }
            }
            return next;
        }
    }
}