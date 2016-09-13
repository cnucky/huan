using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Xilium.CefGlue.Demo.Renderer {
    public class ExampleAv8Handler : CefV8Handler {

        public string MyParam { get; set; }
        public ExampleAv8Handler() {
            MyParam = "ExampleAv8Handlerler : flydoos@vip.qq.com";
        }
        protected override bool Execute(string name, CefV8Value obj,
            CefV8Value[] arguments, out CefV8Value returnValue,
            out string exception) {

            string result = string.Empty;
            switch (name) {
                case "MyFunction":
                    MyFunction();
                    break;
                case "GetMyParam":
                    result = GetMyParam();
                    break;
                case "SetMyParam":
                    result = SetMyParam(arguments[0].GetStringValue());
                    break;
                default:
                    MessageBox.Show(string.Format("JS调用C# >> {0} >> {1} 返回值",
                        name, obj.GetType()), "系统提示", MessageBoxButtons.OK);
                    break;
            }
            returnValue = CefV8Value.CreateString(result);
            exception = null;
            return true;
             

        }
        #region 方法
        /// <summary>
        /// 我的函数
        /// </summary>
        public void MyFunction() {
            MessageBox.Show("ExampleAv8Handlerler : JS调用C# >> MyFunction >> 无 返回值",
                "系统提示", MessageBoxButtons.OK);
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <returns></returns>
        public string GetMyParam() {
            return MyParam;
        }
        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public string SetMyParam(string value) {
            MyParam = value;
            return MyParam;
        }
        #endregion 方法



    }
}
