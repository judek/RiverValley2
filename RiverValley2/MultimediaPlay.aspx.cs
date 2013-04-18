using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace RiverValley2
{
    public partial class MultimediaPlay : RiverValleyPage2
    {
        bool _CanEdit;
        bool CanEdit
        {
            get { return  _CanEdit; }
        }
        


        protected void Page_Load(object sender, EventArgs e)
        {
            _CanEdit = (null != Session["EditRiverValleyMedia"]);

            if (IsPostBack)
                return;


            EnalbeEdits(CanEdit);
            

            if (null == Request.QueryString["F"])
                return;

            string sFilePath = Request.QueryString["F"];

            MultimediaFile multimediaFile = new MultimediaFile(this, sFilePath);
            if (false == multimediaFile.Exists)
                return;


           


            //LiteralDownloadLink.Text = "<a href=\"GetFile.aspx?SF=" + Document.MULTIMEDIA_FOLDER + "/" + multimediaFile.Name + "\"><img src=\"picts/icon_download.gif\" alt=\"Download this version\"> Click here</a> to download.";
            

            TextBoxSubject.Text = multimediaFile.Title;
            LabelSubject.Text = multimediaFile.Title;
            LiteralTags.Text += multimediaFile.Tags.ToString();
            TextBoxTags.Text = multimediaFile.Tags.ToString();

            if (multimediaFile.Attachments.Count > 0)
            {
                LiteralAttachments.Text = "Attachements:";

                foreach (Attachement att in multimediaFile.Attachments)
                    LiteralAttachments.Text += "&nbsp;<a target=_blank href=" + Document.ATTACHMENT_FOLDER + "/" + att.AttachmentInfo.Name + ">" + att.Title + "</a>";
            }
            
            //LiteralMain.Text = multimediaFile.HTMLDescription;
            LiteralDescription.Text = ContentReader.FormatTextBlock(multimediaFile.Description);
            TextBoxDescription.Text = multimediaFile.Description;

            LiteralDescription.Text += "<br /><span class=\"eventTime\"><br /><a href=\"GetFile.aspx?SF=" + Document.MULTIMEDIA_FOLDER + "/" + multimediaFile.Name + "\"><img src=\"picts/icon_download.gif\" alt=\"Download this version\"> Click here</a> to download.</span>";

            LiteralDate.Text = multimediaFile.Dated.ToLongDateString();
            TextBoxDated.Text = multimediaFile.Dated.ToShortDateString();


            string sThisFileName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            string PodcastPageURL = (Request.Url.AbsoluteUri).Replace(sThisFileName, "Podcast.aspx");
            //Remove any query strings
            int nQueryBegin = PodcastPageURL.IndexOf('?');
            if (nQueryBegin > 0)
                PodcastPageURL = PodcastPageURL.Substring(0, nQueryBegin);

            //LabelTagCloud.Text = "Categories <i>(Click any link below)</i><br />";
            //LabelTagCloud.Text += "<br /><a href=\"" + PodcastPageURL + "\" target=_blank><img border=0 src=picts/feed-icon-14x14.png alt=\"Sermon rss feed\" /> Podcast</a><br />";
            //LiteralPictures.Text += "<br /><a href=\"" + "Podcast.aspx" + "\" target=_blank><img border=0 src=picts/feed-icon-14x14.png alt=\"Sermon rss feed\" /> Podcast</a><br />";


        }

        void EnalbeEdits(bool blnEnable)
        {
            
            
            //show during edit 
            TextBoxDescription.Visible = blnEnable;
            TextBoxSubject.Visible = blnEnable;
            ButtonSave.Visible = blnEnable;
            TextBoxDated.Visible = blnEnable;
            //LiteralSub.Visible = blnEnable;
            LiteralTags.Visible = blnEnable;
            TextBoxTags.Visible = blnEnable;
            ButtonDelete.Visible = blnEnable;
            CheckBoxEnableDelete.Visible = blnEnable;
            ButtonUpload.Visible = blnEnable;
            FileUpload1.Visible = blnEnable;


            //Show during not editing
            LiteralTags.Visible = (!blnEnable);
            LiteralDescription.Visible = (!blnEnable);
            LabelSubject.Visible = (!blnEnable);
            LiteralDate.Visible = (!blnEnable);
            LiteralDoneButton.Visible = (!blnEnable);


        }

        protected void ButtonDone_Click(object sender, EventArgs e)
        {
            Response.Redirect("Multimedia.aspx");
        }
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (false == CanEdit)
                return;


            if (null == Request.QueryString["F"])
                return;

            string sFilePath = Request.QueryString["F"];

            MultimediaFile multimediaFile = new MultimediaFile(this, sFilePath);
            if (false == multimediaFile.Exists)
                return;



            multimediaFile.Title = TextBoxSubject.Text.Trim();

            LabelSubject.Text = TextBoxSubject.Text;


            multimediaFile.Description = TextBoxDescription.Text.Trim();


            TagList newTaglist = new TagList(TextBoxTags.Text.Trim());
            multimediaFile.Tags = newTaglist;

            DateTime newFileTime;
            try
            {
                newFileTime = DateTime.Parse(TextBoxDated.Text.Trim());
                newFileTime = newFileTime.AddHours(12);
            }
            catch
            {
                return;
            }


            multimediaFile.Dated = newFileTime;


            LiteralDate.Text = newFileTime.ToLongDateString();

            LiteralDescription.Text = TextBoxDescription.Text;


            LiteralMessage.Text = "Save Successfull";
            string tweet = "Sermon updated http://rivervalleycommunity.org" + Request.Url.AbsolutePath + "&" + Request.Url.PathAndQuery;
            AddTweet(tweet);
            DumpCache();

        }
       //Git Test
        
        protected void CheckBoxEnableDelete_CheckedChanged(object sender, EventArgs e)
        {
            ButtonDelete.Enabled = CheckBoxEnableDelete.Checked;
            if (CheckBoxEnableDelete.Checked)
            {
                ButtonUpload.Text = "Delete Attachements";
                ButtonUpload.BackColor = System.Drawing.Color.Yellow;
                FileUpload1.Visible = false;
                ButtonSave.Visible = false;
                TextBoxDated.Visible = false;
                ButtonDelete.Visible = true;
                ButtonDelete.Text = "Delete entire sermon";

            }
            else
            {
                ButtonUpload.Text = "Upload";
                ButtonUpload.BackColor = System.Drawing.Color.Silver;
                FileUpload1.Visible = true;
                ButtonSave.Visible = true;
                TextBoxDated.Visible = true;
                ButtonDelete.Text = "Delete";
                ButtonDelete.Visible = true;

            }
        }
        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (false == CanEdit)
                return;



            if (null == Request.QueryString["F"])
                return;

            string sFilePath = Request.QueryString["F"];

            MultimediaFile multimediaFile = new MultimediaFile(this, sFilePath);
            if (false == multimediaFile.Exists)
                return;


            multimediaFile.Delete();


            LiteralMessage.Text = "Delete Complete";
            DumpCache();

        }
        protected void ButtonUpload_Click(object sender, EventArgs e)
        {

            if ((false == FileUpload1.HasFile) && (false == CheckBoxEnableDelete.Checked))
            {
                LiteralMessage.Text = "Upload Fail:No File selected";
                return;
            }



            if (false == CanEdit)
                return;



            if (null == Request.QueryString["F"])
                return;

            string sFilePath = Request.QueryString["F"];

            MultimediaFile multimediaFile = new MultimediaFile(this, sFilePath);
            if (false == multimediaFile.Exists)
                return;

            if (CheckBoxEnableDelete.Checked)
            {
                foreach (Attachement a in multimediaFile.Attachments)
                {
                    a.AttachmentInfo.Delete();
                }

                LiteralMessage.Text = "Attachements Deleted";
                return;
            }


            try
            {
                string targeFilename = multimediaFile.Name + ".att." + StripString(Path.GetFileName(FileUpload1.FileName));
                FileUpload1.SaveAs(Server.MapPath(Document.ATTACHMENT_FOLDER) + "\\" + targeFilename);
                LiteralMessage.Text = "Upload status: File uploaded!";
                DumpCache();
            }
            catch (Exception ex)
            {
                LiteralMessage.Text = "Upload Fail:" + ex.Message;
            }


        }
    }
}
