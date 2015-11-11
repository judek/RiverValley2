using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;
using System.Data;
using System.IO;

namespace RiverValley2
{
    public partial class Default : RiverValleyPage2
    {
        const int MAX_SERMON_DISPLAY = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (MasterPageFile.Contains("Mobile"))
                Response.Redirect("About.aspx");

            //if (MasterPageFile.Contains("2014"))
                //Response.Redirect("Util.aspx?Full=true");
            

            //Util.aspx?Full=true
            LiteralBanner.Text =  GetRandomBanner();
            
            
            #region Coming Events
            const int MAX_EVENTS_TO_SHOW = 3;
            int nEventshown = 0;


            DateTime now = DateTime.Now.AddDays(-1);
            
            //DataRow[] drs = CalendarEvents.Tables[0].Select(string.Format("EventDate > '{0}-{1}-{2}'", now.Day, now.ToString("MMM"), now.Year));
            List<CalEvent> currentEvents = CalEvents.FindAll(delegate(CalEvent c) { return c.StartTime > now; });

            currentEvents.Sort(delegate(CalEvent d1, CalEvent d2)
            {
                return DateTime.Compare(d1.StartTime, d2.StartTime);
            });

            List<string> UsedSubjects = new List<string>();

            now = DateTime.Now;
            
            //if (drs.Length > 0)
            //    LiteralComingEvents.Text = "<span class=\"title\">Coming<br />Events</span>";

            for (int i = 0; i < currentEvents.Count; i++)
            {
                string sEventSubject = currentEvents[i].Subject;

                if (null == sEventSubject)
                    continue;//Do not display

                if (null != UsedSubjects.Find(delegate(string sub) { return sub == sEventSubject; }))
                    continue;//already displayed we don't want to show a list of repeating events.

                UsedSubjects.Add(sEventSubject);
                nEventshown++;

                //string slink = "<a href=viewEvent.aspx?ID=" + drs[i]["ID"] + "&ColID=207 onclick=\"window.open(this.href,'newWindow','width=400,height=400', 'modal');return false\">";
                string slink = "<a href=CalendarEvent.aspx?ID=" + currentEvents[i].ID + "&Y=" + currentEvents[i].StartDate.Year + "&M=" + currentEvents[i].StartDate.Month + "&D=" + currentEvents[i].StartDate.Day + ">";

                LiteralComingEvents.Text += "<b>" + slink + StringTrunk(sEventSubject, 25) + "...</a></b>";
                //LiteralComingEvents.Text += "<br /><br /><br />" + slink + sEventSubject + "</a><br /><br />";

                DateTime EventDate = currentEvents[i].StartDate;

                //if ((now.Day == EventDate.Day) && (now.Month == EventDate.Month) && (now.Year == EventDate.Year))
                //    LiteralComingEvents.Text += EventDate.ToLongDateString() + " <font color=\"#FF0000\"><b>(Today)</b></font><br />";
                //else
                //    LiteralComingEvents.Text += "<span class=\"eventTime\">" + EventDate.ToLongDateString() + " - ";

                

                //if ((int)drs[i]["IsAllDayEvent"] == 0)
                //{
                //    DateTime StartTime = (DateTime)drs[i]["EventTime"];
                //    DateTime EndTime = (DateTime)drs[i]["EventTime"];
                //    EndTime = EndTime.AddHours((int)drs[i]["LengthHrs"]);
                //    EndTime = EndTime.AddMinutes((int)drs[i]["LengthMins"]);

                //    LiteralComingEvents.Text += StartTime.ToShortTimeString() + " - " + EndTime.ToShortTimeString() + "</span>";
                //}
                //else
                //    LiteralComingEvents.Text += "All Day</span>";

                LiteralComingEvents.Text += " " + EventDate.ToShortDateString() + "<br />";

                if (nEventshown >= MAX_EVENTS_TO_SHOW)
                    break;
            }


            #endregion


            #region Sermon Archive
            DisplaySermons();
            
            #endregion
        }

        void DisplaySermons()
        {
            List<Document> DocumentList = Cache.Get("cache.RiverValley.MultimediaFiles") as List<Document>;

            if (null == DocumentList)
            {
                //Try reloading cache just in case it died.
                ForceMultimediaCacheReload();

            }

            //Try again after force reload
            DocumentList = Cache.Get("cache.RiverValley.MultimediaFiles") as List<Document>;

            if (null == DocumentList)
                return;//In this case we really have no cache.

            //List only the sermons
            List<Document> SermonList = 
                DocumentList.FindAll(delegate(Document d) { return d.Tags.HasTag("sermon"); });

            if (null == SermonList)
                return;
            
            int nDisplayedSermons = 0;
            foreach (MultimediaFile d in SermonList)
            {
                string slink;
                if (d.MultimediaType == MultimediaType.audio)
                {
                    
                    slink = "<a href=MultimediaPlay.aspx?FL=" + Document.MULTIMEDIA_FOLDER + "&F=" + d.Link + "&T=" + d.MultimediaType + "&W=380&H=50" + "&plugins=spectrumvisualizer-1" + ">";
                }
                else if (d.MultimediaType == MultimediaType.video)
                {
                    slink = "<a href=MultimediaPlay.aspx?FL=" + Document.MULTIMEDIA_FOLDER + "&F=" + d.Link + "&T=" + d.MultimediaType + "&W=640&H=388" + ">";
                }
                else
                {
                    slink = "<a href\"\">";
                }
                
                
                
                //<!--  <a href="">James 2:14-26</a> - Andy Morgan<br />
                if (nDisplayedSermons >= MAX_SERMON_DISPLAY)
                    break;

                LiteralSermons.Text += slink + StringTrunk(d.Title, 25) + "...</a> " + d.Dated.ToString("MM/dd/yyyy") + "<br />";
                nDisplayedSermons++;

                
            }
        }

        string GetRandomBanner()
        {
            DirectoryInfo dinfo = new DirectoryInfo(Server.MapPath(".\\Images"));
            FileInfo[] files = dinfo.GetFiles("Banner*.jpg");

            int random = RandomNumber(0, files.Length - 1);

            //<img src=\"images/JimiAurora.jpg\" alt=\"\" />

            return "<img src=\"images/" + files[random].Name + "\" alt=\"River Valley Community Church\" class=\"shadowMe\" />";

        }
          
        
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
