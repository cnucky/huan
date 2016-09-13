using System;
using NUnit.Framework;

namespace Common.smsapi {
    public class JikeSms : IMySms {
        public void GetConfigOfSms() {


            var smsName = ConfigHelper.GetValue("jikeSmsName");
            var SmsPwd = ConfigHelper.GetValue("jikeSmsPwd");
            for (int i = 0; i < 3; i++) {
                if (!SmsApi.logined) {
                    SmsApi.logined = SmsApi.Login(smsName, SmsPwd);
                    //  SmsApi.Login

                }
            }
        }
        [Test]
        public void TestGetPhone() {
            GetConfigOfSms();
            Console.WriteLine(SmsApi.GetPhone(ConvertServerId2(SmsServer.zfb_reg_vcode)));
            ;
        }
        public string GetPhone(SmsServer smstype) {
            return SmsApi.GetPhone(ConvertServerId2(smstype));
        }

        public string GetMessage(SmsServer smstype, string phone) {
            return SmsApi.GetMessage(ConvertServerId2(smstype), phone);// GetPhone(ConvertServerId2(smstype))
        }

        public void ReleasePhone(string phone, SmsServer smstype) {
            SmsApi.ReleasePhone(phone, ConvertServerId2(smstype));
        }


        public string ConvertServerId2(SmsServer smstype) {
            if (smstype == SmsServer.zfb_reg_vcode) {
                return "1365";
            } else if (smstype == SmsServer.tb_reg_vode) {
                return "2";
            } else {

                return "";
            }
        }

        public string getVcodeAndHoldMobilenum(SmsServer smstype, string phone, string nextpid) {
            throw new NotImplementedException();
        }

        public string getOneSpecialMobilenum(SmsServer smstype, string mobile) {
            throw new NotImplementedException();
        }

        public string GetPhone(string phone, SmsServer smstype) {
            throw new NotImplementedException();
        }
    }
}