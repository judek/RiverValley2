<%LANGUAGE="VBScript"%>

<%Session("LoginPage") = "editEvent.asp"%>

<%If Request.Cookies("UserName") = "" Then%>
<%Response.Redirect("../../../login.aspx?ReturnUrl=%2frivervalley%2fEdit%2fDefault.aspx") %>
<%End If%>



<%


Function HandleASPErrors()
	If Err.number <> 0 Then
		Response.Write "<b><font color=#FF0000>"
		Response.Write Err.Description & "</font></b><br>Unable to complete operation<br>Hit the back button and try again"
		Response.End
	End If
End Function

If Request.Form("Button") = "Cancel" Then
	Response.Redirect("Calendar.asp?Show=Yes&CurMonth=" &Request.Form("StartMon") & "&CurYear=" & Request.Form("StartYear"))
End If




Dim IsDebug
IsDebug = false





If Request.QueryString("ID") = "" Then
		Response.Write "Unable to edit empty record." 
		Response.End
End If


Set conn = Server.CreateObject("ADODB.Connection") 
conn.open "Provider=Microsoft.Jet.OLEDB.4.0; Data Source="& Server.MapPath("../../../database/RiverValleyCalendar.mdb")  

SQL = "SELECT * FROM CalendarEvents WHERE ID = " + Request.QueryString("ID")

Set RS = conn.execute(SQL) 

If RS.EOF Then
		%><p>Document not found. Please Try again.</p><%
		conn.Close
		Set conn = Nothing
		Set RS = Nothing
		Response.End
End if
%>
<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<meta name="GENERATOR" content="Microsoft Notepad">
<title>Updating Event...</title>
</head>

<body>
<%
If request.form("Button") = "DELETE" Then
	%><br>Deleting...<br>SQL:<%
	'Set RS = Nothing 
	SQL = "SELECT * FROM CalendarEvents WHERE ID = " + Request.QueryString("ID")
	Set RS = conn.execute(SQL)
	DoneUrl = "Calendar.asp?Show=Yes&CurMonth=" & MonthName(Month(RS("EventDate"))) & "&CurYear=" & Year(RS("EventDate"))
	SQL = "DELETE FROM CalendarEvents WHERE ID = " + Request.QueryString("ID")
	conn.execute(SQL)
	conn.Close
	Set conn=Nothing
	Set RS=Nothing
	Response.Write SQL
	Response.Write DoneUrl	
	Response.Redirect(DoneUrl)
	Response.End
End If



If Request.Form("Button") = "Save Changes" Then


	Dim strStartDate
'	Dim strEndDate
	Dim strStartTime
	Dim DoneUrl

	strStartDate = Request.Form("StartMon") & "-"  & Request.Form("StartDay") & "-" & Request.Form("StartYear")
'	strEndDate = Request.Form("EndMon") & "-"  & Request.Form("EndDay") & "-" & Request.Form("EndYear")		
	strStartTime = Request.Form("StartHourMerid") & ":" & Request.Form("StartMin") & ":00"


	If true = IsDebug Then
		%>
		Adding Event...
		<br><b>Subject:</b><%=Request.Form("Subject")%>
		<br><b>strStartDate:</b><%=strStartDate%>
			
		<br><b>alldayevent:</b><%=Request.Form("alldayevent")%>
	
		<br><b>strStartTime:</b><%=strStartTime%>
		<br><b>DurHour:</b><%=Request.Form("DurHour")%>
		<br><b>DurMin:</b><%=Request.Form("DurMin")%>

		<br><b>Details:</b><%=Request.Form("Details")%>	
		<br><b>Repeat:</b><%=Request.Form("Repeat")%>
		<br><b>RepeatFreq1:</b><%=Request.Form("RepeatFreq1")%>
		<br><b>RepeatFreq2:</b><%=Request.Form("RepeatFreq2")%>

		<br><b>strEndDate:</b><%=strEndDate%>
		<br>
	<%
	End If


	If "" = Request.Form("Subject") Then
		sErrors = sErrors + "<br>Subject cannot be blank."
	End If


	If False = IsDate(strStartDate) Then
		sErrors = sErrors + "<br>Invalid Start Date:" & strStartDate
	End If



	If Request.Form("Repeat") = 1 Then
		If False = IsDate(strEndDate) Then
			sErrors = sErrors + "<br>Invalid End Date for reapeating event:" & strEndDate
		End If

		If "0" = Request.Form("RepeatFreq1") OR "0" = Request.Form("RepeatFreq2")Then
			sErrors = sErrors + "<br>Missing frequency parameters for repeating event:" & strEndDate
		End If

	End If

	
	
	If Not sErrors = "" Then
		Response.Write "<b><font color=#FF0000>"
		Response.Write sErrors & "</font></b><br>Unable to add event<br>Hit the back button and try again"
		Response.Write "</body></html>"
		Response.End
	End If

	DefaultDate = CDate(strStartDate)


	'BEGIN Update EVENT

	
	SQL="UPDATE CalendarEvents SET " &_
	"Subject= '" & replace(request.form("Subject"), "'", "''") & "', " &_
	"Details= '" & replace(request.form("Details"), "'", "''") & "', " &_
	"EventDate=" & "#" & DefaultDate & "#" & "," &_
	"EventTime=" & "#" & strStartTime & "#" & "," &_
	"LengthHrs=" & Request.Form("DurHour") & ", " &_
	"LengthMins =" & Request.Form("DurMin") & ", " &_
	"IsAllDayEvent=" & Request.Form("alldayevent") &_
	" WHERE ID = " & Request.QueryString("ID")
	
	
	
	Set RS = conn.execute(SQL)
	


	conn.Close
	Set conn = Nothing
	Set RS = Nothing
	
	Response.Write SQL
	
	'END ADD EVENT	

	
	


End If






%>










<%
	DoneUrl = "Calendar.asp?Show=Yes&CurMonth=" & MonthName(Month(DefaultDate)) & "&CurYear=" & Year(DefaultDate)
	Response.Write "<br>"
	Response.Write DoneUrl

	Response.Redirect(DoneUrl)
'conn.Close
'Set conn = Nothing
'Set RS = Nothing
%>







