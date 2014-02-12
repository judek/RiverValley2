using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;

namespace RiverValley2
{
    public class ContentReader
    {
        public static string GetContent(string sFilename)
        {
            return GetContent(sFilename, " ");
        }

        public static string GetContent(string sFilename, string sDefaultContent)
        {
            return GetContent(sFilename, sDefaultContent, false);
        }

        public static string GetContent(string sFilename, string sDefaultContent, bool blnRaw)
        {
            StreamReader sr = null;
            try
            {
                try
                {
                    sr = new StreamReader(sFilename);
                }
                catch (FileNotFoundException)
                {

                    StreamWriter sw = new StreamWriter(sFilename);
                    sw.Write(sDefaultContent);
                    sw.Close();
                    return sDefaultContent;
                }

                if (true == blnRaw)
                    return sr.ReadToEnd();
                else
                    return FormatStream(sr);

            }
            finally
            {
                if (sr != null) { sr.Close(); sr.Dispose(); }
            }
        }

        public static void SetContent(string sFilename, string sNewContent)
        {
            StreamWriter sw = new StreamWriter(sFilename);
            sw.Write(sNewContent);
            sw.Close();
        }
        
        public static string FormatTextBlock(string sInput)
        {

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(sInput);

            MemoryStream stream = null;
            StreamReader reader = null;

            try
            {
                stream = new MemoryStream(byteArray);
                reader = new StreamReader(stream);
                return FormatStream(reader);
                
            }
            finally
            {
                if (reader != null) { reader.Close(); reader.Dispose(); }
                if (stream != null) { stream.Close(); stream.Dispose(); }
            }


        }

        static string FormatStream(StreamReader reader)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                if ((line.TrimStart().StartsWith("<ul>")) || (line.TrimStart().StartsWith("<li>")))
                {
                    sb.Append(line);
                    continue;
                }

                int leftArrowCount = 0;
                int rightArrowCount = 0;
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case '<': leftArrowCount++; break;
                        case '>': rightArrowCount++; break;
                        default: break;
                    }
                }

                if (leftArrowCount > rightArrowCount)
                    sb.Append((formatLine(line)));//We don't want to put break within a html tag.
                else
                    sb.Append((formatLine(line) + "<br />"));
            }

            return sb.ToString();
        }
        
        static string formatLine(string sline)
        {
            //This works but not yet on a double number subscript
            //if (true == Regex.IsMatch(sline, @"[0-7]+[A-Z]"))
            //{
            //    sline = Regex.Replace(Regex.Replace(sline, @"[0-7]+[A-Z]", "<sup>$&"), @"<sup>[0-7]", "$&</sup>");
            //}

            if (sline.StartsWith("--++ "))
            {
                return "<span class=\"title\">" + sline.Substring(5) + "</span>";
            }
            else if (sline.StartsWith("--+++ "))
            {
                return "<span class=\"subtitle\">" + sline.Substring(6) + "</span>";
            }
            else if (sline.StartsWith("--++++ "))
            {
                return "<span class=\"smalltitle\">" + sline.Substring(7) + "</span>";
            }
            else if (sline.StartsWith("--+++++ "))
            {
                return "<span class=\"phonetitle\">" + sline.Substring(8) + "</span>";
            }
            else if (sline.StartsWith("--++++++ "))
            {
                return "<span class=\"eventTime\">" + sline.Substring(9) + "</span>";
            }

            else
                return sline;


        }
    }
}
