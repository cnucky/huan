using Xilium.CefGlue.Demo;

namespace Xilium.CefGlue.Demo {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xilium.CefGlue;

    class DemoRenderProcessHandler : CefRenderProcessHandler {
        protected override bool OnProcessMessageReceived(CefBrowser browser,
            CefProcessId sourceProcess, CefProcessMessage message) {
            Console.WriteLine("Render::OnProcessMessageReceived: SourceProcess={0}", sourceProcess);
            Console.WriteLine("Message Name={0} IsValid={1} IsReadOnly={2}", message.Name, message.IsValid, message.IsReadOnly);
            var arguments = message.Arguments;
            for (var i = 0; i < arguments.Count; i++) {
                var type = arguments.GetValueType(i);
                object value;
                switch (type) {
                    case CefValueType.Null: value = null; break;
                    case CefValueType.String: value = arguments.GetString(i); break;
                    case CefValueType.Int: value = arguments.GetInt(i); break;
                    case CefValueType.Double: value = arguments.GetDouble(i); break;
                    case CefValueType.Bool: value = arguments.GetBool(i); break;
                    default: value = null; break;
                }

                Console.WriteLine("  [{0}] ({1}) = {2}", i, type, value);
            }

            if (message.Name == "myMessage2") return true;

            var message2 = CefProcessMessage.Create("myMessage2");
            var success = browser.SendProcessMessage(CefProcessId.Renderer, message2);
            Console.WriteLine("Sending myMessage2 to renderer process = {0}", success);

            var message3 = CefProcessMessage.Create("myMessage3");
            var success2 = browser.SendProcessMessage(CefProcessId.Browser, message3);
            Console.WriteLine("Sending myMessage3 to browser process = {0}", success);

            return false;
        }
        /*
        private ExampleAv8Handler exampleA;

        protected override void OnWebKitInitialized() {
            #region 原生方式注册 ExampleA
            exampleA = new ExampleAv8Handler();
            const string exampleAJavascriptCode = @"function exampleA() {}
    if (!exampleA) exampleA = {};
    (function() {
        exampleA.__defineGetter__('myParam',
        function() {
            native function GetMyParam();
            return GetMyParam();
        });
        exampleA.__defineSetter__('myParam',
        function(arg0) {
            native function SetMyParam(arg0);
            SetMyParam(arg0);
        });
        exampleA.myFunction = function() {
            native function MyFunction();
            return MyFunction();
        };
        exampleA.getMyParam = function() {
            native function GetMyParam();
            return GetMyParam();
        };
        exampleA.setMyParam = function(arg0) {
            native function SetMyParam(arg0);
            SetMyParam(arg0);
        };
    })();";
            CefRuntime.RegisterExtension("exampleAExtensionName", exampleAJavascriptCode, exampleA);
            #endregion 原生方式注册 ExampleA
            base.OnWebKitInitialized();

        }
    */
    }
}
