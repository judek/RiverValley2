using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace RiverValley2
{
    public partial class RiverValley : System.Web.UI.MasterPage
    {
        bool _CanUploadAttachment;
        const int _NumberOfAttachmentsToShow = 5;

        //This comment is only to test GIT branching on 5/27/2014
        //The branch name is "TestBranch"
        
        protected void Page_Load(object sender, EventArgs e)
        {
            ButtonLogoff.Visible = (null != Session["Editing"]);
            ButtonEditHome.Visible = (null != Session["Editing"]);

            _CanUploadAttachment = (null != Session["EditUploadHomework"]);

            //For debug
            //ButtonLogoff.Visible = true;
            //ButtonEditHome.Visible = true;

            //_CanUploadAttachment = true;

            ButtonUpload.Visible = _CanUploadAttachment;
            FileUpload1.Visible = _CanUploadAttachment;


            List<Attachement> pageattachments = GetAttachments();

            if (null == pageattachments)
                return;

            if (pageattachments.Count < 1)
                return;


            pageattachments.Sort(delegate(Attachement f1, Attachement f2)
            {
                return DateTime.Compare(f2.AttachmentInfo.LastWriteTime, f1.AttachmentInfo.LastWriteTime);
            });


            if (true == _CanUploadAttachment)
                PlaceHolder1.Controls.Add(new LiteralControl("<br /><br /><font color=\"#00ff00\">Current Attachments<br /><br /></font>"));
            else
                PlaceHolder1.Controls.Add(new LiteralControl("<br /><span class=\"smalltitle\">Downloads</span><br />"));


            int nDisplayed = 0;
            int NumberOfAttachmentsToShow = _NumberOfAttachmentsToShow;

            if (Request.QueryString["ShowAllAttachments"] != null)
                NumberOfAttachmentsToShow = 99;


            foreach (Attachement att in pageattachments)
            {
                if (true == _CanUploadAttachment)
                {

                    Button button = new Button();
                    button.Text = "Remove";
                    button.BackColor = System.Drawing.Color.Red;
                    button.BorderColor = System.Drawing.Color.Red;
                    button.ID = att.AttachmentInfo.Name;

                    button.Click += new EventHandler(ButtonDeleteClicked);

                    PlaceHolder1.Controls.Add(button);
                    PlaceHolder1.Controls.Add(new LiteralControl("<font color=\"#009900\"> " + att.Title + "</font><br /><br />"));
                }
                else
                {
                    
                    
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();


                    if (nDisplayed == NumberOfAttachmentsToShow)
                    {
                        string CurrentPageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
                        sb.Append("<a href=" + CurrentPageName + "?ShowAllAttachments=True>Show all...</a><br>");
                        PlaceHolder1.Controls.Add(new LiteralControl(sb.ToString()));
                        break;
                    }

                    sb.Append("<a target=\"_blank\" href=\"attachments/page/" +
                    att.AttachmentInfo.Name +
                    "" + " \"><img src=\"picts/icon_download.gif\" alt=\"\"> " + att.Title + "</a><br />");


                    //sb.Append("<a href=\"GetFile.aspx?SF=" + "attachments/page/" +
                    //att.AttachmentInfo.Name + "&TF=" + att.Title +
                    //"" + " \"><img src=\"picts/icon_download.gif\" alt=\"\"> " + att.Title + "</a><br />");

                    PlaceHolder1.Controls.Add(new LiteralControl(sb.ToString()));
                    nDisplayed++;
                }

            }
        }

        List<Attachement> GetAttachments()
        {

            string filnamePrefix = System.IO.Path.GetFileName(Request.Url.AbsolutePath);

            if (false == filnamePrefix.EndsWith(".aspx"))
                throw new ApplicationException("Unable to parse page name from url path");


            filnamePrefix = filnamePrefix.Replace(".aspx", "") + ".";


            DirectoryInfo directoryInfo =
                    new DirectoryInfo(Server.MapPath("attachments/page"));

            if (false == directoryInfo.Exists)
                return null;

            List<Attachement> attlist = new List<Attachement>();
            foreach (FileInfo f in directoryInfo.GetFiles(filnamePrefix + "*"))
            {
                attlist.Add(new Attachement(f, f.Name.Replace(filnamePrefix, "")));
            }
            return attlist;
        }
        protected void ButtonDeleteClicked(object sender, EventArgs e)
        {
            if (false == _CanUploadAttachment)
            {
                Response.Write("Access Denied");
                Response.End();
            }
            
            Button clickedButton = sender as Button;

            if (null == clickedButton)
                return;
            

            string test = clickedButton.ID;
            File.Delete(Server.MapPath("attachments/page/" + test));

            Response.Redirect(System.IO.Path.GetFileName(Request.Url.AbsolutePath + "?ticks=" + DateTime.Now.Ticks.ToString()));
           
        }
        
        protected string CleanString(string targetString)
        {//Simple method in C# to strip non-alphanumeric letters
            return System.Text.RegularExpressions.Regex.Replace(targetString, "[^A-Za-z0-9.]", "");
        }

        protected void ButtonLogoff_Click(object sender, EventArgs e)
        {
            //string sFileName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            //string sQuery = Request.RawUrl;
            Session.Abandon();
            Response.Redirect("Default.aspx");
        }

        protected void ButtonEditHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("edit/Default.aspx");
        }

        protected void ButtonUpload_Click(object sender, EventArgs e)
        {
            if (false == _CanUploadAttachment)
            {
                LiteralMessage.Text = "Upload Fail:Access Denied";
                return;
            }
            
            if (false == FileUpload1.HasFile)
            {
                LiteralMessage.Text = "Upload Fail:No File selected";
                return;
            }


            DirectoryInfo dinfo = new DirectoryInfo(Server.MapPath(".\\attachments\\page"));

            if (false == dinfo.Exists)
            {
                LiteralMessage.Text = "Upload Fail:Server folder for page attachments does not exist.";
                return;
            }

            try
            {

                string filnamePrefix = System.IO.Path.GetFileName(Request.Url.AbsolutePath);

                if (false == filnamePrefix.EndsWith(".aspx"))
                    throw new ApplicationException("Unable to parse page name from url path");


                filnamePrefix = filnamePrefix.Replace(".aspx", "") + ".";
       
                //save uploaded attachment

                string trueFilename = CleanString(Path.GetFileName(FileUpload1.FileName));
                string targeFilename = filnamePrefix + trueFilename;
                //Delete old one if any
                File.Delete(Server.MapPath(".\\attachments\\page") + "\\" + targeFilename);
                
                FileUpload1.SaveAs(Server.MapPath(".\\attachments\\page") + "\\" + targeFilename);
                LiteralMessage.Text = "<font color=\"green\">Upload status: File uploaded!</font>";

                string sPageName = Path.GetFileName(Request.Path);
                string tweet = "New file available for download: " + trueFilename + "... http://rivervalleycommunity.org" + "/" + sPageName;
                AddTweet(tweet);

                Response.Redirect(System.IO.Path.GetFileName(Request.Url.AbsolutePath + "?ticks=" + DateTime.Now.Ticks.ToString()));
                //DumpCache();

              

            }
            catch (Exception ex)
            {
                LiteralMessage.Text = "Upload Fail:" + ex.Message;
                return;
            }

        }

        protected void AddTweet(string tweet)
        {
            if (null == Session["Tweets"])
                Session["Tweets"] = new List<string>();

            List<string> tweets = Session["Tweets"] as List<string>;

            if (null != tweets.Find(delegate(string t) { return t.ToLower() == tweet.ToLower(); }))
                return;


            tweets.Add(tweet);
           
        }
    }
}
