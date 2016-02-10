<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RiverValley2.Edit.SendTweet.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../style.css" rel="stylesheet" type="text/css" />

    <title>Send Tweet</title>
</head>
<body style="margin:0;padding:50px;">
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:TextBox ID="TextBox1" runat="server" Height="170px" Rows="5" TextMode="MultiLine" Width="332px"></asp:TextBox>
        <p>
            <asp:Button ID="Button1" runat="server" Text="Send Tweet" />
            <br /><asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </p>
    </form>
</body>
</html>
