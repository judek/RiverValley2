using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;

namespace RiverValley2
{
    public class EventNotify
    {
        
        
        
        public static void Run(RiverValleyPage callerPage)
        {
        //            Is time to run


        //NO
        //    DONE

        //Save next time to run

        //Get events
       
                
             //To Do: match day exactly
            List<CalEvent> coming7daysEvents =
                FilterDay(callerPage.CalEvents, 7);
            
            string test = "";
            if (coming7daysEvents.Count > 0)
                test = coming7daysEvents[0].Subject;

            int ntest = test.Length;   
                
            
            int ncoming7daysEvents = coming7daysEvents.Count; 
        
            
           
            //DONE


        }


        static List<CalEvent> FilterDay(List<CalEvent> events, int nDay)
        {
            DateTime timeToRun = DateTime.Now;
           

            List<CalEvent> comingEvents = events.FindAll(delegate(CalEvent c)
            {
                return ((c.StartDate.Year == 2013)
                    && (c.StartDate.Month == 5)
                    && (c.StartDate.Day == 6)
                    );

            });

            return comingEvents;
        }


    }
}
