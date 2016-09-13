using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace JiShi.BLL
{
    public class MailBll
    {
        public static void SendOneEmail(SmtpClient smtpClient, string from_email, string send_email, string subject, string body, string fileNamePath)
        {
            MailMessage mailMessage = new MailMessage();
            // 指明邮件发送的地址，主题，内容等信息
            // 发信人的地址为登录收发器的地址，这个收发器相当于我们平时Web版的邮箱或者是OutLook中配置的邮箱
            mailMessage.From = new MailAddress(from_email);
            mailMessage.To.Add(send_email);
            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = Encoding.Default;
            mailMessage.Body = body;
            mailMessage.BodyEncoding = Encoding.Default;
            // 设置邮件正文不是Html格式的内容
            mailMessage.IsBodyHtml = false;
            // 设置邮件的优先级为普通优先级
            mailMessage.Priority = MailPriority.Normal;
            //mailMessage.ReplyTo = new MailAddress(tbxUserMail.Text);

            //string extName = Path.GetExtension(fileNamePath).ToLower();

            // 封装发送的附件
            System.Net.Mail.Attachment attachment = null;

            //if (extName == ".rar" || extName == ".zip")
            //{
            //    attachment = new System.Net.Mail.Attachment(fileNamePath, MediaTypeNames.Application.Zip);
            //}
            //else
            //{
            //    attachment = new System.Net.Mail.Attachment(fileNamePath, MediaTypeNames.Application.Octet);
            //}


            //mailMessage.Attachments.Add(attachment);


            // 发送写好的邮件
            try
            {
                // SmtpClient类用于将邮件发送到SMTP服务器
                // 该类封装了SMTP协议的实现，
                // 通过该类可以简化发送邮件的过程，只需要调用该类的Send方法就可以发送邮件到SMTP服务器了。
                smtpClient.Send(mailMessage);
                
                //  LB_tips.Text = "send scuess!";
                //    MessageBox.Show("邮件发送成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SmtpException smtpError)
            {
                //MessageBox.Show("邮件发送失败：[" + smtpError.StatusCode + "]；["
                //                + smtpError.Message + "];\r\n[" + smtpError.StackTrace + "]."
                //    , "错误", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            finally
            {
                mailMessage.Dispose();
                //this.Cursor = Cursors.Default;
            }
        }




         
        private static bool FindEmailAddress(string fname, out string email)
        {
            email = "";
            var res = Regex.Match(fname, "[(（《]*.com");
            if (!res.Success) return false;

            email = res.Value;
            return true;
        }
    }
}
