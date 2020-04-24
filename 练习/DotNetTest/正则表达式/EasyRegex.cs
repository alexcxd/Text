using System;
using System.Text.RegularExpressions;

namespace DotNetTest.正则表达式
{
    public class EasyRegex
    {
        //Regex.IsMatch();//用来判断给定的字符串是否匹配某个正则表达式
        //Regex.Match();//用来从给定的字符串中按照正则表达式的要求提取【一个】匹配的字符串
        //Regex.Matches();//用来从给定的字符串中按照正则表达式的要求提取【所有】匹配的字符串
        //Regex.Replace(); //替换所有正则表达式匹配的字符串为另外一个字符串。

        public void demo1()
        {
            string str = "Is is the cost of 12323 of gasoline 32424 2 going up up";
            string patt = "[0-9]+";
            var list = Regex.Matches(str, patt);
            foreach (Match match in list)
            {
                var item = match.Value;
            }

            //1.请用户输入一个10（含）-25（含）之间的任何一个数字
            patt = "^(10|[0-9])|(-1[0-9]|2[0-5])$";
            bool isNum = Regex.IsMatch("-29", patt);
            Console.WriteLine("1:" + isNum);

            //2.验证是否是合法的手机号
            patt = "^1[0-9]{10}$";
            bool isPhone = Regex.IsMatch("18158510455", patt);
            Console.WriteLine("2:" + isPhone);

            //3.身份证号码
            patt = @"^\d{18}$";
            bool isCard = Regex.IsMatch("111111111111111111", patt);
            Console.WriteLine("3:" + isCard);
            //4.验证是否为合法的邮件地址 24@qq
            patt = @"^[-.0-9A-Za-z_]+@[0-9A-Za-z_]+.com$";
            //patt = @"^[-0-9a-zA-Z_\.]+@[a-zA-Z0-9]+(\.[a-zA-Z]+){1,2}$";
            bool isEmail = Regex.IsMatch("asd@adw.com", patt);
            Console.WriteLine("4:" + isEmail);

            //5.匹配IP地址，4段用.分割的最多三位数字
            patt = "^(\\d{1,3}.){3}\\d{1,3}$";
            bool isIP = Regex.IsMatch("1.1.1.1", patt);
            Console.WriteLine("5:" + isIP);

            //6.判断是否是合法的日期格式“2008-08-08”。四位数字-两位数字-两位数字
            patt = "^\\d{1,4}((-0[13578]|10|12)(-[0-2][0-9]|3[0-1]))|((-0[469]|11)(-[0-2][0-9]|30))|(-02(-0[0-9]|2[0-8]))";
            bool isDate = Regex.IsMatch("2008-02-29", patt);
            Console.WriteLine("5:" + isDate);

            //7.判断是否是合法的url地址

            //8.Html判断
            patt = @"<p.*?>(?<text>.*?)</p>";
            string value = "<p>前不久在哔哩哔哩上看到一个动画短片，看完以后整颗心都被融化，还特地下载，每看一遍就让我感受到温暖。</p><p>看完后一直一直都在回味着，再看第n多遍时才知道原来是奥斯卡金奖。</p><a class='video - box' href='https://link.zhihu.com/?target=https%3A//www.zhihu.com/video/1005194352003403776' target='_blank' data-video-id='' data-video-playable='' data-name='鹬（四声yu） ' data-poster='https://pic2.zhimg.com/v2-d90826ccb66ea6f407b54d37dd9a472d.jpg' data-lens-id='1005194352003403776'>              <img class='thumbnail' src='https://pic2.zhimg.com/v2-d90826ccb66ea6f407b54d37dd9a472d.jpg'>              <span class='content'>                <span class='title'>鹬（四声yu） <span class='z-ico-extern-gray'></span><span class='z-ico-extern-blue'></span></span>                <span class='url'><span class='z-ico-video'></span>https://www.zhihu.com/video/1005194352003403776</span>              </span>            </a>            <p><br></p><p><br></p><p>b站：av12645301</p><p><br></p><hr><p>还有一个男票的回答，也回答过类似的问题，细节也处理的特别好，感兴趣可以去看看。<a href='https://www.zhihu.com/question/279247693/answer/452678879' class='internal'><span class='invisible'>https://www.</span><span class='visible'>zhihu.com/question/2792</span><span class='invisible'>47693/answer/452678879</span><span class='ellipsis'></span></a></p></span></div><div><div class='ContentItem-time'><a target='_blank' href='/question/277163979/answer/451070201'><span data-tooltip='发布于 2018-07-24 21:04'>编辑于 2018-07-27</span></a></div></div><div class='ContentItem-actions RichContent-actions'><span><button aria-label='赞同' type='button' class='Button VoteButton VoteButton--up'><span style='display:inline-flex;align-items:center'>​<svg class='Zi Zi--TriangleUp VoteButton-TriangleUp' fill='currentColor' viewBox='0 0 24 24' width='10' height='10'><path d='M2 18.242c0-.326.088-.532.237-.896l7.98-13.203C10.572 3.57 11.086 3 12 3c.915 0 1.429.571 1.784 1.143l7.98 13.203c.15.364.236.57.236.896 0 1.386-.875 1.9-1.955 1.9H3.955c-1.08 0-1.955-.517-1.955-1.9z' fill-rule='evenodd'></path></svg></span>赞同 <!-- -->15K</button>";
            var isHtml = Regex.Matches(value, patt);
            foreach (Match match in isHtml)
            {
                Console.WriteLine(match.Groups["text"].Value);
            }

            //将hello ‘welcome’ to ‘China’   替换成 hello 【welcome】 to 【China】
            string msg = "hello 'welcome' to 'China'  'lss'   'ls'   'szj'   ";
            msg = Regex.Replace(msg, "'(.+?)'", "【$1】$$1");//2个$$代表一个，转义了
            Console.WriteLine(msg);

            msg = "xxx13409876543yyy18276354908aa87654321345bb98761234654";
            msg = Regex.Replace(msg, @"([0-9]{3})[0-9]{4}([0-9]{4})", "$1****$2");
            Console.WriteLine(msg);

        }
    }
}