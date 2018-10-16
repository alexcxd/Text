using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace 练习.Json
{
    public class JsonTest
    {

        /// <summary>
        /// JSON.NET,
        /// </summary>
        public static void Demo1()
        {
            var json = File.ReadAllText(@"menu.json");
            //反序列化
            var menuModelList = JsonConvert.DeserializeObject<List<MenuModel>>(json);

            //序列化
            var menuModelJson = JsonConvert.SerializeObject(menuModelList);
        }
    }

    public class MenuModel
    {
        public  List<MenuModel> submenu { get; set; }
        public string sref { get; set; }
        public string name { get; set; }
    }
}