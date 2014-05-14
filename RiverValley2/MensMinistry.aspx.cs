using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiverValley2
{
    public partial class MensMinistry : RiverValleyPage2
    {
        /// <summary>
        /// Need to override the picture pattern otherwise will pick up
        /// pictures from WomensMinistry was well :)
        /// </summary>
        /// <param name="sPageName"></param>
        /// <returns></returns>
        protected override string GetPictureMatchPattern(string sPageName)
        {
            return sPageName + "*.jpg";
        }
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            LabelMain.Text = GetContent("Body");
            LiteralPictures.Text = GenerateRandomPictures();
        }

    }
}
