<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Play.aspx.cs" Inherits="RiverValley2.PayAudio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link rel="StyleSheet" href="style.css" type="text/css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table border="0">
	<tr>
		<td cellpadding="4"></td>
		<td>
		    
            <hr />
            <!-- START OF THE PLAYER EMBEDDING TO COPY-PASTE -->


	<object id="player" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" name="player" width="<%=Request.QueryString["W"]%>" height="<%=Request.QueryString["H"]%>">
		<param name="movie" value="player.swf" />
		<param name="allowfullscreen" value="true" />
		<param name="flashvars" value="file=<%=Request.QueryString["FL"]%>/<%=Request.QueryString["F"]%>&image=HeyJude.mp3.jpg&bufferlength=10&title=My%20Video&plugins=<%=Request.QueryString["plugins"]%>" />
		<object type="application/x-shockwave-flash" data="player.swf" width="<%=Request.QueryString["W"]%>" height="<%=Request.QueryString["H"]%>">
			<param name="movie" value="player.swf" />
			<param name="allowfullscreen" value="true" />
			<param name="allowscriptaccess" value="always" />
			<param name="flashvars" value="file=<%=Request.QueryString["FL"]%>/<%=Request.QueryString["F"]%>&image=HeyJude.mp3.jpg&bufferlength=10&title=My%20Video&plugins=<%=Request.QueryString["plugins"]%>" />
			<p><a href="http://get.adobe.com/flashplayer">Get Flash</a> to see this player.</p>
		</object>
	</object>


	<!-- END OF THE PLAYER EMBEDDING -->
            <br />                  
                <table width="400">
             <tr>
                 <td colspan="2">
                     <span class="subtitle"><asp:Literal ID="LiteralSubject" runat="server" Text="Subject..."></asp:Literal></span><br />
                     <asp:Literal ID="LiteralSub" runat="server" Text="Subj:<br />" /><asp:TextBox ID="TextBoxSubject" runat="server" Width="348px"></asp:TextBox><br />
                     
                     <asp:Literal ID="LiteralTags" runat="server" Text="<br />Tags(Enter each tag separated by semi-colpn (;)<br />"></asp:Literal>
                     <asp:TextBox ID="TextBoxTags" runat="server" Width="348px"></asp:TextBox><br />
                     <asp:Literal ID="LiteralDescription" runat="server" Text="Description Coming Soon..."></asp:Literal>
                     <br />
                     <asp:TextBox ID="TextBoxDescription" runat="server" TextMode="MultiLine" 
                         Width="350px" Height="90px"></asp:TextBox>
                         <br /><asp:Label ID="LabelDated" runat="server" Text=""></asp:Label><br />
                     <asp:TextBox ID="TextBoxDated" runat="server"></asp:TextBox>
                     <asp:Button ID="ButtonSave" runat="server" onclick="ButtonSave_Click"  Text="Save" />
                      <asp:CheckBox ID="CheckBoxEnableDelete" runat="server" AutoPostBack="True" 
                         oncheckedchanged="CheckBoxEnableDelete_CheckedChanged" Text="Enable Delete" />
                     <asp:Button ID="ButtonDelete" runat="server" Enabled="False" 
                         onclick="ButtonDelete_Click" Text="Delete" BackColor="Red" 
                         Visible="False" />
                     <asp:Literal ID="LiteralAttachements" runat="server"></asp:Literal>
                     <br /><asp:Button ID="ButtonUpload" runat="server" onclick="ButtonUpload_Click" 
                         Text="Upload Attachement" />
                     <asp:FileUpload ID="FileUpload1" runat="server" />
                     <br />
                     <asp:Literal ID="LiteralDownloadLink" runat="server" Text="Description"></asp:Literal>
                     <br />
                     <b><font color="red"><asp:Literal ID="LiteralMessage" runat="server"></asp:Literal></font></b>
                     <br />
                     
                 </td>
             </tr>
         </table>
		</td>
		
	</tr>
</table>
    </div>
    </form>
</body>
</html>
