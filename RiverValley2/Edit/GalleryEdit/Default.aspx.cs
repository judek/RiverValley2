using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiverValley2.Edit.Gallery
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["EditRiverValleyGallery"] = "OK";
            Response.Redirect("../../Gallery.aspx?ticks=" + DateTime.Now.Ticks.ToString());
        }
    }
}
