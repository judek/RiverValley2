using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace RiverValley2
{
    public class RiverValleyPage2 : RiverValleyPage
    {
        public string GenerateRandomPictures()
        {
            
            DirectoryInfo dinfo = new DirectoryInfo(Server.MapPath(".\\folders\\Random"));


            string sPageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);

            if (false == sPageName.EndsWith(".aspx"))
                throw new ApplicationException("CafePage.GetContent():Unable to parse page name from url path");


            sPageName = sPageName.Replace(".aspx", "");


            FileInfo[] files = dinfo.GetFiles("*" + sPageName + "*.jpg");

            if (files.Length < 3)
                files = dinfo.GetFiles("*.jpg");
                
             if (files.Length < 3)
                 return "";

            System.Security.Cryptography.RNGCryptoServiceProvider rng = 
                new System.Security.Cryptography.RNGCryptoServiceProvider();

            uint urnd;
            byte[] rnd = new byte[4];

            int pict1 = 0, pict2 = 0, pict3 = 0;
            
            rng.GetBytes(rnd);
            urnd = System.BitConverter.ToUInt32(rnd, 0);

            pict1 = ((int)urnd) % (files.Length);
            if (pict1 < 0)
                pict1 = pict1 * (-1);
            
            
            
            rng.GetBytes(rnd);
            urnd = System.BitConverter.ToUInt32(rnd, 0);
            
            pict2 = ((int)urnd) % (files.Length);

          
            if (pict2 < 0)
                pict2 = pict2 * (-1);

           
            
            rng.GetBytes(rnd);
            urnd = System.BitConverter.ToUInt32(rnd, 0);

            pict3 = ((int)urnd) % (files.Length - 1);

            if (pict3 < 0)
                pict3 = pict3 * (-1);


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
       
            //Tried caching images but still too slow.
            if (MasterPageFile.Contains("Mobile"))
            {

                //sb.Append("<a href=\"gallery.aspx?f=gallery99Gallery\"><img width=\"233\" src=\"folders/Random/" + files[pict1].Name + "\" alt=\"River Valley Community Church\" class=\"shadowMe\" /</a>><br />");

                sb.Append("<a href=\"gallery.aspx?f=gallery99Gallery\"><img width=\"233\" src=\"" + CacheifyImage(files[pict1].Name) + "\" alt=\"River Valley Community Church\" class=\"shadowMe\" /></a><br />");
            }
            else
            {
                //sb.Append("<br /><a href=\"gallery.aspx?f=gallery99Gallery\"><img width=\"233\" src=\"folders/Random/" + files[pict1].Name + "\" alt=\"River Valley Community Church\" class=\"shadowMe\" /></a><br />");
                //sb.Append("<br /><a href=\"gallery.aspx?f=gallery99Gallery\"><img width=\"233\" src=\"folders/Random/" + files[pict2].Name + "\" alt=\"River Valley Community Church\" class=\"shadowMe\" /></a><br />");
                //sb.Append("<br /><a href=\"gallery.aspx?f=gallery99Gallery\"><img width=\"233\" src=\"folders/Random/" + files[pict3].Name + "\" alt=\"River Valley Community Church\" class=\"shadowMe\" /></a><br />");

                sb.Append("<br /><a href=\"gallery.aspx?f=gallery99Gallery\"><img width=\"233\" src=\"" + CacheifyImage(files[pict1].Name) + "\" alt=\"River Valley Community Church\" class=\"shadowMe\" /></a><br />");
                sb.Append("<br /><a href=\"gallery.aspx?f=gallery99Gallery\"><img width=\"233\" src=\"" + CacheifyImage(files[pict2].Name) + "\" alt=\"River Valley Community Church\" class=\"shadowMe\" /></a><br />");
                sb.Append("<br /><a href=\"gallery.aspx?f=gallery99Gallery\"><img width=\"233\" src=\"" + CacheifyImage(files[pict3].Name) + "\" alt=\"River Valley Community Church\" class=\"shadowMe\" /></a><br />");

            }

            return sb.ToString();
        }


        string CacheifyImage(string imageName)
        {
            return "ShrinkImage.aspx?i=folders/Random/" + imageName + "&w=233";
        }


        public override string MasterPageFile
        {
            get
            {
                return base.MasterPageFile;
            }
            set
            {

                bool useMobileTemplate = false;

                if (null == Request.UserAgent)
                    return;
                
                if (null == Session["InMobileMode"])
                {
                    //Otherwise we check our selves
                    string userAgent = Request.UserAgent;

                    if (null != Request.UserAgent)
                    {
                        useMobileTemplate = (userAgent.Contains("Android")
                            || userAgent.Contains("iPhone")
                            || userAgent.Contains("webOS")
                            || userAgent.Contains("iPod"));
                    }
                    else
                    {
                        useMobileTemplate = false;
                    }
                }
                else
                {
                    //Use session Override
                    useMobileTemplate = (bool)Session["InMobileMode"];
                }


                if (true == useMobileTemplate)
                    base.MasterPageFile = "RiverValleyMobile.Master";
                else
                    base.MasterPageFile = value;

                
                Session["InMobileMode"] = useMobileTemplate;


            }
        }
       

        protected void DumpCache()
        {
            Cache.Remove("cache.RiverValley.MultimediaFiles");
            Cache.Remove("cache.RiverValley.MultimediaSideBar");
            Cache.Remove("cache.RiverValley.MultimediaTagList");
            Cache.Remove("cache.RiverValley.PodcastXMLString");
        }
    }
}
