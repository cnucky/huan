using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PwBusiness;
using Xilium.CefGlue.WindowsForms;

namespace Xilium.CefGlue.Client {
    public partial class MainForm : Form {

        private void OnPageLoaded_163_Reg(object s, LoadEndEventArgs e) {
            string currentUrl = e.Frame.Url;
            switch (state) {
                case BusinessStatus.wy_163_begin:
                    if (!currentUrl.Contains(GlobalVar.Wy163Reg_enter_1)) {
                        break;
                    }
                    BT_WY_FillAccount.PerformClick();
                    break;
                case BusinessStatus.wy_163_vcode:
                    if (currentUrl.Contains(GlobalVar.Wy163Reg_Vcode_2)) {
                       // BT_WY_FillAccount.PerformClick();
                        break;
                    }

                    if (currentUrl.Contains(GlobalVar.Wy163Reg_Mian_3)) {
                        
                        break;
                    }
                    break;
                case BusinessStatus.wy_163_main:
                    if (!currentUrl.Contains(GlobalVar.Wy163Reg_Mian_3)) {
                        break;
                    }
                    //ok
                 //   ClearAndPrepareNext();
                    break;
                default: break;
            }


        }
        private void BT_WY_Reg_Click(object sender, EventArgs e)
        {
            state = BusinessStatus.wy_163_begin;
            MainCefFrame.LoadUrl(GlobalVar.Wy163Reg_enter_1);
        }


        private void BT_WY_FillAccount_Click(object sender, EventArgs e) {
            state = BusinessStatus.wy_163_vcode;
            //切换字母邮箱注册
            MainCefFrame.ExecuteJavaScript("_Global.main.turnOn();", MainCefFrame.Url, 0);
            //
            CefFrameHelper.SetElementValueById(MainCefFrame, "nameIpt", currentHaoZi.zfbEmail);
            //密码
            CefFrameHelper.SetElementValueById(MainCefFrame, "mainPwdIpt", currentHaoZi.tbPwd);
            //确认密码
            CefFrameHelper.SetElementValueById(MainCefFrame, "mainCfmPwdIpt", currentHaoZi.tbPwd);

        }
        private void BT_WY_Submit_s1_Click(object sender, EventArgs e) {
            string vcode = TB_VCode.Text.Trim();
            CefFrameHelper.SetElementValueById(MainCefFrame, "vcodeIpt", vcode);
            ClickById("mainRegA");
        }
        private void BT_WY_VCode_zw_Click(object sender, EventArgs e) {
            state = BusinessStatus.wy_163_main;
            string vcode = TB_VCode.Text.Trim();
            CefFrameHelper.SetElementValueById(MainCefFrame, "gvcodeIpt", vcode);
            ClickById("gsubmitA");
        }


    }
}
