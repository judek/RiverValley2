using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiverValley2
{
    public partial class execute :  RiverValleyPage2
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Literal1.Text = "Dumping calender cache...";
            DropCalendarCache();
            Literal1.Text += "<br />Operation complete.<a href=Calendar.aspx>Click here to continue</a> ";
        }
    }
}