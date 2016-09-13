namespace Xilium.CefGlue.Client.SchemeHandler {
    internal sealed class Js2CsharpSchemeHandlerFactory : CefSchemeHandlerFactory {
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request) {
            return new Js2CsharpRequestResourceHandler();
        }
    }
}
