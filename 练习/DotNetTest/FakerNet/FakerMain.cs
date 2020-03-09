
using Faker;
using Faker.Extensions;
namespace DotNetTest.FakerNet
{
    /// <summary>
    /// 未找到本地化方法，无法使用
    /// </summary>
    public static class FakerMain
    {
        public static void Write()
        {
            //FakerNameTest();
            FakerPhoneTest();
        }

        public static void FakerNameTest()
        {
            var firstName = Name.First();
            var lastName = Name.Last();
            var fullName = Name.FullName();
            var prefix = Name.Prefix();
            var suffix = Name.Suffix();
        }

        public static void FakerPhoneTest()
        {
            var phone = Phone.Number();
        }
    }
}