using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Web.Security;


namespace RiverValley2.Edit
{
    public class JWT
    {
        public int expires_in { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string id_token { get; set; }
    }

    public class JsonToken
    {
        public string signature { get; set; }
        public Dictionary<string, string> payload { get; set; }
        public Dictionary<string, string> header { get; set; }
    }

    public partial class Logon : System.Web.UI.Page
    {
        public string sLogonRedirectURL = ConfigurationManager.AppSettings["LogonRedirectURL"];
        public string sLogonClientID = ConfigurationManager.AppSettings["LogonClientID"];
       
        protected void Page_Load(object sender, EventArgs e)
        {

            string secret = "Not a secret";
            string secretPath = Server.MapPath(".") + @"\Secret.txt";

            if (true == File.Exists(secretPath))
                secret = File.ReadAllText(secretPath).Trim();
            else
                Literal1.Text = "Error cannot find file:" + secretPath + "<br />";
            
            
            if (Session["state"] == null)
            {
                Session["state"] = Guid.NewGuid();
            }

            if (Request.QueryString.Count == 0)
                return;


            if (Request.QueryString["ReturnUrl"] != null)
            {
                Session["MyReturnUrl"] = (string)Request.QueryString["ReturnUrl"];
            }

            
            
            string error = Request.QueryString["error"];

            if (false == string.IsNullOrEmpty(error))
            {
                Literal1.Text = "Error:" + error;
                return;
            }

            string state = Request.QueryString["state"];
            if (true == string.IsNullOrEmpty(state))
            {
                return;
            }

            if (state != ((Guid)Session["state"]).ToString())
            {
                Literal1.Text = "Invalid State:" + state;
                return;
            }

            string code = Request.QueryString["code"];
            if (true == string.IsNullOrEmpty(code))
            {
                //Literal1.Text = "Access Code not available";
                return;
            }

            StringBuilder sb = new StringBuilder();
            //At this point we have authirization from user.
            sb.Append("Success!");
            sb.Append("<br />Access Code:" + code);


            var baseAddress = "https://www.googleapis.com/oauth2/v4/token";

            //WebProxy proxyObject = new WebProxy("http://proxy.taltrade.com:8080/");



            using (WebClient client = new WebClient())
            {

                byte[] response =
                client.UploadValues(baseAddress, new NameValueCollection()
       {
           { "code", code }
           ,{ "client_id", sLogonClientID }
           //Client secret has changed
           ,{ "client_secret", secret }
           //To Do: get real client_secret
           ,{ "redirect_uri", sLogonRedirectURL }
           ,{ "grant_type", "authorization_code" }
       });




                string result = System.Text.Encoding.UTF8.GetString(response);
                Literal1.Text = result;
            }
            var serializer = new JavaScriptSerializer();
            var deserializedResult = serializer.Deserialize<JWT>(Literal1.Text);

            var JasonReslut = Decode(deserializedResult.id_token);


            foreach (KeyValuePair<string, string> entry in JasonReslut.payload)
            {
                sb.Append("<br />" + entry.Key + ":" + entry.Value);
            }

            sb.Append("<br /><a href='https://accounts.google.com/logout'>Click here to logout</a>");

            Literal1.Text = sb.ToString();


            //FormsAuthentication.RedirectFromLoginPage(JasonReslut.payload["email"], false);
            FormsAuthentication.SetAuthCookie(JasonReslut.payload["email"], false);
            //if (Request.QueryString["ReturnUrl"] != null)
               //Response.Redirect(Request.QueryString["ReturnUrl"]);

            if (Session["MyReturnUrl"] != null)
                Response.Redirect((string)Session["MyReturnUrl"]);


        }

        public static Encoding TextEncoding = Encoding.UTF8;

        private static char Base64PadCharacter = '=';
        private static char Base64Character62 = '+';
        private static char Base64Character63 = '/';
        private static char Base64UrlCharacter62 = '-';
        private static char Base64UrlCharacter63 = '_';

        private static byte[] DecodeBytes(string arg)
        {
            if (String.IsNullOrEmpty(arg))
            {
                throw new ApplicationException("String to decode cannot be null or empty.");
            }

            StringBuilder s = new StringBuilder(arg);
            s.Replace(Base64UrlCharacter62, Base64Character62);
            s.Replace(Base64UrlCharacter63, Base64Character63);

            int pad = s.Length % 4;
            s.Append(Base64PadCharacter, (pad == 0) ? 0 : 4 - pad);

            return Convert.FromBase64String(s.ToString());
        }

        private static string Base64Decode(string arg)
        {
            return TextEncoding.GetString(DecodeBytes(arg));
        }


        public static JsonToken Decode(string rawToken)
        {
            string[] tokenParts = rawToken.Split('.');

            if (tokenParts.Length != 3)
            {
                throw new ApplicationException("Token must have three parts separated by '.' characters.");
            }

            string encodedHeader = tokenParts[0];
            string encodedPayload = tokenParts[1];
            string signature = tokenParts[2];

            string decodedHeader = Base64Decode(encodedHeader);
            string decodedPayload = Base64Decode(encodedPayload);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            Dictionary<string, string> header = serializer.Deserialize<Dictionary<string, string>>(decodedHeader);
            Dictionary<string, string> payload = serializer.Deserialize<Dictionary<string, string>>(decodedPayload);

            return new JsonToken { header = header, payload = payload, signature = signature };
        }
    }


    
}