using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Web.UI;


namespace RiverValley2
{
    public enum MultimediaType { unknown, audio, video };


    public class AudioFile : MultimediaFile
    {
        public AudioFile(Page parentPage, string name)
            : base(parentPage, name)
        {

        }

        public override MultimediaType MultimediaType
        {
            get { return MultimediaType.audio; }
        }
    }
    public class VideoFile : MultimediaFile
    {
        public VideoFile(Page parentPage, string name)
            : base(parentPage, name)
        {

        }

        public override MultimediaType MultimediaType
        {
            get { return MultimediaType.video; }
        }
    }

    public class MultimediaFile : Document
    {
        public MultimediaFile(Page parentPage, string name)
            : base(parentPage, name)
        {

        }

        public virtual MultimediaType MultimediaType
        {
            get { return MultimediaType.unknown; }
        }

    }

    public class Document
    {

        public Document(Page parentPage, string name)
        {
            _parentPage = parentPage;
            _Name = name;
        }

        Page _parentPage;
        public Page ParentPage
        {
            get { return _parentPage; }
        }

        public const string MULTIMEDIA_FOLDER = "multimedia";
        public const string ATTACHMENT_FOLDER = "multimedia/attachments";

        string _Name;
        public string Name
        {
            get { return _Name; }
        }

        public string Description
        {
            get
            {
                return GetContent("Description", "");
            }
            set
            {
                SetContent("Description", value);
            }
        }

        public string HTMLDescription
        {
            get
            {
                return GetContent("Description", "").Replace("\r\n", "<br />"); ;
            }
            set
            {
                SetContent("Description", value);
            }
        }


        string GetContent(string sContentName, string sDefault)
        {
            string content = "";
            string spath = _parentPage.Server.MapPath(MULTIMEDIA_FOLDER + "\\" + _Name + "." + sContentName + ".txt");

            try
            {
                content = File.ReadAllText(spath);
            }
            catch { }

            if (content.Length < 1)
            {
                content = sDefault;
                SetContent(sContentName, content);
            }
            return content;
        }

        void SetContent(string sContentName, string sContent)
        {
            string spath = _parentPage.Server.MapPath(MULTIMEDIA_FOLDER + "\\" + _Name + "." + sContentName + ".txt");
            File.WriteAllText(spath, sContent);
        }

        public bool Exists
        {
            get
            {
                string spath = _parentPage.Server.MapPath(MULTIMEDIA_FOLDER + "\\" + _Name);
                FileInfo fInfo = new FileInfo(spath);
                return fInfo.Exists;
            }
        }

        public void Delete()
        {
            string spath = _parentPage.Server.MapPath(MULTIMEDIA_FOLDER + "\\" + _Name);
            FileInfo fInfo = new FileInfo(spath);

            if (fInfo.Exists)
                fInfo.Delete();


            spath = _parentPage.Server.MapPath(MULTIMEDIA_FOLDER + "\\" + _Name + "." + "Description" + ".txt");
            fInfo = new FileInfo(spath);

            if (fInfo.Exists)
                fInfo.Delete();


            spath = _parentPage.Server.MapPath(MULTIMEDIA_FOLDER + "\\" + _Name + "." + "Title" + ".txt");
            fInfo = new FileInfo(spath);

            if (fInfo.Exists)
                fInfo.Delete();


            spath = _parentPage.Server.MapPath(MULTIMEDIA_FOLDER + "\\" + _Name + "." + "Tags" + ".txt");
            fInfo = new FileInfo(spath);

            if (fInfo.Exists)
                fInfo.Delete();

            foreach (Attachement a in this.Attachments)
            {
                a.AttachmentInfo.Delete();
            }

        }

        public string Title
        {
            get
            {
                return GetContent("Title", _Name);
            }
            set
            {
                SetContent("Title", value);
            }
        }

        string _Link;
        public string Link
        {
            get { return _Link; }
            set { _Link = value; }
        }


        public DateTime Dated
        {
            get
            {
                string spath = _parentPage.Server.MapPath(MULTIMEDIA_FOLDER + "\\" + _Name);
                FileInfo fInfo = new FileInfo(spath);
                return DateTime.Parse(GetContent("Dated", fInfo.LastWriteTime.ToString()));
                //string spath = _parentPage.Server.MapPath(MULTIMEDIA_FOLDER + "\\" + _Name);
                //FileInfo fInfo = new FileInfo(spath);
                //return fInfo.LastWriteTime; 
            }
            set
            {

                SetContent("Dated", value.ToString());
                //string spath = _parentPage.Server.MapPath(MULTIMEDIA_FOLDER + "\\" + _Name);
                //FileInfo fInfo = new FileInfo(spath);
                //fInfo.LastWriteTime = value; 
            }
        }




        public TagList Tags
        {
            get
            {
                TagList TagList = null;
                try
                {
                    TagList = new TagList(GetContent("Tags", ""));

                }
                catch
                {
                    SetContent("Tags", "");
                    TagList = new TagList();
                }

                return TagList;

            }

            set
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder("");

                foreach (Tag t in value)
                {
                    if (t.Name.Length > 0)
                        sb.Append(t.Name + ";");
                }

                SetContent("Tags", sb.ToString());
            }

        }

        public List<Attachement> Attachments
        {
            get
            {
                DirectoryInfo directoryInfo =
                    new DirectoryInfo(_parentPage.Server.MapPath(Document.ATTACHMENT_FOLDER));

                string filnamePrefix = (_Name + "." + "att.");

                List<Attachement> attlist = new List<Attachement>();
                foreach (FileInfo f in directoryInfo.GetFiles(filnamePrefix + "*"))
                {
                    attlist.Add(new Attachement(f, f.Name.Replace(filnamePrefix, "")));
                }

                return attlist;


            }
        }


    }

    public class Attachement
    {
        public FileInfo AttachmentInfo;
        public string Title;

        public Attachement(FileInfo file, string title)
        {
            AttachmentInfo = file;
            Title = title;
        }
        public override string ToString()
        {
            return Title;
        }
    }


    public class Tag
    {
        string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        string _Signature;
        public string Signature
        {
            get { return _Signature; }
        }

        public Tag(string name)
        {
            _Name = name;

            _Signature =
                System.Text.RegularExpressions.Regex.Replace(name,
                "[^A-Za-z0-9.]", "").ToLower();
        }



    }

    public class ListedTag : Tag
    {
        int _ReferenceCount;
        public int ReferenceCount
        {
            get { return _ReferenceCount; }
            set { _ReferenceCount = value; }
        }
        public ListedTag(string name)
            : base(name)
        {
            _ReferenceCount = 1;
        }
    }

    public class TagList : List<ListedTag>
    {
        public void Add(Tag tag)
        {
            ListedTag ltag = GetTagBySignature(tag.Signature);

            if (null == ltag)
            {
                base.Add(new ListedTag(tag.Name));
            }
            else
                ltag.ReferenceCount++;

        }

        public TagList(string sTags)
        {
            string[] sTagList = sTags.Split(';');
            foreach (string tag in sTagList)
            {
                if (tag.Trim().Length > 0)
                {
                    Tag t = new Tag(tag.Trim());
                    this.Add(t);
                }
            }
        }

        public TagList()
        {

        }
        public bool HasTag(string signature)
        {
            if (null != base.Find(delegate(ListedTag t) { return t.Signature == signature; }))
                return true;

            return false;
        }

        public ListedTag GetTagBySignature(string signature)
        {
            return base.Find(delegate(ListedTag t) { return t.Signature == signature; });
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            foreach (Tag t in this)
            {
                if (t.Name.Length > 0)
                    sb.Append(t.Name + ";");
            }
            string temp = sb.ToString();

            if (temp.Length < 1)
                return temp;

            return temp.Remove(temp.Length - 1, 1); ;
        }
    }


}
