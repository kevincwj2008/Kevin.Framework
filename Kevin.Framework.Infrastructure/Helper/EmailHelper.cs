using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Kevin.Framework.Infrastructure
{
    /// <summary>
    /// Email
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email">邮件信息</param>
        /// <returns></returns>
        public static bool Send(EmailInfo email)
        {
            return Send(email.ToAddress, email.Subject, email.Body, email.SenderName, email.FromAddress, email.FromPassword, email.SmtpHost, email.SmtpPort, email.IsBodyHtml, email.EnableSsl);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toAddress">收件人邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文内容</param>
        /// <param name="senderName">发件人名称</param>
        /// <param name="fromAddress">发件人邮箱地址</param>
        /// <param name="fromPassword">发件人邮箱密码</param>
        /// <param name="smtpHost">SMTP服务地址</param>
        /// <param name="smtpPort">SMTP服务端口</param>
        /// <param name="isBodyHtml">是否是HTML邮件</param>
        /// <param name="enableSsl">是否启用SSL</param>
        /// <returns></returns>
        public static bool Send(string[] toAddress, string subject, string body, string senderName, string fromAddress, string fromPassword, string smtpHost, int? smtpPort, bool isBodyHtml = true, bool enableSsl = false)
        {
            if (toAddress == null || toAddress.Length <= 0)
                return false;

            var to = new Dictionary<string, string>();
            foreach (var item in toAddress)
                to.Add(item, string.Empty);

            return Send(to, subject, body, senderName, fromAddress, fromPassword, smtpHost, smtpPort, isBodyHtml, enableSsl);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toAddress">收件人邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文内容</param>
        /// <param name="senderName">发件人名称</param>
        /// <param name="fromAddress">发件人邮箱地址</param>
        /// <param name="fromPassword">发件人邮箱密码</param>
        /// <param name="smtpHost">SMTP服务地址</param>
        /// <param name="smtpPort">SMTP服务端口</param>
        /// <param name="isBodyHtml">是否是HTML邮件</param>
        /// <param name="enableSsl">是否启用SSL</param>
        /// <returns></returns>
        public static bool Send(string toAddress, string subject, string body, string senderName, string fromAddress, string fromPassword, string smtpHost, int? smtpPort, bool isBodyHtml = true, bool enableSsl = false)
        {
            if (string.IsNullOrWhiteSpace(toAddress))
                return false;

            var to = new Dictionary<string, string>() { { toAddress, string.Empty } };
            return Send(to, subject, body, senderName, fromAddress, fromPassword, smtpHost, smtpPort, isBodyHtml, enableSsl);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toAddress">收件人邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文内容</param>
        /// <param name="senderName">发件人名称</param>
        /// <param name="fromAddress">发件人邮箱地址</param>
        /// <param name="fromPassword">发件人邮箱密码</param>
        /// <param name="smtpHost">SMTP服务地址</param>
        /// <param name="smtpPort">SMTP服务端口</param>
        /// <param name="isBodyHtml">是否是HTML邮件</param>
        /// <param name="enableSsl">是否启用SSL</param>
        /// <returns></returns>
        public static bool Send(Dictionary<string, string> toAddress, string subject, string body, string senderName, string fromAddress, string fromPassword, string smtpHost, int? smtpPort, bool isBodyHtml = true, bool enableSsl = false)
        {
            if (toAddress == null || toAddress.Count <= 0)
                return false;

            using (MailMessage mailMessage = new MailMessage())
            {
                foreach (var to in toAddress)
                {
                    mailMessage.To.Add(new MailAddress(to.Key, to.Value, Encoding.UTF8));
                }
                mailMessage.From = new MailAddress(fromAddress, senderName, Encoding.UTF8);

                mailMessage.Subject = subject;
                mailMessage.SubjectEncoding = Encoding.UTF8;                        //邮件标题编码
                mailMessage.Body = body;
                mailMessage.BodyEncoding = Encoding.UTF8;                           //邮件内容编码
                mailMessage.IsBodyHtml = isBodyHtml;                                //是否是HTML邮件
                mailMessage.Priority = MailPriority.High;                           //邮件优先级

                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(fromAddress, fromPassword);
                if (smtpPort.HasValue)
                    client.Port = smtpPort.Value;                                             //Gmail 使用 465 和 587 端口
                client.Host = smtpHost;                                             //如 smtp.163.com, smtp.gmail.com
                client.EnableSsl = enableSsl;
                if (fromAddress.ToLower().IndexOf("gmail") > -1)
                    client.EnableSsl = true;                                        //如果使用GMail，则需要设置为true

                client.Send(mailMessage);
                return true;
            }
        }
    }

    /// <summary>
    /// 邮件信息
    /// </summary>
    public class EmailInfo
    {
        /// <summary>
        /// 收件人
        /// key：收件人邮箱地址
        /// value：收件人显示名称
        /// </summary>
        public Dictionary<string, string> ToAddress { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 发件人名称
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 发件人邮箱地址
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// 发件人邮箱密码
        /// </summary>
        public string FromPassword { get; set; }

        /// <summary>
        /// SMTP服务地址
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// SMTP服务端口
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// 是否是HTML邮件
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// 是否启用SSL
        /// </summary>
        public bool EnableSsl { get; set; }
    }
}