using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
 
/*
 * 20100516
 * aqq
 * update:
 */


namespace NPCMS.Common.Extensions
{
    /// <summary>
    /// String 扩展 类
    /// </summary>
    public static class StringExtensions
    {

        #region  切割
        /// <summary>
        /// 截取固定长度的 内容 
        /// 异常 ：1content 为空 
        ///           2length  <=0 为负数
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubShortContent(this string content, int length)
        {
            if (string.IsNullOrEmpty(content))
            {
                //throw(  );
                throw new Exception(" conetnt is null");
            }
            if (length < 0)
            {
                //throw
                return "length   为负数";
            }
            content = content.Trim();
            if (content.Length > length)
            {
                content = content.Substring(0, length);
                content += "...";
            }

            return content;

        }


        /// <summary>
        /// 按指定 格式 切割 字符串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static List<string> splitString(this string content, char sp)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            List<string> zhan2 = new List<string>();
            string temp = "";
            foreach (var s in content)//content 的 格式是    /area/Article/show/1
            { 
                if (s == sp)
                {
                    zhan2.Add(temp);
                    temp = "";
                }
                else
                {
                    temp += s;
                }
            }
            zhan2.Add(temp);
            return zhan2;
        }
        #endregion
        #region str2=> 转换
        public static Int32 str2Int(this string conetnt)
        {
            if (string.IsNullOrEmpty(conetnt))
            {
                throw new Exception(" conetnt is null");
            }
            int num = -1;
            int.TryParse(conetnt, out num);
            return num;

        }
        public static bool ToBool(this string conetnt)
        {
            if (string.IsNullOrEmpty(conetnt))
            {
                throw new Exception(" conetnt is null");
            }
            bool bl = false;
            bool.TryParse(conetnt, out bl);
            return bl;

        }
        public static int ToInt32(this string content)
        {
            int temp = 0;
            try
            {
                temp = Convert.ToInt32(content);
            }
            catch (Exception)
            {


            }
            return temp;
        }
        #endregion
        #region// 字符串拼接
        /// <summary>
        /// 格式化 添加
        /// </summary>
        public static string With(this string conetnt, params object[] args)
        {
            return string.Format(conetnt, args);
        }
        #endregion
     
        #region 判断是否是某类型
        public static bool IsInt(this  string content)
        {
            try
            {
                Convert.ToInt32(content);
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        #endregion
        #region html
        public static string  AddToCahe(this string  content )
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<select ></select>");
            return "<>";
        }
        #endregion
    }
    
}
