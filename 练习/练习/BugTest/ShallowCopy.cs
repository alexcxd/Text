namespace 练习.BugTest
{
    public class ShallowCopy
    {
        /// <summary>
        /// 浅拷贝
        /// </summary>
        public static void ShallowCopyMain()
        {
            Model modelA = new Model(1,"cxd",new Model(2,"mm"));

            //浅拷贝，会导致modelA和modelB中的引用类型一致
            Model modelB = modelA;
            modelB.ChildModel = new Model(3,"tt");
        }
    }

    class Model
    {
        public Model() { }

        public Model(int num, string str, Model childModel)
        {
            Num = num;
            Str = str;
            ChildModel = childModel;
        }
        public Model(int num, string str)
        {
            Num = num;
            Str = str;
        }

        public int Num { get; set; }
        public string Str { get; set; }
        public Model ChildModel { get; set; }
    }
}