using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace RiverValley2
{
    public partial class Multimedia : RiverValleyPage2
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //LabelMain.Text = GetContent("Body");

            LabelRightSideBarArea.Text = "<span class=\"title\">Archives</span><br />";


            TagList pageTagList = null;
            List<Document> SideBarList = null;
            List<Document> DocumentList = null;


            if (null == Session["EditRiverValleyMedia"])
            {
                DocumentList = Cache.Get("cache.RiverValley.MultimediaFiles") as List<Document>;
                SideBarList = Cache.Get("cache.RiverValley.MultimediaSideBar") as List<Document>;
                pageTagList = Cache.Get("cache.RiverValley.MultimediaTagList") as TagList;
            }


            if ((null == DocumentList) ||
                (null == SideBarList) ||
                (null == pageTagList))
            {//Means that there is no cache read everything from disk


                #region ReadLoop

                DocumentList = new List<Document>();
                SideBarList = new List<Document>();
                pageTagList = new TagList();


                DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath(Document.MULTIMEDIA_FOLDER));

                List<FileInfo> multimediaFileList = new List<FileInfo>();

                multimediaFileList.AddRange(directoryInfo.GetFiles("*.mp3"));
                multimediaFileList.AddRange(directoryInfo.GetFiles("*.wma"));
                multimediaFileList.AddRange(directoryInfo.GetFiles("*.aac"));
                multimediaFileList.AddRange(directoryInfo.GetFiles("*.ac3"));
                multimediaFileList.AddRange(directoryInfo.GetFiles("*.mp4"));
                multimediaFileList.AddRange(directoryInfo.GetFiles("*.wmv"));
                multimediaFileList.AddRange(directoryInfo.GetFiles("*.mpg"));

                Document doc;
                System.Collections.Hashtable ht2 = new System.Collections.Hashtable();


                foreach (FileInfo file in multimediaFileList)
                {
                    try
                    {
                        if ((file.Extension.ToLower() == ".mp4") || (file.Extension.ToLower() == ".wmv") || (file.Extension.ToLower() == ".mpg"))
                            doc = new VideoFile(this, file.Name);
                        else
                            doc = new AudioFile(this, file.Name);


                        foreach (Tag t in doc.Tags)
                        {
                            pageTagList.Add(t);
                        }

                        //doc.Link = Document.MULTIMEDIA_FOLDER + "/" + doc.Name;
                        doc.Link = doc.Name;

                    }
                    catch { continue; }


                    DocumentList.Add(doc);



                    #region Fill RightSideBar




                    if (null == SideBarList.Find(delegate(Document st) { return ((st.Dated.Year == doc.Dated.Year) && (st.Dated.Month == doc.Dated.Month)); }))
                    {
                        SideBarList.Add(doc);
                    }


                    #endregion
                }
                #endregion

                #region Sort
                //Sort by date - sort should always be done after all filters for performance
                DocumentList.Sort(delegate(Document f1, Document f2)
                {
                    return DateTime.Compare(f2.Dated, f1.Dated);
                });


                //Do the same for right side bar
                SideBarList.Sort(delegate(Document f1, Document f2)
                {
                    return DateTime.Compare(f2.Dated, f1.Dated);
                });

                //And left
                pageTagList.Sort(delegate(ListedTag t1, ListedTag t2)
                {
                    return t1.Name.CompareTo(t2.Name);
                });

                #endregion

                #region Insert Cache
                //Fill the cache with newly read info.

                Cache.Insert("cache.RiverValley.MultimediaFiles", DocumentList);
                Cache.Insert("cache.RiverValley.MultimediaSideBar", SideBarList);
                Cache.Insert("cache.RiverValley.MultimediaTagList", pageTagList);

                #endregion
            }


            #region Filter
            try
            {
                //Try and do all filters here at once for performance
                if (Request.QueryString["perma"] != null)
                {
                    string permaFilter = Request.QueryString["perma"];
                    DocumentList = DocumentList.FindAll(delegate(Document d)
                    {
                        return ((d.Name.ToLower() == permaFilter.ToLower()));
                    });
                }


                else if (Request.QueryString["MonthFilter"] != null)
                {
                    string MonthFilter = Request.QueryString["MonthFilter"];
                    if (MonthFilter.Length == 6)
                    {
                        int nYearFilter = Int32.Parse(MonthFilter.Substring(0, 4));
                        int nMonthFilter = Int32.Parse(MonthFilter.Substring(4, 2));

                        DocumentList = DocumentList.FindAll(delegate(Document d)
                        {
                            return ((d.Dated.Month == nMonthFilter) && (d.Dated.Year == nYearFilter));
                        });
                    }


                }
                else if (Request.QueryString["Tags"] != null)
                {
                    string sTag = Request.QueryString["Tags"];

                    DocumentList = DocumentList.FindAll(delegate(Document d)
                    {
                        return d.Tags.HasTag(sTag);
                    });


                }
                else if (Request.QueryString["YearFilter"] != null)
                {
                    int YearView;
                    if (true == Int32.TryParse(Request.QueryString["YearFilter"], out YearView))
                    {
                        DocumentList = DocumentList.FindAll(delegate(Document d)
                        {
                            return d.Dated.Year == YearView;
                        });

                    }
                }
                else
                { }//no filter


            }
            catch { }
            #endregion





            #region Body Write Loop
            //Show just 25 entries maximum
            for (int i = 0; ((i < 25) && (i < DocumentList.Count)); i++)
            {
                MultimediaFile d = DocumentList[i] as MultimediaFile;

                if (null == d) continue;


                LabelMultiMediaFiles.Text += "<br><span class=\"subtitle\" >";

                string slink;

                if (d.MultimediaType == MultimediaType.audio)
                {
                    //slink = "<br /><a href=MultimediaPlay.aspx?FL=" + Document.MULTIMEDIA_FOLDER + "&F=" + d.Link + "&T=" + d.MultimediaType + "&W=380&H=50" + "&plugins=spectrumvisualizer-1" + " onclick=\"window.open(this.href,'newWindow','width=400,height=400', 'modal');return false\">";
                    slink = "<br /><a href=MultimediaPlay.aspx?FL=" + Document.MULTIMEDIA_FOLDER + "&F=" + d.Link + "&T=" + d.MultimediaType + "&W=380&H=50" + "&plugins=spectrumvisualizer-1" + ">";
                }
                else if (d.MultimediaType == MultimediaType.video)
                {
                    slink = "<br /><a href=MultimediaPlay.aspx?FL=" + Document.MULTIMEDIA_FOLDER + "&F=" + d.Link + "&T=" + d.MultimediaType + "&W=640&H=388" + ">";
                }
                else
                {
                    slink = "";
                }



                slink += d.Title + "</a></span>";

                LabelMultiMediaFiles.Text += slink;
                LabelMultiMediaFiles.Text += " <br /><br /><span class=\"footer\" >[" + d.Dated.ToLongDateString() + "] <a href=\"Multimedia.aspx?perma=" + d.Name + "\">PermaLink</a></span>";
                

                if (d.Attachments.Count > 0)
                    LabelMultiMediaFiles.Text += "<br /><span class=\"footer\" >Attachements:</span>";


                foreach (Attachement att in d.Attachments)
                {
                    LabelMultiMediaFiles.Text += "&nbsp;<span class=\"footer\" >(<a href=GetFile.aspx?SF=" + Document.ATTACHMENT_FOLDER + "/" + att.AttachmentInfo.Name + "&TF=" + att.Title + ">" + att.Title + "</a>)</span>";
                }
                //LabelMultiMediaFiles.Text += "<br />" + d.HTMLDescription + "";

                string sDescription = ContentReader.FormatTextBlock(d.Description);
                if (sDescription.Length > 500)
                    sDescription = sDescription.Substring(0, 500) + "...";


                LabelMultiMediaFiles.Text += "<br />" + sDescription;

            }
            #endregion

            #region SideBar Write Loop

            int yearView = DateTime.Now.Year;

            if (Request.QueryString["YearFilter"] != null)
            {
                if (false == Int32.TryParse(Request.QueryString["YearFilter"], out yearView))
                {
                    yearView = DateTime.Now.Year;
                }
            }
            
            
            

            List<int> yearsPrinted = new List<int>();


            foreach (Document SideBarDoc in SideBarList)
            {
               
                if (SideBarDoc.Dated.Year == yearView)
                {
                    string sYearMonth = SideBarDoc.Dated.ToString("yyyy") + SideBarDoc.Dated.ToString("MM");

                    LabelRightSideBarArea.Text += string.Format("<span class=\"subtitle\"><a href=\"{2}\">{0}-{1}</a></span><br />",
                         SideBarDoc.Dated.ToString("MMMM"), SideBarDoc.Dated.ToString("yyyy"),
                         "Multimedia.aspx?MonthFilter=" + sYearMonth);
                }
                else
                {

                    int j = yearsPrinted.Find(delegate(int i) { return i == SideBarDoc.Dated.Year; });

                    if (j != SideBarDoc.Dated.Year)
                    {
                        yearsPrinted.Add(SideBarDoc.Dated.Year);
                        LabelRightSideBarArea.Text += string.Format("<span class=\"title\"><a href=\"Multimedia.aspx?YearFilter={0}\">{0}</a></span><br />",
                             SideBarDoc.Dated.Year);
                    }


                    


                }
            }


            #endregion


            string sThisFileName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            string PodcastPageURL = (Request.Url.AbsoluteUri).Replace(sThisFileName, "Podcast.aspx");
            //Remove any query strings
            int nQueryBegin = PodcastPageURL.IndexOf('?');
            if (nQueryBegin > 0)
                PodcastPageURL = PodcastPageURL.Substring(0, nQueryBegin);

            //LabelTagCloud.Text = "Categories <i>(Click any link below)</i><br />";
            //LabelTagCloud.Text += "<br /><a href=\"" + PodcastPageURL + "\" target=_blank><img border=0 src=picts/feed-icon-14x14.png alt=\"Sermon rss feed\" /> Podcast</a><br />";
            LabelTagCloud.Text += "<br /><a href=\"" + "Podcast.aspx" + "\" target=_blank><img border=0 src=picts/feed-icon-14x14.png alt=\"Sermon rss feed\" /> Podcast</a><br />";
            foreach (Tag tag in pageTagList)
            {
                LabelTagCloud.Text += string.Format(" | <a href=\"Multimedia.aspx?Tags={0}\">{1}</a> ",
                    tag.Signature, tag.Name);
            }

        }
    }
}
