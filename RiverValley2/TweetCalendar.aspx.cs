using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiverValley2
{
    public partial class TweetCalendar : RiverValleyPage2
    {
        public  void Page_Load(object sender, EventArgs e)
        {
            EventNotify.Run(this);
        }

        public void PrintLine(string line)
        {
            Response.Write("<br>" + DateTime.Now + " : " + line);

        }
    }
}
