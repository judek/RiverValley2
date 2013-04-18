using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiverValley2
{
    public partial class Util1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Mobile"] != null)
            {
                Session["InMobileMode"] = true;
            }

            if (Request.QueryString["Full"] != null)
            {
                Session["InMobileMode"] = false;
            }

            Response.Redirect("About.aspx");
        }
    }
}
