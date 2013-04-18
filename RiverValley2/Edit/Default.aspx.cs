using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiverValley2.Edit
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Page.User.Identity.Name.Length > 0)
            //    LiteralCurrentIdentity.Text = Page.User.Identity.Name;
            //else
            //    LiteralCurrentIdentity.Text = "";

            string test = Request.Url.ToString();

            if (true ==  test.Contains(".org"))
                Response.Redirect("https://rivervalleycarpe.readyhosting.com/rivervalley/edit");


            Session["Editing"] = "OK";
        }

        protected void ButtonLogoff_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("../Default.aspx" + "?ticks=" + DateTime.Now.Ticks.ToString());
        }
    }
}
