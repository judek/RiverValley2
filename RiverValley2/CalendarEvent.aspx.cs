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
    public partial class CalendarEvent : RiverValleyPage2
    {
        //int nEventID;
        bool _CanEditCalendar;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //LiteralPictures.Text = GenerateRandomPictures();


            _CanEditCalendar = (null != Session["EditRiverValleyCalendar"]);

            if (Request.QueryString["ID"] == null)
            {

                if (true == _CanEditCalendar)
                    Response.Redirect("CalendarEventEdit.aspx");
                
                return;
            }

            if (true == _CanEditCalendar)
                Response.Redirect("CalendarEventEdit.aspx?ID=" + Request.QueryString["ID"]);

            int Year = 0, Month = 0, Day = 0;

            string stest = Request.QueryString["ID"];
            try
            {
                if (Request.QueryString["Y"] != null) Year = Int32.Parse(Request.QueryString["Y"]);
                if (Request.QueryString["M"] != null) Month = Int32.Parse(Request.QueryString["M"]);
                if (Request.QueryString["D"] != null) Day = Int32.Parse(Request.QueryString["D"]);
            }
            catch { }//do nothing event just won't be found if parsing date parsing fails

            //Event IDs are no longer ints
            //try { nEventID = Int32.Parse(Request.QueryString["ID"]); }
            //catch { return; }


            //DataRow[] drs = CalendarEvents.Tables[0].Select(string.Format("ID = {0}", nEventID));
            
            CalEvent calEvent = CalEvents.Find(delegate(CalEvent c)
            {
                return ((c.ID == stest)
                && (c.StartDate.Year == Year)
                && (c.StartDate.Month == Month)
                && (c.StartDate.Day == Day)
                );
            });


            if (null == calEvent)
            {
                LabelMain.Text = "Event Not Found";
                return;
            }

            //if (drs.Length != 1)
            //{
            //    LabelMain.Text = "Unexpected number of events";
            //    return;
            //}


            //if ((int)drs[0]["CollectionID"] == -1)
            //    LiteralDate.Text = ((DateTime)drs[0]["EventDate"]).ToLongDateString();
            //else
            //{

            //    LiteralDate.Text = ((DateTime)drs[0]["EventDate"]).ToLongDateString();
            //}

            LiteralDate.Text = calEvent.StartDate.ToLongDateString();

            if (false == calEvent.IsAllDayEvent)
            {
                //DateTime StartTime = (DateTime)drs[0]["EventTime"];
                //DateTime EndTime = (DateTime)drs[0]["EventTime"];
                //EndTime = EndTime.AddHours((int)drs[0]["LengthHrs"]);
                //EndTime = EndTime.AddMinutes((int)drs[0]["LengthMins"]);

                LiteralTime.Text = calEvent.StartTime.ToShortTimeString() + " - " + calEvent.EndTime.ToShortTimeString();
            }
            else
                LiteralTime.Text = "This ia an all day event";



            LabelTitle.Text = FormatPageTitle(calEvent.Subject);
            //Label1.Text = drs[0]["Subject"] as string;
            //LabelDetails.Text = drs[0]["Details"] as string;
            //TextBoxBody.Text = drs[0]["Details"] as string;

            //LabelMain.Text = LabelTitle.Text;

            string sDetails = "";

            if (null != calEvent.Location)
            if (calEvent.Location.Length > 1)
                sDetails += "Location: " + calEvent.Location + "<a target=_blank href=http://maps.google.com?q=" + System.Web.HttpUtility.UrlEncode(calEvent.Location.Trim()) + "> view map</a>" + "<br /><br />";

            
            sDetails += calEvent.Details;


            


            if (null != sDetails)
                //LabelMain.Text = sDetails.Replace("\r\n", "<br />");
                LabelMain.Text = ContentReader.FormatTextBlock(sDetails);


        }
    }
}
