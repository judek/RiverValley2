using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using System.Web.Caching;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Text;
using System.IO;

namespace RiverValley2
{
   

    
    public class RiverValleyPage : System.Web.UI.Page
    {
        DbConnection _DbConnection;
        DataSet _CalendarEvents;
        protected const string DATBASE_PATH = "~/database/RiverValleyCalendar.mdb";

        
        protected List<string> _GoogleCalendarURLs;



        public List<CalEvent> CalEvents
        {
            
            get
            {
                //return new List<CalEvent>();
              
                //Had to create this short circuit because the version of Google API has been deprecated.



                List<CalEvent>  calevents = Cache.Get("cache.CalEvents.RiverValley") as List<CalEvent>;
                if (null != calevents)
                {
                    //markRepeatingEvents(calevents);
                    return calevents;
                }

                string HardCacheName = Server.MapPath(".") + "\\RiverValleyCalEvents.xml";

                calevents = new List<CalEvent>();


                DateTime now = DateTime.Now.AddDays(-30);
                DateTime nowEnd = DateTime.Now.AddYears(1);



                try
                {
                    //#region Legacy Database Fetch
                    //DataRow[] drs;
                    //drs = OLDCalendarEvents.Tables[0].Select(string.Format("EventDate > '{0}-{1}-{2}'", now.Day, now.ToString("MMM"), now.Year));
                    //CalEvent calevent;
                    //foreach (DataRow dr in drs)
                    //{
                    //    calevent = new CalEvent();
                    //    calevent.ID = ((int)dr["ID"]).ToString();
                    //    calevent.Subject = dr["Subject"] as string;
                    //    calevent.Details = dr["Details"] as string;

                    //    calevent.StartDate = (DateTime)dr["EventDate"];
                    //    calevent.StartTime = (DateTime)dr["EventDate"];

                    //    calevent.StartTime = calevent.StartTime.AddHours(((DateTime)dr["EventTime"]).Hour);
                    //    calevent.StartTime = calevent.StartTime.AddMinutes(((DateTime)dr["EventTime"]).Minute);


                    //    calevent.EndTime = (DateTime)dr["EventTime"];
                    //    calevent.EndTime = calevent.EndTime.AddHours((int)dr["LengthHrs"]);
                    //    calevent.EndTime = calevent.EndTime.AddMinutes((int)dr["LengthMins"]);
                    //    calevent.EndDate = calevent.EndTime;
                    //    calevent.IsAllDayEvent = ((int)dr["IsAllDayEvent"] > 0);

                    //    calevents.Add(calevent);
                    //}
                    //#endregion

                    #region Google Calendar database fetch

                    //calevents = GoogleAPI.GetCalendarEvents("rivervalleycommunity", "holybible1",
                    //    "https://www.google.com/calendar/feeds/rivervalleycommunity@gmail.com/public/full",
                    //    now, nowEnd, null);


                    
                    string SERVICE_ACCOUNT_EMAIL = "414558114750-vj3ttcv5n87sol20e3lshtvuft5p0b3l@developer.gserviceaccount.com";
                    string SERVICE_ACCOUNT_KEYFILE = Server.MapPath(@"./certs/rivervalleycommunity.p12");


                    calevents = GoogleAPI.GetCalendarEvents3(SERVICE_ACCOUNT_EMAIL, SERVICE_ACCOUNT_KEYFILE, "rivervalleycommunity@gmail.com", now, nowEnd);

                    if (null != calevents)
                        if (calevents.Count < 1)
                            throw new ApplicationException("Calendar returned zero events");

                    //blnCreateBackup = true;

                    
                    //markRepeatingEvents(calevents);

                    #endregion

                }
                catch (Exception exp)
                {//If anything happens to during a calendar database refresh we will have to rely on latest local
                    // harch cache back up. In the case of a could database like google calendar this may
                    // ocurr from time to time if the could is not available.

                    if (true == File.Exists(HardCacheName))
                        calevents = XMLUtil.Deserialize<List<CalEvent>>(File.ReadAllText(HardCacheName));


                    try
                    {
                        //Alert some one of the potential problem
                        using (MailMessage AlertMessage = new MailMessage())
                        {
                            AlertMessage.Subject = "Calendar Error:" + exp.Message;
                            AlertMessage.Body = "Calendar Failure:" + exp.StackTrace;
                            AlertMessage.To.Add("judek@yahoo.com");
                            SendMailMessage(AlertMessage);
                        }
                    }
                    catch
                    {
                        //really cant do anything if we die here
                    }
                }
                finally
                {

                }


                Cache.Insert("cache.CalEvents.RiverValley", calevents,
                   null,
                   //DateTime.Now.AddMinutes(15), TimeSpan.Zero);
                   DateTime.Now.AddHours(1), TimeSpan.Zero);

                //Save to disk in case we loose cache and cannot access database
                //This way we will have something to show for events even if not latest
                File.WriteAllText(HardCacheName, XMLUtil.Serialize<List<CalEvent>>(calevents));

                //#region Backup Calendar Database
                ////Lets save a restorable backup of the entire calendar data base
                ////The back file names will be recycled everymonth so we will keep 30 days of backups
                //if (false == blnCreateBackup)
                //    return calevents;
                
                //if (0 != DateTime.Now.Day % 2) //back up only even dates
                //    return calevents;

                //string backupFileName = Server.MapPath(".") + "\\RiverValleyCalendar" + DateTime.Now.ToString("dd") + ".ics";
                //FileInfo backupFile = new FileInfo(backupFileName);

                

                //if (true == backupFile.Exists)
                //    if (backupFile.LastWriteTime > DateTime.Now.AddDays(-2))
                //        return calevents; //back up not neededed.


                ////Get and wirite backup
                //#endregion


                return calevents;
            }
        }

        //void markRepeatingEvents(List<CalEvent> calEvents)
        //{
        //    List<string> subjects = new List<string>();

        //    List<string> RepeatiungEvents = new List<string>();



            
        //    foreach (CalEvent calEvent in calEvents)
        //    {

        //        string s = subjects.Find(delegate(string c)
        //        {
        //            return (c == calEvent.Subject);
        //        });


        //        if (null == s)
        //            subjects.Add(calEvent.Subject);
        //        else
        //            calEvent.Recurrence = "True";


                
        //        /* List<CalEvent> calEvents1 = CalEvents.FindAll(delegate(CalEvent c)
        //        {
        //            return (c.ID == calEvent.Subject);
        //        });

        //        if (calEvents1.Count > 1)
        //            calEvent.Recurrence = "True";
        //        */

        //    }
   


        //}

        public virtual DataSet OLDCalendarEvents
        {

            get
            {
                _CalendarEvents = Cache.Get("cache.CalendarEvents.RiverValley") as DataSet;
                if (null != _CalendarEvents)
                    return _CalendarEvents;


                //Refill cache then return events
                _DbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                    Server.MapPath(DATBASE_PATH) + ";User Id=admin;Password=;");
                _DbConnection.Open();

                DateTime dFilter = DateTime.Now.AddMonths(-1);
                string sDateFilter = string.Format(" WHERE EventDate > #{0}#", dFilter.ToShortDateString());
                string sql = "SELECT * FROM CalendarEvents" + sDateFilter;

                DataAdapter da = new OleDbDataAdapter(sql, (OleDbConnection)_DbConnection);
                _CalendarEvents = new DataSet();
                da.Fill(_CalendarEvents);
                _DbConnection.Close();

                Cache.Insert("cache.CalendarEvents.RiverValley", _CalendarEvents,
                    new CacheDependency(Server.MapPath(DATBASE_PATH)),
                    DateTime.Now.AddHours(1), TimeSpan.Zero);


                return _CalendarEvents;

            }

        }

        protected virtual string GetContent(string sPortion)
        {
            string sContent = "";


            string sFileName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);

            if (false == sFileName.EndsWith(".aspx"))
                throw new ApplicationException("CafePage.GetContent():Unable to parse page name from url path");


            sFileName = sFileName.Replace(".aspx", "");

            string sSuffix = sFileName + "." + sPortion + ".txt";

            string sPrefix;

            string sPreviewSection = Request.QueryString["section"];
            if (null == sPreviewSection)
                sPreviewSection = sPortion;//To support backward compatibility, otherwise we would set to just ""

            if ((Request.QueryString["preview"] != null) && (sPortion.ToLower() == sPreviewSection.ToLower()))
                sPrefix = "./RiverValleyContent/Preview.";
            else
            {
                sPrefix = "./RiverValleyContent/Content.";
                sContent = Cache.Get(("cache." + (sPrefix + sSuffix))) as string;
                if (sContent != null)
                    return sContent;
            }

            try
            {
                sContent = ContentReader.GetContent(Server.MapPath(sPrefix + sSuffix));

                Cache.Insert(("cache." + (sPrefix + sSuffix)), sContent,
                new CacheDependency(Server.MapPath((sPrefix + sSuffix))),
                DateTime.Now.AddHours(1), TimeSpan.Zero);


                return sContent;
            }
            catch
            {
                return sContent;
            }





        }

        protected string StringTrunk(string mystring, int nMaxLength)
        {
            return mystring.Substring(0, Math.Min(mystring.Length, nMaxLength));
        }

        public static bool TestEmailRegex(string emailAddress)
        {
            //                string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            //                Regex reLenient = new Regex(patternLenient);
            string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"
                  + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                  + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                  + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                  + @"[a-zA-Z]{2,}))$";
            Regex reStrict = new Regex(patternStrict);


            //                      bool isLenientMatch = reLenient.IsMatch(emailAddress);
            //                      return isLenientMatch;


            bool isStrictMatch = reStrict.IsMatch(emailAddress);
            return isStrictMatch;


        }

        protected string GetRawContent(string FileName, string sDefault)
        {
            string sout = sDefault;

            try { sout = File.ReadAllText(FileName); }
            catch { }

            return sout;
        }


        protected int ExecuteSQLNonQuery(string dbPath, string SQL)
        {
            OleDbCommand cmd = null;
            try
            {
                cmd = new OleDbCommand(SQL);

                return ExecuteSQLNonQuery(dbPath, cmd);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (null != cmd)
                    cmd.Dispose();
            }


        }


        protected DataSet ExecuteSQLQuery(string dbPath, string SQL)
        {

            OleDbCommand cmd = null;
            try
            {
                cmd = new OleDbCommand(SQL);

                return ExecuteSQLQuery(dbPath, cmd);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (null != cmd)
                    cmd.Dispose();
            }

        }


        protected DataSet ExecuteSQLQuery(string dbPath, OleDbCommand cmd)
        {
            DataSet dataset = new DataSet();

            OleDbConnection dbConnection = null;
            OleDbDataAdapter dap = null;


            try
            {
                lock (this)
                {

                    dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                           Server.MapPath(dbPath) + ";User Id=admin;Password=;");

                    dap = new OleDbDataAdapter(cmd);

                    cmd.Connection = dbConnection;

                    dbConnection.Open();

                    dap.Fill(dataset);


                    return dataset;


                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }
            finally
            {
                if (null != dbConnection)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }

                if (null != dap)
                {
                    dap.Dispose();
                }

            }


        }


        protected int ExecuteSQLNonQuery(string dbPath, OleDbCommand cmd)
        {
            OleDbConnection dbConnection = null;

            int nResult = 0;

            try
            {
                lock (this)
                {

                    dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                           Server.MapPath(dbPath) + ";User Id=admin;Password=;");

                    cmd.Connection = dbConnection;

                    dbConnection.Open();

                    nResult = cmd.ExecuteNonQuery();

                    dbConnection.Close();
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }
            finally
            {
                if (null != dbConnection)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }

            return nResult;
        }

        protected string StripString(string targetString)
        {//Simple method in C# to strip non-alphanumeric letters
            return System.Text.RegularExpressions.Regex.Replace(targetString, "[^A-Za-z0-9.]", "");
        }

        protected void ForceMultimediaCacheReload()
        {
            string sThisFileName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            string MultimediaPageURL = (Request.Url.AbsoluteUri).Replace(sThisFileName, "multimedia.aspx");
            string MultimediaPath = MultimediaPageURL.Replace(".aspx", "/");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(MultimediaPageURL);
            try
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch
            {//If that didn't work then go there directly
                Response.Redirect("multimedia.aspx");
            }
        }

        protected void AddTweet(string tweet)
        {
            if (null == Session["Tweets"])
                Session["Tweets"] = new List<string>();

            List<string> tweets = Session["Tweets"] as List<string>;

            if (null != tweets.Find(delegate(string t) { return t.ToLower() == tweet.ToLower(); }))
                return;

           
            tweets.Add(tweet);
           
        }


        public static void SendMailMessage(MailMessage mail)
        {
 
            try
            {
                //set the addresses
                if (null == mail.From)
                    mail.From = new MailAddress("\"River Valley Community Church - Do not reply\" <donotreply@rivervalleycommunity.org>");
                    //mail.From = new MailAddress(sFrom);

                //send the message
                SmtpClient smtp = new SmtpClient("mail.rivervalleycommunity.org");

                System.Net.NetworkCredential SmtpUser;
                  SmtpUser = new System.Net.NetworkCredential();
                    SmtpUser.UserName = "donotreply@rivervalleycommunity.org";
                    SmtpUser.Password = "45574557";
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = SmtpUser;


                    smtp.Send(mail);

            }
            finally
            {
                mail.Dispose();
            }
        }
    }
}
