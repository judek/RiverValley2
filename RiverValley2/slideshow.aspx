<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="slideshow.aspx.cs" Inherits="RiverValley2.slideshow" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link rel="StyleSheet" href="style.css" type="text/css" media="screen" />
</head>
<body bgcolor="#BB0000" background="picts/background.gif" >
    <form id="form1" runat="server">
    <div>
<table border="0">
	<tr>
		<td width="100" ></td>
		<td>
		    <table width="900">
             <tr>
                 <td>
                 <asp:Button ID="ButtonPrevious" runat="server" onclick="ButtonPrevious_Click" 
                            Text="Previous" UseSubmitBehavior="False" CssClass="button-order" />
                 <asp:Button ID="ButtonNext" runat="server" Text="Next" onclick="ButtonNext_Click" 
                         CssClass="button-order" />                 
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button 
                         ID="ButtonRotateLeft" runat="server" onclick="ButtonRotateLeft_Click" 
                         Text="Rotate Left" CssClass="button-edit" />
&nbsp;<asp:Button ID="ButtonRotateRight" runat="server" onclick="ButtonRotateRight_Click" 
                         Text="Rotate Right" CssClass="button-edit" />
                     <asp:CheckBox ID="CheckBoxEnableDelete" runat="server" AutoPostBack="True" 
                         oncheckedchanged="CheckBoxEnableDelete_CheckedChanged" Text="OK to delete" 
                         ToolTip="Just a saftey for the delete button" />
                     <asp:Button ID="ButtonDelete" runat="server" Enabled="False" 
                         onclick="ButtonDelete_Click" Text="Delete" />
                 </td>
                 <td align="right">
                  <asp:Button ID="ButtonDone" runat="server" Text="Done" onclick="ButtonDone_Click" 
                         CssClass="button-edit"/>
                 </td>
             </tr>
             <tr>
                 <td colspan="2">
                     <asp:Literal ID="LiteralDescription" runat="server" Text="Description"></asp:Literal>
                     <asp:Literal ID="LiteralFullSizeLink" runat="server" Text="Description"></asp:Literal>
                     <asp:TextBox ID="TextBoxDescription" runat="server" TextMode="MultiLine" 
                         Width="588px" Height="90px"></asp:TextBox>
                     <asp:Button ID="ButtonSave" runat="server" onclick="ButtonSave_Click" 
                         Text="Save Description" CssClass="button-edit" />
                 </td>
             </tr>
         </table>
            <hr />
            <asp:Image ID="Image1" runat="server" />
            <br />
		</td>
	</tr>
</table>

           
       
       
       
    </div>
    </form>
</body>
</html>
