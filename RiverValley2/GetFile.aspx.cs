using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace RiverValley2
{
    public partial class GetFile : System.Web.UI.Page
    {
        const int CHUNK_SIZE = 1048576;

        protected void Page_Load(object sender, EventArgs e)
        {
            string name = Request.QueryString["SF"];
            string targetname = Request.QueryString["TF"];

            if (null == name)
                return;

            FileInfo fileInfo = new FileInfo(Server.MapPath(name));


            if (fileInfo.Exists == false)
            {
                Response.Write("<br />File not found:" + Request.QueryString["SF"]);
                return;
            }

            //For security, dont want them to get files from any folder by hacking URL.
            if (File.Exists(fileInfo.Directory + "\\FileStream.token") == false)
            {
                Response.Write("Access Denied:Invalid Token:");
                Response.Write(Request.QueryString["SF"]);
                return;
            }


            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/octet-stream";

            if (null == targetname)
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileInfo.Name);
            else
                Response.AddHeader("Content-Disposition", "attachment;filename=" + targetname);

            Response.AddHeader("Content-Length", fileInfo.Length.ToString());


            //long bytesToGo;
            //int bytesRead;
            //Byte[] buffer = new byte[CHUNK_SIZE];
            //FileStream fs = null;

            try
            {

                Response.WriteFile(fileInfo.FullName);

                //fs = new FileStream(Server.MapPath(name), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                //bytesToGo = fileInfo.Length;
                //while (bytesToGo > 0)
                //{
                //    if (Response.IsClientConnected)
                //    {
                //        bytesRead = fs.Read(buffer, 0, CHUNK_SIZE);
                //        Response.OutputStream.Write(buffer, 0, bytesRead);
                //        Response.Flush();
                //        bytesToGo -= bytesRead;
                //    }
                //    else
                //    {
                //        bytesToGo = -1;
                //    }
                //}

            }
            catch (Exception exp)
            {
                Response.Write("Erro:" + exp.Message);
                return;
            }

            finally
            {
                //if (null != fs)
                //{
                //    fs.Close();
                //    fs.Dispose();
                //}
                //Response.Flush();
            }
        }
    }
}
