<%LANGUAGE="VBScript"%>

<%Session("LoginPage") = "ListAnnouncements.asp"%>

<%If Request.Cookies("UserName") = "" Then%>
<%Response.Redirect("../../../login.aspx?ReturnUrl=%2frivervalley%2fEdit%2fDefault.aspx") %>
<%End If%>




<% 
Set conn = Server.CreateObject("ADODB.Connection") 
conn.open "Provider=Microsoft.Jet.OLEDB.4.0; Data Source="& Server.MapPath("../../../database/RiverValleyCalendar.mdb")  


'SQL = "SELECT * FROM CalendarEvents"

SQL = "SELECT * FROM CalendarEvents WHERE EventDate BETWEEN #Jan-05-2005# AND #DEC-05-2005# ORDER BY ID"



Set RS = conn.execute(SQL) 

%> 
<head>
<meta http-equiv="Content-Language" content="en-us">
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<meta name="GENERATOR" content="Microsoft Notepad">
<title>Calendar Events</title>
</head>

<body> 
<b><h2>stephaniesacademyofdance.com - Calendar Events</b></h2><br></h>
<a href="addEvent.htm"><b>Click here to add new Event</b></a>
<table border="0">
  <tr>
    <td width="15%"><b><h2>Subject</td>
    <td width="5%"><b><h2>Date</td>
  </tr>

<% Do While NOT RS.EOF %> 
  <tr>
    <td width="15%"><a href="editEvent.asp?ID=<%= rs("ID")%>"><%= rs("Subject")%></a><%=rs("Details")%></td>
	<td width="5%"><%= rs("EventDate")%></td>
  </tr>
<% RS.movenext 
Loop %> 
</body>
</html>
<%conn.Close
Set conn = Nothing
Set RS = Nothing
%>





























































