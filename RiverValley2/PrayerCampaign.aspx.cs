using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace RiverValley2
{
    public partial class PrayerCampaign : RiverValleyPage2
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LabelMainBeforeForm.Text = GetContent("BodyBeforeSubmitForm");
            ///LabelMainAfterForm.Text = GetContent("BodyAfterSubmitForm");
            LiteralPictures.Text = GenerateRandomPictures();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LiteralMessage.Text = "";
            
            string selected = DropDownList1.SelectedValue;

            if (selected == "Nothing")
            {
                submit.Visible = false;
                LabelMainAfterForm.Text = "";
                submit.Text = "Join";
            }
            else
            {
                LabelMainAfterForm.Text = GetContent(selected);
                submit.Visible = true;
                submit.Text = "Join " + DropDownList1.SelectedItem.Text;
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {


            string ErrorMsg = "";



            
            if ((TextBoxFirstName.Text.Trim().Length < 1) || (TextBoxLastName.Text.Trim().Length < 1))
            {
                ErrorMsg += "First and Last name required.<br />";
            }

            
            if (TextBoxEmail.Text != TextBoxEmailConfirm.Text)
            {
                ErrorMsg += "Emails do not match<br />";
            }

            
            if (false == TestEmailRegex(TextBoxEmail.Text.Trim()))
            {
                ErrorMsg += "Invalid Email<br />";
            }


            if (ErrorMsg.Length > 0)
            {
                LiteralMessage.Text = "<font color=\"red\">" + ErrorMsg + "</font>";
                return;
            }

            try
            {

                //Send notification message
                string sheader = "campaign:" + DropDownList1.SelectedItem.Text + "<br /><br />Email,LastName,FirstName,Phone,Address,City,State,Zip<br />";
                string sRecord =
                    TextBoxEmail.Text.Trim() + "," +
                        TextBoxLastName.Text.Trim() + "," +
                        TextBoxFirstName.Text.Trim() + "," +
                        TextBoxPhone.Text.Trim() + "," +
                        TextBoxAddress.Text.Trim() + "," +
                        TextBoxCity.Text.Trim() + "," +
                        UserState.SelectedValue + "," +
                        TextBoxZip.Text.Trim();


                string sPath = Server.MapPath("./RiverValleyContent/Content.PrayerCampaign." + DropDownList1.SelectedValue + "AddressList.txt");
                System.IO.File.AppendAllText(sPath, sRecord + "\r\n");


                //Send welcome message
                using (MailMessage welcomeMail = new MailMessage())
                {
                    welcomeMail.Subject = "Welcome to the " + DropDownList1.SelectedItem.Text + " prayer campaign";
                    //welcomeMail.Body = GetContent(DropDownList1.SelectedValue + "Welcome");
                    welcomeMail.Body = GetContent("WelcomeEmail");
                    welcomeMail.To.Add(TextBoxEmail.Text.Trim());
                    welcomeMail.IsBodyHtml = true;

                    SendMailMessage(welcomeMail);
                }




                using (MailMessage mail = new MailMessage())
                {

                    mail.Subject = "New sign up for:" + DropDownList1.SelectedItem.Text;
                    mail.Body = sheader + sRecord;
                    mail.To.Add("rvccprayercampaign@gmail.com");
                    //mail.To.Add("judek@yahoo.com");
                    //mail.To.Add("esuddarth@gmail.com");
                    mail.IsBodyHtml = true;
                    SendMailMessage(mail);
                }

            }
            catch (Exception exp)
            {
                LiteralMessage.Text = "<font color=\"red\">" + exp.Message + "</font>";
                return;

            }
            
            
            LiteralMessage.Text = "<font color=\"green\">Thank you for joining the " +  DropDownList1.SelectedItem.Text + " prayer campaign</font>";

            submit.Visible = false;
            submit.Text = "Join";
        }
    }
}
