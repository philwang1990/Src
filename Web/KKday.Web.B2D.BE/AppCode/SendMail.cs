using System;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using KKday.Web.B2D.BE.App_Code;

/// <summary>
/// Summary description for SendMail
/// </summary>
///

public class SendMail
{        
    public SendMail()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Send Text Email
    /// <summary>
    ///     SendTextMail
    /// </summary>
    /// <param name="from_email"></param>
    /// <param name="from_name"></param>
    /// <param name="sendto">PAIR(string _name, string _email)</param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    public static void SendTextMail(string from_email, string from_name, Dictionary<string, string> sendto, string subject, string body)
    {
        Dictionary<string, string> bcc_email = new Dictionary<string, string>();

        SendTextMail(from_email, from_name, sendto, bcc_email, subject, body);
    }

    public static void SendTextMail(string from_email, string from_name, Dictionary<string, string> sendto,
            Dictionary<string, string> send_bcc, string subject, string body)
    {
        try
        {
            //if (!ConfigurationManager.AppSettings["EnableSMTP"].Equals("true", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    throw new Exception("系統禁止發送郵件!");
            //}

            bool IsDuplicated = false;
            string szHost = Website.Instance.Configuration["MailTo:Smtp_Host"];

            //create the mail message
            MailMessage mail = new MailMessage();

            //set the addresses : from
            mail.From = new MailAddress(from_email, from_name);
            //set the addresses : to
            foreach (KeyValuePair<string, string> receipient in sendto)
            {
                if (string.IsNullOrEmpty(receipient.Value)) continue;

                IsDuplicated = false;
                foreach (MailAddress addr in mail.To)
                {
                    if (addr.Address.Equals(receipient.Value))
                    {
                        IsDuplicated = true;
                        break;
                    }
                }

                MailAddress ma = new MailAddress(receipient.Value);
                if (string.IsNullOrEmpty(receipient.Key))
                    ma = new MailAddress(receipient.Value, receipient.Key);

                if (IsDuplicated == false) mail.To.Add(ma);
            }

            //set the content
            mail.Subject = subject;
            mail.Body = body;

            foreach (KeyValuePair<string, string> receipient in send_bcc)
            {
                if (string.IsNullOrEmpty(receipient.Value)) continue;

                IsDuplicated = false;
                foreach (MailAddress addr in mail.Bcc)
                {
                    if (addr.Address.Equals(receipient.Value))
                    {
                        IsDuplicated = true;
                        break;
                    }
                }

                MailAddress ma = new MailAddress(receipient.Value);
                if (!(string.IsNullOrEmpty(receipient.Key)))
                    ma = new MailAddress(receipient.Value, receipient.Key);

                if (IsDuplicated == false) mail.Bcc.Add(ma);
            }

            //send the message
            SmtpClient smtp = new SmtpClient(szHost, 587);
            //to authenticate we set the username and password properites on the SmtpClient
            // smtp.Credentials = new NetworkCredential(szAccount, szPassword);
            //smtp.Credentials = new NetworkCredentialByHost();
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials= new NetworkCredential("b2b@mailgun.kkday.com", "iloveB2B");
            smtp.Send(mail);
        }
        catch (Exception ex)
        {
            //FormApp.Instance.logger.ErrorFormat("{0},{1}", ex.Message, ex.StackTrace);
            throw ex;
        }
    }

    #endregion

    #region Send HTML Email

    /// <summary>
    ///    Send HTML Email
    /// </summary>
    /// <param name="from_email"></param>
    /// <param name="from_name"></param>
    /// <param name="sendto">PAIR(string _name, string _email)</param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    public static void SendHtmlMail(string from_email, string from_name, Dictionary<string, string> sendto, string subject, string body)
    {
        SendHtmlMail(from_email, from_name, sendto, subject, body, null);
    }

    public static void SendHtmlMail(string from_email, string from_name, Dictionary<string, string> sendto, string subject, string body, Dictionary<string, string> mail_tokens)
    {
        Dictionary<string, string> send_cc = new Dictionary<string, string>();
        Dictionary<string, string> send_bcc = new Dictionary<string, string>();

        SendHtmlMail(from_email, from_name, sendto, send_cc, send_bcc, subject, body, mail_tokens);
    }

    public static void SendHtmlMail(string from_email, string from_name,
        Dictionary<string, string> sendto, Dictionary<string, string> send_cc, Dictionary<string, string> send_bcc,
        string subject, string body, Dictionary<string, string> mail_tokens)
    {            
        try
        {
            //if (!ConfigurationManager.AppSettings["EnableSMTP"].Equals("true", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    throw new Exception("系統禁止發送郵件!");
            //}

            bool IsDuplicated = false;
            string szHost = Website.Instance.Configuration["MailTo:Smtp_Host"];

            //create the mail message
            MailMessage mail = new MailMessage();

            if (mail_tokens != null)
            {
                foreach (KeyValuePair<string, string> token in mail_tokens)
                {
                    mail.Headers.Add(token.Key, token.Value);
                }
            }

            //set the addresses : from
            mail.From = new MailAddress(from_email, from_name);

            //set the addresses : to
            foreach (KeyValuePair<string, string> receipient in sendto)
            {
                if (string.IsNullOrEmpty(receipient.Value)) continue;

                IsDuplicated = false;
                foreach (MailAddress addr in mail.To)
                {
                    if (addr.Address.Equals(receipient.Value))
                    {
                        IsDuplicated = true;
                        break;
                    }
                }

                MailAddress ma = new MailAddress(receipient.Value);
                if (!(string.IsNullOrEmpty(receipient.Key)))
                    ma = new MailAddress(receipient.Value, receipient.Key);

                if (IsDuplicated == false) mail.To.Add(ma);
            }

            //set the addresses : cc
            foreach (KeyValuePair<string, string> receipient in send_cc)
            {
                if (string.IsNullOrEmpty(receipient.Value)) continue;

                IsDuplicated = false;
                foreach (MailAddress addr in mail.CC)
                {
                    if (addr.Address.Equals(receipient.Value))
                    {
                        IsDuplicated = true;
                        break;
                    }
                }

                MailAddress ma = new MailAddress(receipient.Value);
                if (!(string.IsNullOrEmpty(receipient.Key)))
                    ma = new MailAddress(receipient.Value, receipient.Key);

                if (IsDuplicated == false) mail.CC.Add(ma);
            }

            //set the addresses : bcc
            foreach (KeyValuePair<string, string> receipient in send_bcc)
            {
                if (string.IsNullOrEmpty(receipient.Value)) continue;

                IsDuplicated = false;
                foreach (MailAddress addr in mail.Bcc)
                {
                    if (addr.Address.Equals(receipient.Value))
                    {
                        IsDuplicated = true;
                        break;
                    }
                }

                MailAddress ma = new MailAddress(receipient.Value);
                if (!(string.IsNullOrEmpty(receipient.Key)))
                    ma = new MailAddress(receipient.Value, receipient.Key);

                if (IsDuplicated == false) mail.Bcc.Add(ma);
            }

            //set the content
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = body;

            //send the message
            SmtpClient smtp = new SmtpClient(szHost);
            //to authenticate we set the username and password properites on the SmtpClient            
            // smtp.Credentials = new NetworkCredential(szAccount, szPassword);            
            smtp.Credentials = new NetworkCredentialByHost();
            smtp.Send(mail);
        }
        catch (Exception ex)
        {
            //FormApp.Instance.logger.ErrorFormat("{0},{1}", ex.Message, ex.StackTrace);
            throw ex;
        }
    }

    private static void SendHtmlMail(string p, string p_2, string send_to, string p_3, string MailBody)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Send HTML Email with attached file

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from_email"></param>
    /// <param name="from_name"></param>
    /// <param name="sendto">PAIR(string _name, string _email)</param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <param name="filename"></param>
    /// <param name="attach_data"></param>
    public static void SendHtmlMail(string from_email, string from_name, Dictionary<string, string> sendto, string subject, string body,
        string filename, ref byte[] attach_data)
    {
        SendHtmlMail(from_email, from_name, sendto, string.Empty, subject, body, filename, ref attach_data);
    }

    public static void SendHtmlMail(string from_email, string from_name, Dictionary<string, string> sendto, string bcc_email, string subject,
        string body, string filename, ref byte[] attach_data)
    {            
        try
        {
            //if (!ConfigurationManager.AppSettings["EnableSMTP"].Equals("true", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    throw new Exception("系統禁止發送郵件!");
            //}

            bool IsDuplicated = false;
            string szHost = Website.Instance.Configuration["MailTo:Smtp_Host"];

            //create the mail message
            MailMessage mail = new MailMessage();

            //set the addresses : from
            mail.From = new MailAddress(from_email, from_name);
            //set the addresses : to
            foreach (KeyValuePair<string, string> receipient in sendto)
            {
                if (string.IsNullOrEmpty(receipient.Value)) continue;

                IsDuplicated = false;
                foreach (MailAddress addr in mail.To)
                {
                    if (addr.Address.Equals(receipient.Value))
                    {
                        IsDuplicated = true;
                        break;
                    }
                }

                MailAddress ma = new MailAddress(receipient.Value);
                if (string.IsNullOrEmpty(receipient.Key))
                    ma = new MailAddress(receipient.Value, receipient.Key);

                if (IsDuplicated == false) mail.To.Add(ma);
            }


            //set the content
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = body;
            if (string.IsNullOrEmpty(bcc_email) == false)
            {
                mail.Bcc.Add(bcc_email);
            }

            // Create  the file attachment for this e-mail message.
            System.IO.MemoryStream ms = new System.IO.MemoryStream(attach_data);

            // Add the file attachment to this e-mail message.
            mail.Attachments.Add(new Attachment(ms, filename, "applicaiton/word"));

            //send the message
            SmtpClient smtp = new SmtpClient(szHost);
            //to authenticate we set the username and password properites on the SmtpClient
            // smtp.Credentials = new NetworkCredential(szAccount, szPassword);            
            smtp.Credentials = new NetworkCredentialByHost();
            smtp.Port = 587;
            smtp.Send(mail);
        }
        catch (Exception ex)
        {
            //FormApp.Instance.logger.ErrorFormat("{0},{1}", ex.Message, ex.StackTrace);
            throw ex;
        }
    }

    #endregion

    internal class NetworkCredentialByHost : ICredentialsByHost
    {
        #region   ICredentialsByHost Methods

        public NetworkCredential GetCredential(string host, int port, string authenticationType)
        {
            //string szAccount = ConfigurationManager.AppSettings["Smtp.Account"].ToString();
            //string szPassword = ConfigurationManager.AppSettings["Smtp.Password"].ToString();

            return null;//new NetworkCredential(szAccount, szPassword);
        }

        #endregion
    }
}

