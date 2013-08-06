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
            Literal1.Text = "Loading...";
            EventNotify.Run(this);
        }

        public void PrintLine(string line)
        {
            PrintLine(line, false);
        }
        
        public void PrintLine(string line, bool blnTimeStamp)
        {
            if(true == blnTimeStamp)
                //Response.Write("<br>" + DateTime.Now + " : " + line);
                Literal1.Text += ("<br>" + DateTime.Now + " : " + line);

            else
                //Response.Write("<br>" + line);
                Literal1.Text += ("<br>" + line);
        }

        internal void AddaTweet(string stweet)
        {
            AddTweet(stweet);
        }
    }
}
