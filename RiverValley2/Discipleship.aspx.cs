using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace RiverValley2
{
    public partial class Discipleship : RiverValleyPage2
    {
        //bool _CanUploadHomework;
        //bool CanUploadHomework
        //{
        //    get { return _CanUploadHomework; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            //_CanUploadHomework = (null != Session["EditUploadHomework"]);


            //ButtonUpload.Visible = _CanUploadHomework;
            //FileUpload1.Visible = _CanUploadHomework;
            //LiteralMessage.Visible = _CanUploadHomework;


            LabelMain.Text = GetContent("Body");
            LiteralPictures.Text = GenerateRandomPictures();

            //DirectoryInfo dinfo = new DirectoryInfo(Server.MapPath(".\\attachments\\homework"));
            //FileInfo[] files = dinfo.GetFiles("hmwrk.*");


            //if (files.Length < 1)
            //{
            //    LiteralHomeworkLink.Text = "";
            //    return;
            //}

            //string targetFileName = files[0].Name.Replace("hmwrk.", "");

            //LiteralHomeworkLink.Text = "<a href=\"GetFile.aspx?SF=" + "attachments/homework/" +
            //    files[0].Name + "&TF=" + targetFileName +
            //    "" + " \"><img src=\"picts/icon_download.gif\" alt=\"\"> Download latest homework</a>";

        }

        //protected void ButtonUpload_Click(object sender, EventArgs e)
        //{
        //    if (false == FileUpload1.HasFile)
        //    {
        //        LiteralMessage.Text = "Upload Fail:No File selected";
        //        return;
        //    }

        //    if (false == CanUploadHomework)
        //    {
        //        LiteralMessage.Text = "Upload Fail:Access Denied";
        //        return;
        //    }

        //    try
        //    {

        //        //Delete any existing homework
        //        DirectoryInfo dinfo = new DirectoryInfo(Server.MapPath(".\\attachments\\homework"));
        //        FileInfo[] files = dinfo.GetFiles("hmwrk.*");
        //        foreach (FileInfo f in files)
        //        {
        //            File.Delete(f.FullName);
        //        }

        //        //save uploaded homework
        //        string targeFilename = "hmwrk." + StripString(Path.GetFileName(FileUpload1.FileName));
        //        FileUpload1.SaveAs(Server.MapPath(".\\attachments\\homework") + "\\" + targeFilename);
        //        LiteralMessage.Text = "<font color=\"green\">Upload status: File uploaded!</font>";
        //        DumpCache();
        //    }
        //    catch (Exception ex)
        //    {
        //        LiteralMessage.Text = "Upload Fail:" + ex.Message;
        //        return;
        //    }



        //}
    }
}
