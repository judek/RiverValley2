<%@ Page Title="" Language="C#" MasterPageFile="RiverValley.Master" AutoEventWireup="true" CodeBehind="MultimediaPlay.aspx.cs" Inherits="RiverValley2.MultimediaPlay" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
    <br /><asp:Label ID="LabelSubject" runat="server" Text=""></asp:Label>
    <asp:TextBox ID="TextBoxSubject" runat="server" Width="348px" 
        BorderColor="#00CC00" BorderWidth="1px"></asp:TextBox></span>
<br /><span class="subtitle"><asp:Literal ID="LiteralDate" runat="server"></asp:Literal>
    <asp:TextBox ID="TextBoxDated" runat="server" Width="70px" 
    BorderColor="#00CC00" BorderWidth="1px"></asp:TextBox></span>
<br /><span class="smalltitle">Tags:<asp:Literal ID="LiteralTags" runat="server"></asp:Literal>
<asp:TextBox ID="TextBoxTags" runat="server" Width="348px" 
    BorderColor="#00CC00" BorderWidth="1px"></asp:TextBox></span>
<br /><span class="footer"><asp:Literal ID="LiteralAttachments" runat="server"></asp:Literal>

</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
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
<asp:Literal ID="LiteralDoneButton" runat="server">
<input type="button" value="Done" onclick="history.go(-1)" class="button-order" id="button1" name="button1" />
</asp:Literal>
<asp:Button ID="ButtonSave" runat="server" onclick="ButtonSave_Click"  
    Text="Save" CssClass="button-edit" />

	
<br /><br /><asp:Literal ID="LiteralDescription" runat="server"></asp:Literal>
<asp:TextBox ID="TextBoxDescription" runat="server" TextMode="MultiLine" 
                         Width="718px" Height="260px" BorderColor="#00CC00" 
        BorderWidth="1px"></asp:TextBox>

    <br />
    
    

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PictureColumn" runat="server">
    <b><font color="red"><asp:Literal ID="LiteralMessage" runat="server"></asp:Literal></font></b>
 <br />
<asp:Button ID="ButtonUpload" runat="server" onclick="ButtonUpload_Click" 
                         Text="Upload Attachement" CssClass="button-edit" />
                     <asp:FileUpload ID="FileUpload1" runat="server" 
    BorderColor="#00CC00" BorderWidth="1px" />
 <br /><asp:CheckBox ID="CheckBoxEnableDelete" runat="server" AutoPostBack="True" 
                         oncheckedchanged="CheckBoxEnableDelete_CheckedChanged" Text="Enable Delete" />
                     <asp:Button ID="ButtonDelete" runat="server" Enabled="False" 
                         onclick="ButtonDelete_Click" Text="Delete" BackColor="Red" 
                         Visible="False" />
<asp:Literal ID="LiteralPictures" runat="server"></asp:Literal>

</asp:Content>
