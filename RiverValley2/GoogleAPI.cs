using System;
using System.Collections.Generic;
using System.Web;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;
using System.Net;

namespace RiverValley2
{

    [Serializable]
    public class CalEvent
    {
        public string ID = "";
        public string Subject = "";
        public string Details = "";
        public string Location = "";
        public DateTime StartDate = DateTime.MinValue;
        public DateTime EndDate = DateTime.MinValue;
        public DateTime StartTime = DateTime.MinValue;
        public DateTime EndTime = DateTime.MinValue;
        public bool IsAllDayEvent;
        public bool IsPrivate;
        public TimeSpan Duration
        {
            get { return EndTime - StartTime; }
        }

        public string Recurrence = null;
    }
    
    public class GoogleAPI
    {
        //static WebProxy _myProxy = new WebProxy("proxy.taltrade.com:8080");
        static WebProxy _myProxy = null;

        public static List<CalEvent> GetCalendarEvents(string userName, string password, string CalendarURL,
           DateTime StartDate, DateTime EndDate, string QueryString)
        {

            CalendarService service = new CalendarService("Just another calendar service");

            if (null == service)
                throw new ApplicationException("Google calendare service returned null");
            
            if ((null != userName) && (null != password))
                service.setUserCredentials(userName, password);


            if (_myProxy != null)
            {
                GDataRequestFactory requestFactory = (GDataRequestFactory)service.RequestFactory;
                requestFactory.Proxy = _myProxy;
            }

            List<CalEvent> googleEvents = new List<CalEvent>();
            
            //foreach (string CalendarURL in calendarURLs)
            //{
                EventQuery myQuery = new EventQuery(CalendarURL);

                //Optional query parameters

                //myQuery.TimeZone = "ctz=America/Chicago";
                
                //We must use StartTime and EndTime for this query
                //If we use StartDate and EndDate it does not work for
                //some reason

                if (StartDate > DateTime.MinValue)
                    myQuery.StartTime = StartDate;

                if (EndDate > DateTime.MinValue)
                    myQuery.EndTime = EndDate;


                if (null != QueryString)
                    myQuery.Query = QueryString;

                //Just a guess default is 25 which is to low
                myQuery.NumberToRetrieve = 1024;

                //This is important be cause since we are just displaying all the events
                //we want all the reocurring events automatically expanded into their
                //own events (myQuery.SingleEvents = true;) default is false
                myQuery.SingleEvents = true;

                EventFeed myResultsFeed = null;

                //try
                {
                    myResultsFeed = service.Query(myQuery);
                    if (null == myResultsFeed)
                    {
                        throw new Exception("Calendar not found");
                    }

                }
                //catch (Exception exp)
                //{
                //    throw new Exception("Error quering calendar:" + CalendarURL +  ":" + exp.Message);
                //    //continue;
                //}

                if (null == myResultsFeed.Entries)
                {
                    throw new Exception("No entries found in calendar feed:" + CalendarURL + ":");
                    //continue;
                }


                foreach (EventEntry Entry in myResultsFeed.Entries)
                {
                    CalEvent googleEvent = new CalEvent();

                    googleEvent.ID = Entry.EventId;
                    googleEvent.Subject = Entry.Title.Text;
                    googleEvent.Details = Entry.Content.Content;

                    foreach (When when in Entry.Times)
                    {
                        googleEvent.StartTime = when.StartTime;
                        googleEvent.StartDate = when.StartTime;
                        googleEvent.EndTime = when.EndTime;
                        googleEvent.EndDate = when.EndTime; 

                        //Fix time zone we want to display cst but server is est
                        googleEvent.StartTime = googleEvent.StartTime.AddHours(-1);
                        //googleEvent.StartDate = googleEvent.StartDate.AddHours(-1);
                        googleEvent.EndTime = googleEvent.EndTime.AddHours(-1);
                        //googleEvent.EndDate = googleEvent.EndDate.AddHours(-1);

                    }

                    foreach (Where where in Entry.Locations)
                    {
                        googleEvent.Location += where.ValueString;
                    }

                    if (null != Entry.Recurrence)
                        googleEvent.Recurrence = Entry.Recurrence.Value;

                    
                    googleEvent.IsPrivate = false;
                    googleEvent.IsAllDayEvent = googleEvent.Duration.Days > 0;

                    googleEvents.Add(googleEvent);

                }

            //}


            return googleEvents;


        }
    }
}
