using System.Web;
using DotNet4.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class SmsApi {
    private static readonly HttpHelper http = new HttpHelper();
    private static string loginCookies;
    private static string cookies;
    public static bool logined;
    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="username"></param>
    /// <param name="userpass"></param>
    public static bool Login(string username, string userpass) {
        bool bRet = false;
        var result = DoPost(string.Format("action={2}&event_name_login={3}&uid={0}&password={1}", username, MD5Encrypt(userpass).ToUpper(), HEConToUE("user: UserEventAction"), HEConToUE("提交")));
        if (result.StatusCode == System.Net.HttpStatusCode.OK && result.Html.Contains("登录成功")) {
            logined = true;
            bRet = true;
        }
        loginCookies = cookies;
        return bRet;
    }

    /// <summary>
    /// 获取一个手机号
    /// </summary>
    /// <param name="serviceId">项目ID</param>
    /// <returns></returns>
    public static string GetPhone(string serviceId) {
        string bret = null;
        if (logined) {
            var result = DoPost(string.Format("event_name_getPhone=%E5%8F%96%E6%89%8B%E6%9C%BA%E5%8F%B7&action=phone%3APhoneEventAction&serviceId={0}", serviceId));
            if (result.StatusCode == System.Net.HttpStatusCode.OK) {
                string m = Regex.Match(result.Html, @"(\d{11})", RegexOptions.Multiline).Groups[1].Value;
                if (!string.IsNullOrEmpty(m)) {
                    bret = m;
                } else {
                    bret = "未获取到号码";
                }
            }
        } else {
            bret = "请先登录";
        }
        return bret;
    }

    public static string GetMessage(string serviceId, string phone) {
        string bret = null;
        if (logined) {
            var result = DoPost(string.Format("event_name_getMessage=%E5%8F%96%E7%9F%AD%E4%BF%A1&action=phone%3APhoneEventAction&serviceId={0}&phone={1}", serviceId, phone));
            if (result.StatusCode == System.Net.HttpStatusCode.OK) {
                bret = result.Html;
            }
        } else {
            bret = "请先登录";
        }
        return bret;
    }

    /// <summary>
    /// 执行Post
    /// </summary>
    /// <param name="postdata"></param>
    /// <returns></returns>
    private static HttpResult DoPost(string postdata) {
        var item = new HttpItem() {
            Cookie = loginCookies,
            URL = "http://www.jikesms.com/common/ajax.htm", //URL这里都是测试URl   必需项        
            Encoding = Encoding.UTF8,
            ContentType = "application/x-www-form-urlencoded",
            Method = "post", //URL     可选项 默认为Get
            PostDataType = PostDataType.String,
            Postdata = postdata
        };
        item.Header.Add("Accept-Encoding: gzip,deflate,sdch");

        HttpResult result = http.GetHtml(item);
        cookies = result.Cookie;

        return result;
    }

    protected static string HEConToUE(string content) {
        return HttpUtility.UrlEncode(HttpUtility.HtmlDecode(content), Encoding.UTF8);
    }

    public static string MD5Encrypt(string beforeStr) {
        string afterString = "";
        try {
            MD5 md5 = MD5.Create();
            byte[] hashs = md5.ComputeHash(Encoding.UTF8.GetBytes(beforeStr));

            foreach (byte by in hashs)
                //这里是字母加上数据进行加密.//3y 可以,y3不可以或 x3j等应该是超过32位不可以
                afterString += by.ToString("x2");
        } catch {
        }
        return afterString;
    }

    public static void ReleasePhone(string phone, string serverid) {
         
        //        地址：/common/ajax.htm
        //编码：UTF-8
        //表单提交方法：POST
        //参数
        //action=phone:PhoneEventAction
        //event_name_cancelRecv=提交
        //serviceId=[服务项目Id] （多个服务项目，后面加多个serviceId=[服务项目Id] ）
        //phone=[要释放的手机号]
        if (logined) {
            var result = DoPost(string.Format("event_name_cancelRecv=%E6%8F%90%E4%BA%A4&action=phone%3APhoneEventAction&serviceId={0}", serverid));
            if (result.StatusCode == System.Net.HttpStatusCode.OK) {
                if (result.Html.Contains("true")) {
                    Console.WriteLine("释放成功.");
                } else {
                    Console.WriteLine(result.Html);
                }
                //string m = Regex.Match(result.Html, @"(\d{11})", RegexOptions.Multiline).Groups[1].Value;
                //if (!string.IsNullOrEmpty(m)) {
                //    bret = m;
                //} else {
                //    bret = "未获取到号码";
                //}
            }
        } else {
            Console.WriteLine("请先登录");
        }

    }
}