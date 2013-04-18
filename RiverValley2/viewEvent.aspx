<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewEvent.aspx.cs" Inherits="RiverValley2.viewEvent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><asp:Literal ID="LiteralPageTitle" runat="server" Text="Calendar"></asp:Literal></title>
    <link rel="StyleSheet" href="style.css" type="text/css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <font face="Arial" size="5" color="#C0C0C0"><b><asp:Label ID="LabelSubject" runat="server" Text="Subject"></asp:Label></b></font><br><br>
    
    <table cellspacing="0" cellpadding="4" width="100%" border="0">
  <tbody>
  
   
   <tr>

      <td vAlign="top" noWrap align="right" width="1%"><font face="Arial" size="2"><b>Date:</b></td>
      <td><font face="Arial" size="2"><font size="2"><asp:Label ID="LabelDate" runat="server" Text="Date"></asp:Label></font></font></td>

    </tr>
   
   
   
   <tr>
      <td valign="top" nowrap align="right" width="1%"><b><font face="Arial" size="2">Time:</b></td>
      <td><font face="Arial" size="2"><asp:Label ID="LabelTimeSpan" runat="server" Text="TimeSpan"></asp:Label></td>
    </tr>
    <tr>
      <td valign="top" nowrap align="right" width="1%"><font face="Arial" size="2"><b>Details:</b></font></td>
      <td>
          <asp:Literal ID="LiteralDetails" runat="server"></asp:Literal>
          <!-- <asp:TextBox ID="TextBoxBody" runat="server" Columns="35" Height="232px" 
              ReadOnly="True" TextMode="MultiLine" Width="290px"></asp:TextBox> -->
        </td>
      
    </tr>
  
  </tbody>
</table>
    
    
    
    </form>
</body>
</html>
