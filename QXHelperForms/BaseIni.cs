using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using NPCMS.Common.File;

namespace SYManager
{
    //r=WorldNpc08931,86400,100,,1,2,0
    //1 读入文本
    //2 过滤文本
    //3 解析文本
    /// <summary>
    /// INI文件解析类
    /// 20130601
    /// </summary>
    public class BaseIni {
        public BaseIni() {
            Lines = new List<string>();
            Blocks = new List<IniBlock>();
        }
        public List<string> Lines;
        public List<IniBlock> Blocks;
        public void ReadPath(string filPath) {

            StreamReader objReader = new StreamReader(filPath, System.Text.Encoding.Default);
            string sLine = "";
            while (sLine != null) {
                sLine = objReader.ReadLine();
                if (FilterLine(sLine)) {
                    Lines.Add(sLine);
                }
            }
            objReader.Close();

        }

        public virtual bool FilterLine(string sLine) {
            return sLine != null && !sLine.Equals("") && !sLine.Trim().StartsWith("##");
        }
        //        public void 
        public void analysisLine() {
            bool blockStart = false;
            IniBlock ini1 = new IniBlock();
            for (int i = 0; i < Lines.Count; i++) {

                var line1 = Lines[i].Trim();
                if (line1.StartsWith("[") && line1.EndsWith("]")) {//   [SceneType]

                    if (blockStart) {
                        Blocks.Add(ini1);
                    }
                    ini1 = new IniBlock();
                    ini1.BlockKey = line1.Replace("[", "").Replace("]", "");
                    blockStart = true;
                } else {
                    //    ///1 = 14,17,5,7,8,9,10,11,12
                    var itemlist = line1.Split(Convert.ToChar("="));

                    var item = new IniItem();
                    item.ItemKey = itemlist[0];//1 
                    //[001] 兼容老版本的函数
                    string[] itemlist2 = new string[] { };
                    string value = "";
                    if (itemlist.Length == 1) {
                        value = "";
                    } else {
                        value = itemlist[1];
                    }
                    if (value.Contains(",")) {
                        value.Split(Convert.ToChar(","));// 14,17,5,7,8,9,10,11,12    
                    }
                    item.ItemValueString = value;
                    //end of [001]
                    //ItemValueString

                    foreach (string s_item in itemlist2) {
                        item.ItemValue.Add(s_item);
                    }
                    ini1.BlockValue.Add(item);

                }
            }
            Blocks.Add(ini1);
        }

        public void test() {
            Console.WriteLine(Blocks.Count);
            foreach (var bb in Blocks) {
                Console.WriteLine("BlockKey:" + bb.BlockKey);
                foreach (var iniItem in bb.BlockValue) {
                    Console.WriteLine("iniItem:" + iniItem.ItemKey);
                    foreach (var line in iniItem.ItemValueString) {
                        Console.WriteLine(line);
                    }
                }
            }
        }

        public List<string> getItemValueString() { 
            List<string> list=new List<string> ();
            foreach (var bb in Blocks)
            {
                Console.WriteLine("BlockKey:" + bb.BlockKey);
                foreach (var iniItem in bb.BlockValue)
                {
                   // Console.WriteLine("iniItem:" + iniItem.ItemKey);
                    list.Add(iniItem.ItemValueString);
                    
                }
            }
            return list;
        }


    }
    /// <summary>
    /// 
    /// SceneType=BlockKey
    /// [SceneType]
    ///1 = 14,17,5,7,8,9,10,11,12
    /// </summary>
    public class IniBlock {
        public IniBlock() {
            BlockValue = new List<IniItem>();
        }
        /// <summary>
        /// [SceneType]
        /// </summary>
        public string BlockKey { get; set; }
        /// <summary>
        /// 1 = 14,17,5,7,8,9,10,11,12
        /// 2 = 14,17,5,7,8
        /// </summary>
        public List<IniItem> BlockValue;
    }
    /// <summary>
    /// IniItem
    /// Key=1;ValueItem=14,17....
    ///1 = 14,17,5,7,8,9,10,11,12
    /// </summary>
    public class IniItem {
        public IniItem() {
            ItemValue = new List<string>();
        }
        /// <summary>
        /// 1 = 14,17,5,7,8,9,10,11,12中的
        /// 1
        /// </summary>
        public string ItemKey { get; set; }
        /// <summary>
        /// 1 = 14,17中的14,17
        /// </summary>
        public List<string> ItemValue;
        public string ItemValueString { get; set; }
    }
}