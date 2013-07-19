using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;

namespace RiverValley2
{
    public class EventNotify
    {

        static ReaderWriterLock _Lock = new ReaderWriterLock();
        //static DateTime _TimetoRun = DateTime.MaxValue;
        static string _spath = null;


        public static void Run(TweetCalendar callerPage)
        {
            callerPage.PrintLine("Starting calendar notify logic", true);
            
            if (null == _spath) 
                _spath = callerPage.Server.MapPath("TimeToCheckCalendar.txt");

            //if (false == IsTimeToRun(callerPage))
            //{
            //    return;
            //}

            TweetEvents(FilterDay(callerPage.CalEvents, 60, callerPage, false), callerPage, "Save the date: "); 
            
            TweetEvents(FilterDay(callerPage.CalEvents, 30, callerPage, false), callerPage, "Save the date: ");

             TweetEvents(FilterDay(callerPage.CalEvents, 14, callerPage, false), callerPage, "Coming soon: ");

             TweetEvents(FilterDay(callerPage.CalEvents, 7, callerPage, false), callerPage, "Next week: ");

             TweetEvents(FilterDay(callerPage.CalEvents, 1, callerPage, true), callerPage, "Tomorrw: ");

           
            //DONE


        }


        static List<CalEvent> FilterDay(List<CalEvent> events, int nDay, TweetCalendar callerPage, bool blnIncludeReacurring)
        {
            DateTime timeCheck = DateTime.Now.AddDays(nDay);

            callerPage.PrintLine("Cheching for events that happen in " + nDay + " days on " + timeCheck.ToString("MM/dd/yyyy") + "...");
           

            List<CalEvent> comingEvents = events.FindAll(delegate(CalEvent c)
            {
                if (false == blnIncludeReacurring)
                    if (c.Recurrence == null)
                        return false;
                
                return ((c.StartDate.Year == timeCheck.Year)
                    && (c.StartDate.Month == timeCheck.Month)
                    && (c.StartDate.Day == timeCheck.Day)
                    );

            });

            callerPage.PrintLine("found " + comingEvents.Count + " events.");
            

            return comingEvents;
        }


        static bool IsTimeToRun(TweetCalendar callerPage)
        {

            callerPage.PrintLine("Time to run file:" + _spath);

            try
            {//Code block for releasing read and write locks

                if (true == System.IO.File.Exists(_spath))
                {
                    DateTime nowTime  = DateTime.Now;
                    try
                    {

                        _Lock.AcquireReaderLock(100);
                        callerPage.PrintLine("Acquired reader lock");
                    }
                    catch 
                    {
                        callerPage.PrintLine("Could not acquire reader lock");
                        return false;
                    }

                    //Set to max value so if parse fails we do not keep notifying
                    DateTime reconsturct = DateTime.MaxValue;
                    try
                    {
                        callerPage.PrintLine("Parsing time from file");
                        reconsturct = DateTime.Parse(System.IO.File.ReadAllText(_spath));
                        callerPage.PrintLine("Go time is:" + reconsturct.ToLongDateString());
                    }
                    catch { }

                    
                    if (DateTime.Now < reconsturct)
                    {
                        callerPage.PrintLine("Not time to run yet");
                        return false;
                    }

                }
                
                //If we reach this point then it is time to run
               
                //Update next run time
                _Lock.UpgradeToWriterLock(10000);
                callerPage.PrintLine("Acquired writer lock");
                DateTime _NextTimetoRun = DateTime.Now.AddDays(1);

                callerPage.PrintLine("Next go time is:" + _NextTimetoRun.ToLongDateString());

                System.IO.File.WriteAllText(_spath, _NextTimetoRun.ToString());


                return true;
              
    
            }
            catch { return false; }
            finally
            {
                if (true == _Lock.IsReaderLockHeld)
                    _Lock.ReleaseReaderLock();

                if (true == _Lock.IsWriterLockHeld)
                    _Lock.ReleaseWriterLock();
            }


           }

        static void TweetEvents(List<CalEvent> eventsToTweet, TweetCalendar callerPage, string sTweetPrefix)
        {
            if (null == eventsToTweet)
                return;
            
            if (null == sTweetPrefix)
                sTweetPrefix = "";
            
            foreach(CalEvent eventToTweet in eventsToTweet)
            {
                string tweet;
                string sURL;

                sURL = callerPage.Request.Url.AbsoluteUri.Replace(callerPage.Request.Path, "/CalendarEvent.aspx?ID=");

               

                if (eventToTweet.IsAllDayEvent)
                    tweet = sTweetPrefix + " " + eventToTweet.Subject + " " + sURL + eventToTweet.ID;
                else
                    tweet = sTweetPrefix + " " + eventToTweet.Subject + " " + eventToTweet.StartTime.ToShortTimeString() + " - " + eventToTweet.EndTime.ToShortTimeString() + " " + sURL + eventToTweet.ID;
                
                
                callerPage.PrintLine(tweet);
                callerPage.AddaTweet(tweet);
            }
        }



    }
}
