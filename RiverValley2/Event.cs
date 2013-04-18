using System;
using System.Collections.Generic;
using System.Web;

namespace RiverValley2
{
    public class Event
    {
        int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }


        string _subject;
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        string _details;
        public string Details
        {
            get { return _details; }
            set { _details = value; }
        }


        DateTime _eventDate;
        public DateTime EventDate
        {
            get { return _eventDate; }
            set { _eventDate = value; }
        }

        bool _isAllDayEvent;
        public bool IsAllDayEvent
        {
            get { return _isAllDayEvent; }
            set { _isAllDayEvent = value; }
        }

        int _lengthMins;
        public int LengthMins
        {
            get { return _lengthMins; }
            set { _lengthMins = value; }
        }





    }
}
