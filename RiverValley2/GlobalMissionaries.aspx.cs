﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace RiverValley2
{
    public partial class GlobalMissionaries : RiverValleyPage2
    {

       
        protected void Page_Load(object sender, EventArgs e)
        {
            LabelMain.Text = GetContent("Body");
            LiteralPictures.Text = GenerateRandomPictures();

            
        }

       
    }
}
