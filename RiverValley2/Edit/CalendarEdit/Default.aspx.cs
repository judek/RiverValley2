using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiverValley2.Edit.CalendarEdit
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string sIndentity;
            
            if (Page.User.Identity.Name.Length > 0)
                sIndentity = Page.User.Identity.Name;
            else
                sIndentity = "";


            Response.Cookies["UserName"].Value = sIndentity;
            Response.Cookies["UserName"].Expires = DateTime.Now.AddMinutes(20);

            //Invoke legacy Calendar Editor
            //Response.Redirect("../CalendarEditor/Calendar.asp");
        }
    }
}
