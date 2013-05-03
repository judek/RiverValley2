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
            callerPage.PrintLine("Starting calendar tweet logic");
            
            if (null == _spath) 
                _spath = callerPage.Server.MapPath("TimeToCheckCalendar.txt");

            if (false == IsTimeToRun(callerPage))
            {

                return;
            }
           
       

            List<CalEvent> coming7daysEvents =
                FilterDay(callerPage.CalEvents, 7);

            List<CalEvent> coming3daysEvents =
                FilterDay(callerPage.CalEvents, 3);
            
           
            
            
            string test = "";
            if (coming7daysEvents.Count > 0)
                test = coming7daysEvents[0].Subject;

            int ntest = test.Length;   
                
            
            int ncoming7daysEvents = coming7daysEvents.Count; 
        
            
           
            //DONE


        }


        static List<CalEvent> FilterDay(List<CalEvent> events, int nDay)
        {
            DateTime timeCheck = DateTime.Now.AddDays(nDay);
           

            List<CalEvent> comingEvents = events.FindAll(delegate(CalEvent c)
            {
                return ((c.StartDate.Year == timeCheck.Year)
                    && (c.StartDate.Month == timeCheck.Month)
                    && (c.StartDate.Day == timeCheck.Day)
                    );

            });

            return comingEvents;
        }


        static bool IsTimeToRun(TweetCalendar callerPage)
        {

            callerPage.PrintLine("Time to run file:" + _spath);

            try
            {

                if (true == System.IO.File.Exists(_spath))
                {
                    DateTime nowTime  = DateTime.Now; 
                    _Lock.AcquireReaderLock(100);
                    callerPage.PrintLine("Acquired reader lock");


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
                    
                _Lock.UpgradeToWriterLock(10000);
                callerPage.PrintLine("Acquired writer lock");


                DateTime _NextTimetoRun = DateTime.Now.AddDays(1);

                System.IO.File.WriteAllText(_spath, _NextTimetoRun.ToString());
                

                // 
                 //_TimetoRun = reconsturct;

                //_TimetoRun = _NextTimetoRun;
                //callerPage.PrintLine("Next time to run will be:" + _TimetoRun.ToString());
                
              
                




            }
            catch { return false; }
            finally
            {
                if (true == _Lock.IsReaderLockHeld)
                    _Lock.ReleaseReaderLock();

                if (true == _Lock.IsWriterLockHeld)
                    _Lock.ReleaseWriterLock();
            }


            return false;
        }



    }
}
