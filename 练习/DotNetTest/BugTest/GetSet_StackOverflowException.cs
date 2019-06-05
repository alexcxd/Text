namespace DotNetTest.BugTest
{
    public class StackOverflowBugTest
    {
        public void Demo1()
        {
            Test test = new Test();
            test.Boolean = true;
            test.Str1 = "Str1";
            test.Str = "Str";
        }
    }

    public class Test
    {
        //解决方案
        private string str;
        public string Str
        {
            get { return str; }
            set
            {
                if (Boolean)
                {
                    str = value;
                }
                else
                {
                    str = Str1;
                }
            }
        }

        public string Str1 { get; set; }

        public string str2 { get; set; }

        public bool Boolean { get; set; }
    }
}