using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;

namespace RiverValley2
{
    public class EventNotify
    {
        
        
        
        static void Run(RiverValleyPage callerPage)
        {
        //            Is time to run

            DateTime timeToRun = DateTime.Now;

        //NO
        //    DONE

        //Save next time to run

        //Get events
        List<CalEvent>  events =  callerPage.CalEvents;

        //tweet new events, events in one week, event next day.
        List<CalEvent> newEvents = events.FindAll(delegate(CalEvent c) { return c.StartDate > timeToRun; });
        //To Do: match day exactly
        List<CalEvent> coming7daysEvents = events.FindAll(delegate(CalEvent c) { return c.StartDate > timeToRun.AddDays(7); });
        List<CalEvent> coming3daysEvents = events.FindAll(delegate(CalEvent c) { return c.StartDate > timeToRun.AddDays(3); });

        //DONE


        }
    }
}
