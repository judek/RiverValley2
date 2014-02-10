using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;

namespace RiverValley2
{
    public partial class HomePage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LiteralButton4Title.Text = GetContent("Button4Title");
            LiteralButton4Description.Text = GetContent("Button4Description");

            LiteralButton3Title.Text = GetContent("Button3Title");
            LiteralButton3Description.Text = GetContent("Button3Description");
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

            if (Request.QueryString["preview"] != null)
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
                sContent = ContentReader.GetContent(Server.MapPath(sPrefix + sSuffix)," ", true);

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
        
        
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Calendar.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Multimedia.aspx");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("PrayerCampaign.aspx");
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Response.Redirect("CalendarEvent.aspx?ID=127");
        }
    }
}
