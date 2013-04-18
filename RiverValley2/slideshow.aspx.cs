using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiverValley2
{
    [Serializable()]
    public class slideShowPicture
    {



        public slideShowPicture(string sPath, string sName)
        {
            Name = sName;
            Path = sPath;
        }

        public string FullPath
        {
            get
            {
                return Path + "/" + Name;
            }
        }


        public string Path;
        public string Name;
    }

    public partial class slideshow : System.Web.UI.Page
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

        public List<slideShowPicture> SlideShowPictures
        {
            get
            {
                return ViewState["SlideShowPictures"] as List<slideShowPicture>;
            }
            set
            {
                ViewState["SlideShowPictures"] = value;
            }
        }
        public string ImageIndex
        {
            get
            {
                return ViewState["ImageIndex"] as string;
            }
            set
            {
                ViewState["ImageIndex"] = value;
            }

        }
        void EnalbeEdits(bool blnEnable)
        {
            TextBoxDescription.Visible = blnEnable;
            ButtonSave.Visible = blnEnable;
            CheckBoxEnableDelete.Visible = blnEnable;
            ButtonDelete.Visible = blnEnable;
            ButtonRotateLeft.Visible = blnEnable;
            ButtonRotateRight.Visible = blnEnable;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            string sFolder;
            EnalbeEdits(CanEdit);

            if (null == Request.QueryString["f"])
                return;

            sFolder = Request.QueryString["f"];

            if (null == ImageIndex)
            {
                ImageIndex = "0";
            }

            if (null == SlideShowPictures)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath(sFolder));
                FileInfo[] files = directoryInfo.GetFiles("*.jpg");

                SlideShowPictures = new List<slideShowPicture>();
                foreach (FileInfo f in files)
                    SlideShowPictures.Add(new slideShowPicture(sFolder, f.Name));

                SlideShowPictures.Sort(delegate(slideShowPicture p1, slideShowPicture p2)
                { return p1.Name.CompareTo(p2.Name); });

            }

            int i = 0;


            if (false == IsPostBack)
            {
                if (Request.QueryString["i"] != null)
                {
                    i = SlideShowPictures.FindIndex(delegate(slideShowPicture p) { return p.Name == Request.QueryString["i"]; });
                    ImageIndex = ShowImage(i).ToString();
                }
            }


            ShowImage(i);


        }

        protected void ButtonNext_Click(object sender, EventArgs e)
        {
            advanceImage(1);
        }

        protected void ButtonPrevious_Click(object sender, EventArgs e)
        {
            advanceImage(-1);
        }

        void advanceImage(int nPosition)
        {

            if (null == ImageIndex)
                ImageIndex = "0";

            int i = Int32.Parse(ImageIndex);

            i += nPosition;

            i = ShowImage(i);

            ImageIndex = i.ToString();
        }

        int ShowImage(int i)
        {
            string sPicturePath;
            slideShowPicture currentPicture;
            try
            {
                currentPicture = SlideShowPictures[i];
            }
            catch
            {
                i = 0;
                currentPicture = SlideShowPictures[0];
            }

            sPicturePath = currentPicture.FullPath;


            Image1.ImageUrl = "ShrinkImage.aspx?i=" + sPicturePath + "&w=900";
            LiteralDescription.Text = ContentReader.GetContent(Server.MapPath(GetPictureDescriptionFile(sPicturePath)), sPicturePath);
            TextBoxDescription.Text = ContentReader.GetContent(Server.MapPath(GetPictureDescriptionFile(sPicturePath)), sPicturePath, true);

            ViewState["sPicturePath"] = sPicturePath;

            string sFullSizePath;
            sFullSizePath = currentPicture.Path + "/fullsize/" + currentPicture.Name;

            if (File.Exists(Server.MapPath(sFullSizePath)))
                LiteralFullSizeLink.Text = "<a href=\"" + sFullSizePath + "\"><img src=\"picts/icon_download.gif\" alt=\"Download this version\"> Download Full Size</a>";
            else
                LiteralFullSizeLink.Text = "";

            return i;
        }

        protected void ButtonDone_Click(object sender, EventArgs e)
        {
            if (null == Request.QueryString["r"])
                return;

            Response.Redirect(Request.QueryString["r"]);
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            string sPicturePath = (string)ViewState["sPicturePath"];

            if (null == sPicturePath)
                return;

            ContentReader.SetContent(Server.MapPath(GetPictureDescriptionFile(sPicturePath)), TextBoxDescription.Text);
            LiteralDescription.Text = ContentReader.GetContent(Server.MapPath(GetPictureDescriptionFile(sPicturePath)));

        }
        string GetPictureFullSizeServerPath(string sPicturePath)
        {
            return sPicturePath + ".txt";
        }
        string GetPictureDescriptionFile(string sPicturePath)
        {
            return sPicturePath + ".txt";
        }

        protected void CheckBoxEnableDelete_CheckedChanged(object sender, EventArgs e)
        {
            ButtonDelete.Enabled = CheckBoxEnableDelete.Checked;
        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (false == CanEdit)
                return;

            string sPicturePath = ViewState["sPicturePath"] as string;

            string sFolder = Request.QueryString["f"];

            File.Delete(Server.MapPath(sPicturePath));
            File.Delete(Server.MapPath(sPicturePath) + ".txt");

            Response.Redirect(Request.QueryString["r"]);
        }

        protected void ButtonRotateRight_Click(object sender, EventArgs e)
        {
            if (null == Request.QueryString["r"])
                return;

            RotatePicture(System.Drawing.RotateFlipType.Rotate90FlipNone);
        }

        protected void ButtonRotateLeft_Click(object sender, EventArgs e)
        {
            if (null == Request.QueryString["r"])
                return;

            RotatePicture(System.Drawing.RotateFlipType.Rotate270FlipNone);
        }

        void RotatePicture(System.Drawing.RotateFlipType rotateType)
        {
            string sPicturePath = ViewState["sPicturePath"] as string;

            if (null == sPicturePath)
                return;

            System.Drawing.Image image = null;
            string imageFileName = Server.MapPath(sPicturePath);

            try
            {
                image = System.Drawing.Image.FromFile(imageFileName);

                image.RotateFlip(rotateType);
                image.Save(imageFileName);
                //Response.Redirect(Request.QueryString["r"]);
                Image1.ImageUrl = "ShrinkImage.aspx?i=" + sPicturePath + "&w=900";
            }
            finally
            {
                if (null != image) image.Dispose();
            }
        }


    }
}
