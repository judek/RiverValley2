using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace RiverValley2
{
    public partial class PayAudio : RiverValleyPage2
    {

        bool _CanEdit;
        bool CanEdit
        {
            get { return _CanEdit; }
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


            if (multimediaFile.Attachments.Count > 0)
                LiteralAttachements.Text += "<br /><span class=\"footer\" >Attachements:</span>";

            foreach (Attachement att in multimediaFile.Attachments)
            {
                LiteralAttachements.Text += "&nbsp;<span class=\"footer\" ><a target=_blank href=" + Document.ATTACHMENT_FOLDER + "/" + att.AttachmentInfo.Name + ">" + att.Title + "</a></span>";
            }


            LiteralDownloadLink.Text = "<a href=\"GetFile.aspx?SF=" + Document.MULTIMEDIA_FOLDER + "/" + multimediaFile.Name + "\"><img src=\"picts/icon_download.gif\" alt=\"Download this version\"> Click here</a> to download.";


            TextBoxSubject.Text = multimediaFile.Title;
            LiteralSubject.Text = multimediaFile.Title;
            TextBoxTags.Text = multimediaFile.Tags.ToString();
            LiteralDescription.Text = multimediaFile.HTMLDescription;
            TextBoxDescription.Text = multimediaFile.Description;


            LabelDated.Text = multimediaFile.Dated.ToLongDateString();
            TextBoxDated.Text = multimediaFile.Dated.ToShortDateString();

        }
        void EnalbeEdits(bool blnEnable)
        {
            TextBoxDescription.Visible = blnEnable;
            TextBoxSubject.Visible = blnEnable;
            ButtonSave.Visible = blnEnable;
            TextBoxDated.Visible = blnEnable;
            LiteralSub.Visible = blnEnable;
            LiteralTags.Visible = blnEnable;
            TextBoxTags.Visible = blnEnable;
            ButtonDelete.Visible = blnEnable;
            CheckBoxEnableDelete.Visible = blnEnable;
            ButtonUpload.Visible = blnEnable;
            FileUpload1.Visible = blnEnable;




            LiteralDescription.Visible = (!blnEnable);
            LiteralSubject.Visible = (!blnEnable);
            LabelDated.Visible = (!blnEnable);


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

            LiteralSubject.Text = TextBoxSubject.Text;


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


            LabelDated.Text = newFileTime.ToLongDateString();

            LiteralDescription.Text = TextBoxDescription.Text;


            LiteralMessage.Text = "Save Successfull";
            DumpCache();

        }
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
