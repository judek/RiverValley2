using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;

namespace RiverValley2
{
    public partial class getthumbnail : System.Web.UI.Page
    {
        static string THUMB_FOLDER = "cache";
        static string THUMB_FOLDER_NAME = @"\" + THUMB_FOLDER + @"\";

        static readonly object imageWriteLock = new object();
        
        bool ThumbnailCallback()
        {
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string sOldImageFileUrl = Request.QueryString["i"];

            if (string.IsNullOrEmpty(sOldImageFileUrl))
                Response.End();


            string sOldImageFileName = Server.MapPath(sOldImageFileUrl);

            
            FileInfo olfFileInfo = new FileInfo(sOldImageFileName);
         

            
           
            int requestedWidth = 100;

            if (null != Request.QueryString["w"])
            {
                try
                {
                    requestedWidth = Int32.Parse(Request.QueryString["w"]);
                }
                catch { }
            }

            FileInfo newFileInfo = new FileInfo(olfFileInfo.DirectoryName + THUMB_FOLDER_NAME + requestedWidth.ToString() + "_" + olfFileInfo.Name);

            #region Short circut if we already have good thumb nail

            bool blnCreateThumbnail = true;
            if (true == newFileInfo.Exists)
            {
                blnCreateThumbnail = false;
                if (olfFileInfo.LastWriteTimeUtc > newFileInfo.LastWriteTimeUtc)
                {
                    blnCreateThumbnail = true;
                }
            }

            if (false == blnCreateThumbnail)
            {
                try
                {
                    //Amazingly the using code block below is MUCH FASTER than Response.WriteFile!
                    //Response.WriteFile(newFileInfo.FullName);

                    string sNewImageFileUrl = sOldImageFileUrl.Replace(olfFileInfo.Name, "") + THUMB_FOLDER + "/" + requestedWidth.ToString() + "_" + olfFileInfo.Name;
                    Server.Transfer(sNewImageFileUrl);

                    //using (System.Drawing.Image cachedThumbImage = System.Drawing.Image.FromFile(newFileInfo.FullName))
                    //{
                    //    Response.ContentType = "image/jpeg";
                    //    cachedThumbImage.Save(Response.OutputStream, ImageFormat.Jpeg);
                    //    Response.End();
                    //}
                }
                catch { Response.End(); }
            }

            #endregion

            #region Create thumbnail folder if not present
            if (false == Directory.Exists(olfFileInfo.DirectoryName + THUMB_FOLDER_NAME))
            {
                try { Directory.CreateDirectory(olfFileInfo.DirectoryName + THUMB_FOLDER_NAME); }
                catch
                {
                    Response.End();
                    return;
                }
            }
            #endregion


            bool blnShrinkToThumb = false;
            float webFactor;
            int smallHeight = 1;
            int smallWidth = 1;


            using (System.Drawing.Image oldImage = System.Drawing.Image.FromFile(Server.MapPath(Request.QueryString["i"])))
            {

                if (requestedWidth < oldImage.Width)
                {//No work to do if the image itself is smaller than thumb nail request
                    if (oldImage.Height < oldImage.Width)
                    { //Testing orientation of photo
                        //Landscape image
                        //Defaulting to webSize for now to test
                        if (oldImage.Width > requestedWidth)
                        { //testing for oldImage being larger than resized target

                            //
                            //Calculate new shrinkFactor
                            //
                            webFactor = (float)oldImage.Width / (float)requestedWidth;
                            //
                            //Calculate new height and width for photo
                            //
                            smallHeight = (int)(oldImage.Height / webFactor);
                            smallWidth = (int)(oldImage.Width / webFactor);
                            blnShrinkToThumb = true;
                        }
                    }
                    else
                    {
                        //Portrait or square image
                        //Defaulting to webSize for now to test
                        if (oldImage.Height > requestedWidth) //testing for oldImage being larger than resized target
                        {
                            //Calculate new shrinkFactor
                            webFactor = oldImage.Height / requestedWidth;

                            //Calculate new height and width for photo
                            smallHeight = (int)(oldImage.Height / webFactor);
                            smallWidth = (int)(oldImage.Width / webFactor);
                            blnShrinkToThumb = true;
                        }
                    }
                }

                System.Drawing.Image ouputImage = null;

              
                Response.ContentType = "image/jpeg";
                
                if (true == blnShrinkToThumb)
                    ouputImage = oldImage.GetThumbnailImage(smallWidth, smallHeight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                else
                    ouputImage = oldImage;

               
                ouputImage.Save(Response.OutputStream, ImageFormat.Jpeg);

                lock (imageWriteLock)
                {
                    ouputImage.Save(newFileInfo.FullName, ImageFormat.Jpeg);
                }

            }



        }
    }
}
