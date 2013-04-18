using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace RiverValley2
{
    public partial class gallery : RiverValleyPage2
    {

        public bool CanEdit
        {
            get
            {
                if (null == Session["EditRiverValleyGallery"])
                    return false;

                if ("OK" != (string)Session["EditRiverValleyGallery"])
                    return false;

                return true;
            }
        }

        public List<GalleryPicture> _galleryPictures = new List<GalleryPicture>();

        public List<GalleryFolder> _galleryFolders = new List<GalleryFolder>();

        void EnalbeEdits(bool blnEnable)
        {
            TextBoxGalleryDescription.Visible = blnEnable;
            ButtonSave.Visible = blnEnable;

            ButtonUpload.Visible = blnEnable;
            FileUpload1.Visible = blnEnable;

            LiteralUploadMessage.Visible = blnEnable;
        }

        string GenerateBreadCrums(string sVirtualFolder)
        {
            System.Text.StringBuilder sCrumbs = new System.Text.StringBuilder();
            System.Text.StringBuilder Link = new System.Text.StringBuilder();


            Link.Append("<a href=gallery.aspx?f=");

            sCrumbs.Append(Link.ToString() + ">Gallery Home</a>");

            string[] folders = sVirtualFolder.Split('/');

            if (folders.Length == 1)
            {
                return sCrumbs.ToString();
            }

            for (int i = 0; i < folders.Length - 1; i++)
            {
                string sFolderName = folders[i];

                if (sFolderName.Length < 10)
                    sFolderName = "Home";
                else
                    sFolderName = sFolderName.Remove(0, 9);

                //sFolderName = sFolderName.Replace("gallery", "");

                Link.Append(Server.UrlEncode(folders[i] + "/"));
                sCrumbs.Append(">>" + Link.ToString() + ">" + sFolderName + "</a>");
            }

            return sCrumbs.ToString();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            EnalbeEdits(CanEdit);

            DirectoryInfo directoryInfo;
            string sVirtualFolder = ".";

            if (null != Request.QueryString["f"])
                sVirtualFolder = Request.QueryString["f"];


            LiteralBreadCrums.Text = GenerateBreadCrums(sVirtualFolder);

            //popualte names and info of any sub galleries in this gallery (page)
            directoryInfo = new DirectoryInfo(Server.MapPath(sVirtualFolder));
            DirectoryInfo[] gallerydirectories = directoryInfo.GetDirectories("gallery*");

            _galleryFolders.Clear();
            foreach (DirectoryInfo d in gallerydirectories)
                _galleryFolders.Add(new GalleryFolder(d, sVirtualFolder, Request.Url.PathAndQuery));

            _galleryFolders.Sort(delegate(GalleryFolder f1, GalleryFolder f2)
            { return f1.Name.CompareTo(f2.Name); });


            RepeaterGalleries.DataSource = _galleryFolders;
            RepeaterGalleries.DataBind();

            System.Text.StringBuilder sbg = new System.Text.StringBuilder();
            foreach (GalleryFolder gf in _galleryFolders)
            {
                sbg.Append(string.Format("<a href=\"gallery.aspx?f={0}\">&#187; {1}</a><br />",
                    gf.URL, gf.GalleryName));
            }

            LiteralOtherGalleries.Text = sbg.ToString();

            //populate picture collection on this page
            directoryInfo = new DirectoryInfo(Server.MapPath(sVirtualFolder));
            FileInfo[] files = directoryInfo.GetFiles("*.jpg");

            _galleryPictures.Clear();
            foreach (FileInfo f in files)
                _galleryPictures.Add(new GalleryPicture(f, sVirtualFolder, Request.Url.PathAndQuery));

            _galleryPictures.Sort(delegate(GalleryPicture p1, GalleryPicture p2)
            { return p1.Name.CompareTo(p2.Name); });

            DataListPictures.DataSource = _galleryPictures;
            DataListPictures.DataBind();


            LiteralGalleryDescription.Text = ContentReader.GetContent(Server.MapPath(sVirtualFolder + "//Description.txt"), sVirtualFolder);
            TextBoxGalleryDescription.Text = ContentReader.GetContent(Server.MapPath(sVirtualFolder + "//Description.txt"), sVirtualFolder, true);

            ViewState["sVirtualFolder"] = sVirtualFolder;



            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<table>");

            int columncout = 5;

            for (int i = 0; i < _galleryPictures.Count; i++)
            {
                GalleryPicture galleryPicture = _galleryPictures[i];

                if (i % columncout == 0) { sb.Append("<tr>"); }

                sb.Append("<td>");
                //Write link
                sb.Append(string.Format("<a href=\"slideshow.aspx?f={0}&i={1}&r={2}\">",
                    galleryPicture.VirtualDirectory,
                    galleryPicture.Name,
                    galleryPicture.ParentURLPath));

                //Write image
                sb.Append(string.Format("<img src=\"GetThumbNail.aspx?i={0}&w=100\" alt=\"{1}\" />",
                    galleryPicture.URL, galleryPicture.Name));

                sb.Append("</a>");

                sb.Append("</td>");


                if (i % columncout == columncout) { sb.Append("</tr>"); }
            }


            sb.Append("</table>");


            LiteralPictureTable.Text = sb.ToString();

        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            string sVirtualFolder = (string)ViewState["sVirtualFolder"];

            if (null == sVirtualFolder)
                return;

            ContentReader.SetContent(Server.MapPath(sVirtualFolder + "//Description.txt"), TextBoxGalleryDescription.Text);
            LiteralGalleryDescription.Text = ContentReader.GetContent(Server.MapPath(sVirtualFolder + "//Description.txt"));


        }

        protected void ButtonUpload_Click(object sender, EventArgs e)
        {

            if (false == FileUpload1.HasFile)
            {
                //LiteralMessage.Text = "Upload Fail:No File selected";
                return;
            }


            if (false == CanEdit)
                return;


            string sFilePath = Request.QueryString["F"];
            if (null == sFilePath)
                sFilePath = ".";


            try
            {
                string test = Request.Url.ToString();
                FileUpload1.SaveAs(Server.MapPath(sFilePath) + "\\" + StripString(Path.GetFileName(FileUpload1.FileName)));
                LiteralUploadMessage.Text = "Upload status: File " + StripString(Path.GetFileName(FileUpload1.FileName)) + " uploaded!";
                Response.Redirect(Request.Url.ToString());


            }
            catch (Exception ex)
            {
                LiteralUploadMessage.Text = "Upload Fail:" + ex.Message;
            }



        }


    }
}
