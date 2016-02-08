using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;

namespace RiverValley2.Edit.SendTweet
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (false == IsPostBack)
                return;



            try
            {
                SendEmail(null, TextBox1.Text, new string[] { "tweet@tweetymail.com", "judek@yahoo.com" });
            }
            catch
            {
                
            }
        }

        void SendEmail(string MessageSuubject, string MessageBody, string[] sToAddresses)
        {


            using (MailMessage mailMessage = new MailMessage())
            {
                try
                {

                    if (false == string.IsNullOrEmpty(MessageSuubject))
                        mailMessage.Subject = MessageSuubject;

                    if (false == string.IsNullOrEmpty(MessageBody))
                        mailMessage.Body = MessageBody;

                    foreach (string sToAddress in sToAddresses)
                    {
                        mailMessage.To.Add(sToAddress);
                    }

                    //tweetMessage.To.Add("tweet@tweetymail.com");
                    //tweetMessage.To.Add("judek@yahoo.com");


                    SendMailMessage(mailMessage);
                }
                catch (Exception )
                {
                    
                }

            }
        }

        public void SendMailMessage(MailMessage mail)
        {
            SendMailMessage(mail, "\"River Valley Community Church\" <2bc17e6f292e@rivervalleycommunity.org>",
                "2bc17e6f292e@rivervalleycommunity.org", "Myza2828");
        }
        
        public void SendMailMessage(MailMessage mail, string sFrom, string sUserName, string sPassword)
        {
            //set the addresses
            if (null == mail.From)
                mail.From = new MailAddress(sFrom);
            //mail.From = new MailAddress(sFrom);

            //send the message
            SmtpClient smtp = new SmtpClient("mail.rivervalleycommunity.org");

            System.Net.NetworkCredential SmtpUser;
            SmtpUser = new System.Net.NetworkCredential();
            SmtpUser.UserName = sUserName;
            SmtpUser.Password = sPassword;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtp.UseDefaultCredentials = false;
            smtp.Credentials = SmtpUser;


            smtp.Send(mail);
        }
    }
}