using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiverValley2
{
    public partial class WomensMinistry : RiverValleyPage2
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LabelMain.Text = GetContent("Body");
            LiteralPictures.Text = GenerateRandomPictures();
        }
    }
}
