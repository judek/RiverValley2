using System;
using System.IO;
using System.Collections.Generic;
using System.Web;



namespace RiverValley2
{
    public class GalleryItem
    {
        public GalleryItem(FileSystemInfo fileinfo, string virtualDirectory, string parentURLPath)
        {
            _fileSystemInfo = fileinfo;

            if (virtualDirectory != ".")
                _virtualDirectory = virtualDirectory;
            else
                _virtualDirectory = "";

            _parentURLPath = parentURLPath;

        }
        public string Name { get { return _fileSystemInfo.Name; } }
        protected FileSystemInfo _fileSystemInfo;

        public string URL
        {
            get
            {
                if (_virtualDirectory.Length > 0)
                    return _virtualDirectory + "/" + _fileSystemInfo.Name;
                else
                    return _fileSystemInfo.Name;
            }
        }

        public string VirtualDirectory
        {
            get { return _virtualDirectory; }

        }
        public string ParentURLPath
        {
            get { return _parentURLPath; }

        }



        string _virtualDirectory = "";
        string _parentURLPath = "";
    }

    public class GalleryPicture : GalleryItem
    {
        public GalleryPicture(FileInfo fileinfo, string virtualDirectory, string parentURLPat) : base(fileinfo, virtualDirectory, parentURLPat) { }
    }

    public class GalleryFolder : GalleryItem
    {
        public GalleryFolder(DirectoryInfo directoryinfo, string virtualDirectory, string parentURLPat) : base(directoryinfo, virtualDirectory, parentURLPat) { }

        public string GalleryName
        {
            get { return _fileSystemInfo.Name.Substring(9, (_fileSystemInfo.Name.Length - 9)); }
        }
    }
}
