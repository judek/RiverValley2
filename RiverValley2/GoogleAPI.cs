using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Threading;
using System.Security.Cryptography.X509Certificates;




namespace RiverValley2
{
    [Serializable]
    public class JSONCalEvents
    {
        public List<CalEvent> calEvents;
    }

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

}