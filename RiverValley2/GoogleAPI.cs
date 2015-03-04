using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using System.Security.Cryptography.X509Certificates;




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

    class Authentication
    {


        /// <summary>
        /// Authenticate to Google Using Oauth2
        /// Documentation https://developers.google.com/accounts/docs/OAuth2
        /// </summary>
        /// <param name="clientId">From Google Developer console https://console.developers.google.com</param>
        /// <param name="clientSecret">From Google Developer console https://console.developers.google.com</param>
        /// <param name="userName">A string used to identify a user.</param>
        /// <returns></returns>
        public static CalendarService AuthenticateOauth(string clientId, string clientSecret, string userName)
        {

            string[] scopes = new string[] {
        CalendarService.Scope.Calendar  ,  // Manage your calendars
        CalendarService.Scope.CalendarReadonly    // View your Calendars
            };

            try
            {
                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                    , scopes
                                                                    , userName
                                                                    , CancellationToken.None
                                                                    , new FileDataStore("Daimto.GoogleCalendar.Auth.Store")).Result;



                CalendarService service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Calendar API Sample",
                });
                return service;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("AuthenticateOauth:" + ex.InnerException.Message);
            }

        }

        /// <summary>
        /// Authenticating to Google using a Service account
        /// Documentation: https://developers.google.com/accounts/docs/OAuth2#serviceaccount
        /// </summary>
        /// <param name="serviceAccountEmail">From Google Developer console https://console.developers.google.com</param>
        /// <param name="keyFilePath">Location of the Service account key file downloaded from Google Developer console https://console.developers.google.com</param>
        /// <returns></returns>
        public static CalendarService AuthenticateServiceAccount(string serviceAccountEmail, string keyFilePath)
        {

            // check the file exists
            if (!File.Exists(keyFilePath))
            {
                throw new ApplicationException("AuthenticateServiceAccount: An Error occurred - Key file does not exist");
            }

            string[] scopes = new string[] {
        CalendarService.Scope.Calendar  ,  // Manage your calendars
        CalendarService.Scope.CalendarReadonly    // View your Calendars
            };

            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            try
            {
                ServiceAccountCredential credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = scopes
                    }.FromCertificate(certificate));

                // Create the service.
                CalendarService service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Calendar API Sample",
                });
                return service;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("AuthenticateServiceAccount:" + ex.InnerException.Message);
            }
        }

    }
    
    public class GoogleAPI
    {

        //string SERVICE_ACCOUNT_EMAIL = "414558114750-vj3ttcv5n87sol20e3lshtvuft5p0b3l@developer.gserviceaccount.com";
        //string SERVICE_ACCOUNT_KEYFILE = Server.MapPath(@"./certs/rivervalleycommunity.p12");

        public static List<CalEvent> GetCalendarEvents3(string email, string certFile, string calendarID, DateTime QueryStartDate, DateTime QueryEndDate)
        {
            List<CalEvent> eventList = new List<CalEvent>();

            
            CalendarService service;


            service = Authentication.AuthenticateServiceAccount(email, certFile);

            Events request = null;

            EventsResource.ListRequest lr = service.Events.List(calendarID);

            lr.TimeMin = QueryStartDate;
            lr.TimeMax = QueryEndDate; 
            //This expands repeating pattern if any
            lr.SingleEvents = true;


            request = lr.Execute();
            foreach (var entry in request.Items)
            {

                CalEvent myevent = new CalEvent();

                if ((null == entry.Start.DateTime) || (null == entry.End.DateTime))
                {
                    myevent.IsAllDayEvent = true;
                    myevent.StartTime = DateTime.Parse(entry.Start.Date);
                    myevent.StartDate = DateTime.Parse(entry.Start.Date);

                    myevent.EndTime = DateTime.Parse(entry.End.Date);
                    myevent.EndDate = DateTime.Parse(entry.End.Date);


                }
                else
                {
                    myevent.IsAllDayEvent = false;

                    if (null != entry.Start.DateTime)
                    {
                        myevent.StartTime = (DateTime)entry.Start.DateTime;
                        myevent.StartDate = (DateTime)entry.Start.DateTime;
                    }
                    if (null != entry.End.DateTime)
                    {
                        myevent.EndTime = (DateTime)entry.End.DateTime;
                        myevent.EndDate = (DateTime)entry.End.DateTime;
                    }


                }

                myevent.ID = entry.Id;
                myevent.Location = entry.Location;

                if (null != entry.Recurrence)
                    if (entry.Recurrence.Count > 0)
                    {
                        myevent.Recurrence = entry.Recurrence[0];
                    }

                myevent.Subject = entry.Summary;
                myevent.Details = entry.Description;

                eventList.Add(myevent);


            }


            return eventList;
        }

        public static List<CalEvent> GetCalendarEvents(string userName, string password, string CalendarURL,
           DateTime StartDate, DateTime EndDate, string QueryString)
        {

            List<CalEvent> googleEvents = new List<CalEvent>();
            
            //CalendarService service = new CalendarService("Just another calendar service");

            //if (null == service)
            //    throw new ApplicationException("Google calendare service returned null");
            
            //if ((null != userName) && (null != password))
            //    service.setUserCredentials(userName, password);


            //if (_myProxy != null)
            //{
            //    GDataRequestFactory requestFactory = (GDataRequestFactory)service.RequestFactory;
            //    requestFactory.Proxy = _myProxy;
            //}

            
            
            ////foreach (string CalendarURL in calendarURLs)
            ////{
            //    EventQuery myQuery = new EventQuery(CalendarURL);

            //    //Optional query parameters

            //    //myQuery.TimeZone = "ctz=America/Chicago";
                
            //    //We must use StartTime and EndTime for this query
            //    //If we use StartDate and EndDate it does not work for
            //    //some reason

            //    if (StartDate > DateTime.MinValue)
            //        myQuery.StartTime = StartDate;

            //    if (EndDate > DateTime.MinValue)
            //        myQuery.EndTime = EndDate;


            //    if (null != QueryString)
            //        myQuery.Query = QueryString;

            //    //Just a guess default is 25 which is to low
            //    myQuery.NumberToRetrieve = 1024;

            //    //This is important be cause since we are just displaying all the events
            //    //we want all the reocurring events automatically expanded into their
            //    //own events (myQuery.SingleEvents = true;) default is false
            //    myQuery.SingleEvents = true;

            //    EventFeed myResultsFeed = null;

            //    //try
            //    {
            //        myResultsFeed = service.Query(myQuery);
            //        if (null == myResultsFeed)
            //        {
            //            throw new Exception("Calendar not found");
            //        }

            //    }
            //    //catch (Exception exp)
            //    //{
            //    //    throw new Exception("Error quering calendar:" + CalendarURL +  ":" + exp.Message);
            //    //    //continue;
            //    //}

            //    if (null == myResultsFeed.Entries)
            //    {
            //        throw new Exception("No entries found in calendar feed:" + CalendarURL + ":");
            //        //continue;
            //    }


            //    foreach (EventEntry Entry in myResultsFeed.Entries)
            //    {
            //        CalEvent googleEvent = new CalEvent();

            //        googleEvent.ID = Entry.EventId;
            //        googleEvent.Subject = Entry.Title.Text;
            //        googleEvent.Details = Entry.Content.Content;

            //        foreach (When when in Entry.Times)
            //        {
            //            googleEvent.StartTime = when.StartTime;
            //            googleEvent.StartDate = when.StartTime;
            //            googleEvent.EndTime = when.EndTime;
            //            googleEvent.EndDate = when.EndTime; 

            //            //Fix time zone we want to display cst but server is est
            //            googleEvent.StartTime = googleEvent.StartTime.AddHours(-1);
            //            //googleEvent.StartDate = googleEvent.StartDate.AddHours(-1);
            //            googleEvent.EndTime = googleEvent.EndTime.AddHours(-1);
            //            //googleEvent.EndDate = googleEvent.EndDate.AddHours(-1);

            //        }

            //        foreach (Where where in Entry.Locations)
            //        {
            //            googleEvent.Location += where.ValueString;
            //        }

            //        if (null != Entry.Recurrence)
            //            googleEvent.Recurrence = Entry.Recurrence.Value;

                    
            //        googleEvent.IsPrivate = false;
            //        googleEvent.IsAllDayEvent = googleEvent.Duration.Days > 0;

            //        googleEvents.Add(googleEvent);

            //    }

            ////}


            return googleEvents;


        }
    }
}
