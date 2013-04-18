using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

namespace RiverValley2
{
    public partial class Podcast : System.Web.UI.Page
    {
        const int MAX_ARTICLES = 25;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool blnPutInCache = false;

            string PodcastXMLString = Cache.Get("cache.RiverValley.PodcastXMLString") as string;

            if (null != PodcastXMLString)
            {//All Done we have a valid cache
                SendXmlString(PodcastXMLString);
                return;
            }

            //If That cache is dead chances are that document cache is dead also so force load 
            //of the multimedia page so we know we have at least that cache
            string sThisFileName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            string MultimediaPageURL = (Request.Url.AbsoluteUri).Replace(sThisFileName, "multimedia.aspx");
            string MultimediaPath = MultimediaPageURL.Replace(".aspx", "/");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(MultimediaPageURL);

            try
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch
            {
                //Oh well we will try to continue
            }


            rss podCast = new rss();

            //Time to live (Optional)
            podCast.channel.ttl = 60;
            ///ttl stands for time to live. 
            ///It's a number of minutes that indicates how long a channel can be cached before 
            ///refreshingfrom the source. This makes it possible for RSS sources to be managed by 
            ///a file-sharing network such as Gnutella. 

            podCast.channel.title = "River Valley Community Church Sermons";
            podCast.channel.link = "http://www.rivervalleycommunity.org";
            podCast.channel.copyright = "River Valley Community Church " + DateTime.Now.Year;
            podCast.channel.description = "Sermon recordings form Andy Morgan, Pastor, River Valley Community Church, Aurora IL";
            podCast.channel.generator = "Jude's cool Podcast Generator";
            podCast.channel.lastBuildDate = DateTime.Now;


            List<Document> DocumentList = Cache.Get("cache.RiverValley.MultimediaFiles") as List<Document>;

            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

            if (DocumentList != null)
            {
                item i;
                foreach (MultimediaFile d in DocumentList)
                {
                    i = new item();
                    i.title = d.Title;
                    i.pubDate = d.Dated;
                    i.description = d.Description;
                    i.enclosure.url = MultimediaPath + d.Name;

                    foreach (Tag t in d.Tags)
                    {
                        i.cats.Add(new cat(textInfo.ToTitleCase(t.Name)));
                    }



                    podCast.channel.items.Add(i);

                    if (podCast.channel.items.Count >= MAX_ARTICLES)
                        break;

                    blnPutInCache = true;
                }
            }

            PodcastXMLString = Util.Serialize<rss>(podCast);


            if (true == blnPutInCache)
                Cache.Insert("cache.RiverValley.PodcastXMLString", PodcastXMLString);

            SendXmlString(PodcastXMLString);

        }

        void SendXmlString(string XmlString)
        {
            Response.Clear();
            Response.ContentType = "text/xml; encoding='utf-8'";
            Response.StatusCode = 200;
            Response.Write(XmlString);
            Response.End();
        }
    }
}
