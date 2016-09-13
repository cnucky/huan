using System;
using System.Windows.Forms;
using PwBusiness;
using Xilium.CefGlue.WindowsForms;

namespace Xilium.CefGlue.Client {

    public partial class MainForm : Form {

        private void OnPageLoaded_pp_Ads(object s, LoadEndEventArgs e) {
            string currentUrl = e.Frame.Url;
            switch (state) {
                case BusinessStatus.pp_ads_enter:
                    if (currentUrl.Contains("SearchShopAction.xhtml")) {
                        state = BusinessStatus.pp_ads_enterOk;
                        adForm.BT_Add_talk_Click(s, e);

                    }
                    break;
                case BusinessStatus.pp_ads_enterOk:
                    if (currentUrl.Contains("data:text/html,chromewebdata")) {
                        state = BusinessStatus.pp_ads_enter;

                        MainCefFrame.Browser.GoBack();
                    }


                    break;
            }
        }

        private SendAds adForm;
        private void BT_SendADs_Click(object sender, EventArgs e) {
            if (adForm == null) {
                adForm = new SendAds(this);
                adForm.Show();
            } else {
                adForm.Activate();
            }

        }




    }
}