namespace DotNetTest.BugTest
{
    public class Main
    {
        public void WriteLine()
        {

            //GetSet_StackOverflowException.cs
           /* StackOverflowBugTest stackOverflowBugTest = new StackOverflowBugTest();
            stackOverflowBugTest.Demo1();*/
            ShallowCopy.ShallowCopyMain();
        }
    }
}