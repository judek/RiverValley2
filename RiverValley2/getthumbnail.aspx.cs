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
        bool ThumbnailCallback()
        {
            return true;
        }




        protected void Page_Load(object sender, EventArgs e)
        {

            System.Drawing.Image oldImage;

            try
            {
                oldImage = System.Drawing.Image.FromFile(Server.MapPath(Request.QueryString["i"]));
            }
            catch
            {
                Response.End();
                return;
            }

            int requestedWidth = 100;

            if (null != Request.QueryString["w"])
            {
                try
                {
                    requestedWidth = Int32.Parse(Request.QueryString["w"]);
                }
                catch { }
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

            if (true == blnShrinkToThumb)
                ouputImage = oldImage.GetThumbnailImage(smallWidth, smallHeight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            else
                ouputImage = oldImage;

            Response.ContentType = "image/jpeg";
            ouputImage.Save(Response.OutputStream, ImageFormat.Jpeg);

            oldImage.Dispose();

            if (null != ouputImage)
                oldImage.Dispose();

        }
    }
}
