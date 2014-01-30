using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace RiverValley2
{
    public partial class ShrinkImage : System.Web.UI.Page
    {
        bool _InsertWarterMark = true;
        static string THUMB_FOLDER_NAME = @"\cache\";

        static readonly object imageWriteLock = new object();
        
        protected void Page_Load(object sender, EventArgs e)
        {

            System.Drawing.Image oldImage = null;
            System.Drawing.Image shrunkImage = null;
            System.Drawing.Image ouputImage = null;
            Graphics oGraphic = null;
            Graphics canvas = null;
            Font wmFont = null;
            float fWaterMarkFontSize = 2;

            string sOldImageFileName = Server.MapPath(Request.QueryString["i"]);
            
            FileInfo olfFileInfo = new FileInfo(sOldImageFileName);




            try
            {

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
   
                        using (System.Drawing.Image cachedThumbImage = System.Drawing.Image.FromFile(newFileInfo.FullName))
                        {
                            Response.ContentType = "image/jpeg";
                            cachedThumbImage.Save(Response.OutputStream, ImageFormat.Jpeg);
                            Response.End();
                        }
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


                try
                {
                    oldImage = System.Drawing.Image.FromFile(Server.MapPath(Request.QueryString["i"]));
                }
                catch
                {
                    Response.End();
                    return;
                }

                bool blnShrinkToThumb = false;
                float webFactor;
                int smallHeight = 1;
                int smallWidth = 1;


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

                            fWaterMarkFontSize = (fWaterMarkFontSize * webFactor * 2);
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



                if (true == blnShrinkToThumb)
                {

                    //Create new (blank) Image object called "shrunkImage" of new smaller size (canvas)
                    shrunkImage = new Bitmap(smallWidth, smallHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    //Create new Graphics object called "oGraphic" based on object "shrunkImage" 
                    //with multiple quality attributes
                    oGraphic = Graphics.FromImage(shrunkImage);
                    oGraphic.CompositingQuality = CompositingQuality.HighSpeed;
                    oGraphic.SmoothingMode = SmoothingMode.HighSpeed;
                    oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    //Set attribute of object "oGraphic" to rectangle of new smaller size (photo on canvas)
                    Rectangle rect = new Rectangle(0, 0, smallWidth, smallHeight);

                    //Draw the old photo in the new rectangle
                    oGraphic.DrawImage(oldImage, rect);


                    //ouputImage = oldImage.GetThumbnailImage(smallWidth, smallHeight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                    ouputImage = shrunkImage;

                }
                else
                {
                    ouputImage = oldImage;
                }


                #region Insert Wartermark

                if (true == _InsertWarterMark)
                {
                    canvas = Graphics.FromImage(ouputImage);
                    wmFont = new Font("Verdana", fWaterMarkFontSize, FontStyle.Bold);

                    string sWaterMark = "© RiverValleyCommunity.org";

                    int nWaterMarkX = ouputImage.Width - 320;
                    int nWaterMarkY = ouputImage.Height - 30;

                    if (ouputImage.Height > ouputImage.Width)
                    {
                        nWaterMarkX = ouputImage.Width + 20;
                        nWaterMarkY = ouputImage.Height + 320;
                    }

                    canvas.DrawString(sWaterMark, wmFont, new SolidBrush(Color.Lime), nWaterMarkX, nWaterMarkY);
                    canvas.DrawString(sWaterMark, wmFont, new SolidBrush(Color.FromArgb(128, 0, 0, 0)), nWaterMarkX + 2, nWaterMarkY + 2);
                    canvas.DrawString(sWaterMark, wmFont, new SolidBrush(Color.FromArgb(128, 255, 255, 255)), nWaterMarkX, nWaterMarkY);
                    canvas.DrawString(sWaterMark, wmFont, new SolidBrush(Color.FromArgb(128, 0, 0, 0)), nWaterMarkX + 2, nWaterMarkY + 2);
                    canvas.DrawString(sWaterMark, wmFont, new SolidBrush(Color.FromArgb(128, 255, 255, 255)), nWaterMarkX, nWaterMarkY);
                }
                #endregion

                Response.ContentType = "image/jpeg";
                ouputImage.Save(Response.OutputStream, ImageFormat.Jpeg);

                lock (imageWriteLock)
                {
                    ouputImage.Save(newFileInfo.FullName, ImageFormat.Jpeg);
                }

            }
            finally
            {
                if (null != oldImage) oldImage.Dispose();
                if (null != shrunkImage) shrunkImage.Dispose();
                if (null != ouputImage) ouputImage.Dispose();
                if (null != oGraphic) oGraphic.Dispose();
                if (null != wmFont) wmFont.Dispose();
                if (null != canvas) canvas.Dispose();
            }

        }
    }
}
