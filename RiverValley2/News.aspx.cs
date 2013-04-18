using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Caching;
using Rss;

namespace RiverValley2
{
    public partial class News : RiverValleyPage2
    {
        string _newsURL = "http://www.christianpost.com/services/rss/feed/most-popular";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //LabelMain.Text = GetContent("Body");
            LiteralPictures.Text = GenerateRandomPictures();

            RssItemCollection feedItems = null;

            
            
            //Try loading news from cache first
            feedItems = Cache.Get("cache.News.RiverValley") as RssItemCollection;

            if (null == feedItems)
            {//If not in cache go to the internet

                try
                {
                    RssFeed rssFeed = RssFeed.Read(_newsURL);
                    RssChannel channel = rssFeed.Channels[0] as RssChannel;

                    if (null == channel)
                    {
                        LabelMain.Text = "Unable to read channel zero from feed:" + _newsURL;
                    }

                    feedItems = channel.Items;
                }
                catch (Exception exp)
                {
                    LabelMain.Text = "<span class=\"smalltitle\">Unable to get news articles<br />" + exp.Message + "<br /></span>" + exp.StackTrace;
                    return;
                }

                if (null == feedItems)
                {
                    LabelMain.Text = "No news items found:" + _newsURL;
                    return;
                }
                if (0 == feedItems.Count)
                {
                    LabelMain.Text = "No news items found:" + _newsURL;
                    return;
                }

                //Now that we got some new news cache it and continue
                Cache.Insert("cache.News.RiverValley", feedItems,
                       null,
                       DateTime.Now.AddHours(1), TimeSpan.Zero);

            }

            StringBuilder sb = new StringBuilder();


            foreach (RssItem item in feedItems)
            {
                sb.Append("<span class=\"footer\">[" + (item.PubDate.ToString()) + "]<br /><a href=\"" + (item.Link) + "\" target=\"_blank\"></span>" + (item.Title) + "</a></span>");
                //sb.Append("<span class=\"subtitle\">" + (dr["title"] as string) + "</span> " + ("<span class=\"smalltext\">" + dr["pubDate"] as string) + "</span> ");
                //sb.Append("<br />" + (dr["description"] as string) + "<a href=\"" + (dr["link"] as string) + "\" target=\"_blank\">" + " <b>More...</a></b>");
                sb.Append("<br />" + (item.Description) + "</b>");
                sb.Append("<br /><br /><br />");

            }
  

            LabelMain.Text += sb.ToString();

        }
    }
}
