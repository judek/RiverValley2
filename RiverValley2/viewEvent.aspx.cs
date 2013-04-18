using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using System.Web.Caching;
using System.Text;

namespace RiverValley2
{
    public partial class viewEvent : RiverValleyPage
    {

        int nEventID;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["ID"] == null)
                return;

            string stest = Request.QueryString["ID"];

            try { nEventID = Int32.Parse(Request.QueryString["ID"]); }
            catch { return; }


            DataRow[] drs = OLDCalendarEvents.Tables[0].Select(string.Format("ID = {0}", nEventID));


            if (null == drs)
            {
                LabelSubject.Text = "Event Not Found";
                return;
            }

            if (drs.Length != 1)
            {
                LabelSubject.Text = "Unexpected number of events";
                return;
            }


            if ((int)drs[0]["CollectionID"] == -1)
                LabelDate.Text = ((DateTime)drs[0]["EventDate"]).ToLongDateString();
            else
            {

                LabelDate.Text = ((DateTime)drs[0]["EventDate"]).ToLongDateString();
            }

            if ((int)drs[0]["IsAllDayEvent"] == 0)
            {
                DateTime StartTime = (DateTime)drs[0]["EventTime"];
                DateTime EndTime = (DateTime)drs[0]["EventTime"];
                EndTime = EndTime.AddHours((int)drs[0]["LengthHrs"]);
                EndTime = EndTime.AddMinutes((int)drs[0]["LengthMins"]);

                LabelTimeSpan.Text = StartTime.ToShortTimeString() + " - " + EndTime.ToShortTimeString();
            }
            else
                LabelTimeSpan.Text = "All Day";



            LabelSubject.Text = drs[0]["Subject"] as string;
            //Label1.Text = drs[0]["Subject"] as string;
            //LabelDetails.Text = drs[0]["Details"] as string;
            //TextBoxBody.Text = drs[0]["Details"] as string;

            LiteralPageTitle.Text = LabelSubject.Text;

            string sDetails = drs[0]["Details"] as string;

            if (null != sDetails)
                LiteralDetails.Text = sDetails.Replace("\r\n", "<br />");



        }



    }
}
