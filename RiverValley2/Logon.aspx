<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="RiverValley2.Edit.Logon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

   <link href="style.css" rel="stylesheet" type="text/css" />

    <title>River Valley Web Site Editor</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <%
            string link = "https://accounts.google.com/o/oauth2/auth?client_id="
                + "397605678479-9cnlh0uo8mlo1qo3f756ims11jqe4pqm.apps.googleusercontent.com" 
                + "&state=" 
                + Session["state"] 
                + "&redirect_uri="
                + "http://localhost:49256/Logon.aspx";


            string link2 = "https://accounts.google.com/o/oauth2/v2/auth?"
                                + "client_id=397605678479-9cnlh0uo8mlo1qo3f756ims11jqe4pqm.apps.googleusercontent.com"
                                + "&response_type=code"
                                + "&scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.email+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.profile"
                                + "&redirect_uri=http://localhost:49256/Logon.aspx"
                                + "&state=" + Session["state"]
                                //+ "&login_hint=jsmith@example.com"
                                //+ "&openid.realm=example.com"
                                //+ "&hd=example.com"
                                ;

            
             %>
         
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>

        <%
            if(System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                %>
                    <br />User <%=HttpContext.Current.User.Identity.Name%> not allowed access to this resource. 
                    <br /><a href='https://accounts.google.com/logout'>Please, click here to logout</a>            
                    <br />And then log into an another Google account.
                            <%
            }
            else
            {
               %> <a href="<%=link2%>">Click here to login through Google</a> <%
            }
            
             %>

    </div>
    </form>
</body>
</html>
