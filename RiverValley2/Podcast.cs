using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Text;


namespace RiverValley2
{
    [Serializable]
    public class rss
    {
        [XmlAttribute]
        public string version = "2.0";

        [XmlElement]
        public PodChannel channel = new PodChannel();
    }

    [Serializable]
    public class PodChannel
    {

        public string title = "Channel Title";
        public string link = "http://rivervalleycommunity.org";
        public string description = "Multimedia content from River Valley Community Church, Aurora IL";
        public string language = "en-us";
        public string copyright = DateTime.Now.Year.ToString();
        public DateTime lastBuildDate = DateTime.Now;
        public string generator = "Jude's cool podcast generator";
        public string webMaster = "admin@jrivervalleycommunity.org";


        public int ttl;
        List<item> _items = new List<item>();
        public List<item> items
        {
            get { return _items; }
            set { _items = value; }
        }


    }

    [Serializable]
    public class item
    {
        public string title = "Title";
        public string description = "Description coming soon";
        public DateTime pubDate = DateTime.Now;
        public List<cat> cats = new List<cat>();
        public ItemEnclosure enclosure = new ItemEnclosure();
    }

    [Serializable]
    public class ItemEnclosure
    {
        [XmlAttribute]
        public string url = "thesong.mp3";

        [XmlAttribute]
        public long length = 0;

        [XmlAttribute]
        public string type = "audio/mpeg";

        //<enclosure url="test.mp3" length="1024" type="audio/mpeg"/>
    }


    [Serializable]
    public class cat
    {
        public cat()
        {

        }
        public cat(string Category)
        {
            category = Category;
        }

        public string category;
    }


    public class Util
    {
        public static string Serialize<T>(T srcObject)
        {
            string srcObjectXml = string.Empty;

            XmlSerializer xmlSerializer = new XmlSerializer(srcObject.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8))
                {
                    xtw.Formatting = Formatting.Indented;

                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");

                    xmlSerializer.Serialize(xtw, srcObject, ns);
                    ms.Seek(0, System.IO.SeekOrigin.Begin);

                    using (StreamReader reader = new StreamReader(ms))
                    {
                        srcObjectXml = reader.ReadToEnd();
                        xtw.Close();
                        reader.Close();
                    }
                }
            }

            srcObjectXml = srcObjectXml.Replace("<items>\r\n", "");
            srcObjectXml = srcObjectXml.Replace("</items>\r\n", "");

            srcObjectXml = srcObjectXml.Replace("<cat>\r\n", "");
            srcObjectXml = srcObjectXml.Replace("</cat>\r\n", "");
            srcObjectXml = srcObjectXml.Replace("<cats>\r\n", "");
            srcObjectXml = srcObjectXml.Replace("</cats>\r\n", "");


            return srcObjectXml;
        }

        public static T Deserialize<T>(string xmlString)
        {
            StringReader sr = new StringReader(xmlString);
            XmlTextReader xtr = new XmlTextReader(sr);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T instance = (T)xmlSerializer.Deserialize(xtr);
            xtr.Close();

            return instance;
        }
    }
}
