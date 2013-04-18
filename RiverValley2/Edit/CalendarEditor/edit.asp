<%LANGUAGE="VBScript"%>
<%
Dim ColID
ColID = CInt(Request.QueryString("ColID"))

Dim NextPage
Dim NextQuery
 NextQuery= "?ID=" & Request.QueryString("ID")& "&ColID=" & ColID

If ColID < 1 Then
	NextPage = "editEvent.asp"  
	Response.Redirect(NextPage & NextQuery)
End If

%>
<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<meta name="GENERATOR" content="Microsoft FrontPage 4.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<title>Choose Edit Mode</title>
</head>

<body>

<font size="4" face="Arial">This event is a part of a collection of repeating events<br><br>
</font>
<table border="0" cellpadding="0" cellspacing="0" width="370">
  <tr>
    <td width="43"></td>
    <td width="323"><font face="Arial"><i>Choose an edit mode to continue.</i><br>
      </font>
      <form method="POST" action="editEvents.asp<%=NextQuery%>" name=form1>
        <font face="Arial"><input type="submit" value="Edit" name="Button">&nbsp;the
        entire collection <br>(all events in the collection at once).<br>
        </font>
      </form>
      <form method="POST" action="editEvent.asp<%=NextQuery%>" name=form2>
        <font face="Arial"><input type="submit" value="Edit" name="Button">&nbsp;just
        this instance.</font>
      </form>
  </tr>
</table>
<form method="POST" action="Calendar.asp" name=form3>
 <font face="Arial"><input type="submit" value="Cancel" name="Button">
 </form>

</body>

</html>

