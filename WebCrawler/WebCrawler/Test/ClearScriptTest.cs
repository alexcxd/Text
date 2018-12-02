using System;
using System.Collections.Generic;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace WebCrawler
{
    public class ClearScriptTest
    {
        public static void CleaScriptTestMain()
        {
            using (var engine = new V8ScriptEngine("debug-v8engine",
                    V8ScriptEngineFlags.EnableDebugging, 9222)
                ) // 这边定义一个变量engine  生成一个v8引擎  用来执行js脚本
                // 里面的参数9222为调试端口， V8ScriptEngineFlags.EnableDebugging 是否启用调试模式
                // V8ScriptEngineFlags.AwaitDebuggerAndPauseOnStart  异步停止或开始等待调试
                // debug-v8engine  调试引擎模式
            {
                engine.AddHostType("Console", typeof(Console));
                //添加主机的模式。以便js可以调用主机这边的一些c#类型或、对象
                 engine.AddHostType("tt", typeof(List<string>));
                // execute 开始执行代码 ， 里面填写js代码
                var str1 = @"var a = 3;
                                 var b = 5;
                                 function add(a, b)
                                 {
                                     return a + b;
                                 }
                                 var result = add(a, b)
                               Console.WriteLine(result);";
                var str = @"var page = '';
                            eval(function (p, a, c, k, e, d) {
                                e = function (c) {
                                    return (c < a ? '' : e(parseInt(c / a))) + ((c = c % a) > 35 ? String.fromCharCode(c + 29) : c.toString(36))
                                };
                                if (!''.replace(/^/, String)) {
                                    while (c--) {
                                        d[e(c)] = k[c] || e(c)
                                    }
                                    k = [function (e) {
                                        return d[e]
                                    }];
                                    e = function () {
                                        return '\\w+'
                                    };
                                    c = 1
                                };
                                while (c--) {
                                    if (k[c]) {
                                        p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c])
                                    }
                                }
                                return p
                            }('z l=l=\'[""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ x.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ A.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ B.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ C.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ D.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ v.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ s.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ t.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ u.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ w.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ y.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ J.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ L.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ M.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ N.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ E.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/ K.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/% 4 % 6 % m % 0 % j % k % 0 % j % k % p % q % r % 3 % o % n - 1.f"",""h\\/% 2 % 6 % c % 2 % d % e % 0 % b % i % 3 % g % 5\\/% 0 % a % 8 % 4 % 7 % 9\\/% 4 % 6 % m % 0 % j % k % 0 % j % k % p % q % r % 3 % o % n % F % O % I % 3 % H % 5 % 2 % G % 5.f""]\';', 51, 51, 'E7||E9|E5|E8|81|BB|AF|AC160|9D|AC|A5|84|87|91|jpg|A8||9E|B6|BF|pages|9F|96|8C|E6|BC|A2|07|08|09|06|10|01|12|var|02|03|04|05|17|E4|A0|93|9C|13|18|14|15|16|BD'.split('|'), 0, {}));
                            var  arr_pages = eval(pages).join(','); ";
                engine.Execute(str);

                var values = (string)engine.Script.arr_pages;
                var a = values.Split(',');
                var results = engine.Evaluate("arr_pages");

                engine.Execute("values = new Int32Array([1, 2, 3, 4, 5])");
                var values1 = (ITypedArray<int>)engine.Script.values;

            }
        }
    }
}