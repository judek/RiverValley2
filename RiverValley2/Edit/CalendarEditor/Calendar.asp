<%@ Language=VBScript %>

<%Session("BackPage") = "Calendar.asp"%>

<%If Request.Cookies("UserName") = "" Then%>
<%Response.Redirect("../../../login.aspx?ReturnUrl=%2frivervalley%2fEdit%2fDefault.aspx") %>
<%End If%>

<%




Dim Daylinks, Daylink, EventLinksPrefix, EventLinksSuffix
Daylinks = true
Daylink = "addevent.asp"

EventLinksPrefix = "<a href=edit.asp?ID="
EventLinksSuffix = ">"



Dim strPageTitle
strPageTitle = "ASP Calender Demo"

Dim strTopColor
strTopColor = "#800080"

Dim strTopFontColor
strTopFontColor = "#FFFFFF"

Set conn = Server.CreateObject("ADODB.Connection") 
conn.open "Provider=Microsoft.Jet.OLEDB.4.0; Data Source="& Server.MapPath("../../../database/RiverValleyCalendar.mdb") 

Dim RS
Dim HasRecords
HasRecords = 0



Public Function NDow(Y, M, N, DOW )
	NDow = DateSerial(Y, M, (8 - WeekDay(DateSerial(Y, M, 1),   _ 
	(DOW + 1) Mod 8)) + ((N - 1) * 7))
End Function

Public Function EasterDate(Yr)
	Dim d 
	d = (((255 - 11 * (Yr Mod 19)) - 21) Mod 30) + 21
	EasterDate = DateSerial(Yr, 3, 1) + d + (d > 48) + 6 - ((Yr + Yr \ 4 + _
	d + (d > 48) + 1) Mod 7)
End Function


Public Function DOWsInMonth(Yr, M, DOW)
'Calling this function will tell us how many Mondays there are in May, 1998.
'=DOWsInMonth(1998, 5, 2)

	On Error Resume Next

	Dim I
	Dim Lim
	Lim = Day(DateSerial(Yr, M + 1, 0))
	DOWsInMonth = 0
	For I = 1 To Lim
	    If WeekDay(DateSerial(Yr, M, I)) = DOW Then
	        DOWsInMonth = DOWsInMonth + 1
	    End If
	Next

	If Err.number <> 0 Then
		On Error GoTo 0 
		DOWsInMonth = 0
	End If

End Function



Function GetEventsForDay(dbDate, nDay, strBeginTags, strEndTags)

	GetEventsForDay = ""
	Dim nMonth
	
	nMonth = Month(dbDate)
	nYear = Year(dbDate)

	
	If nMonth = 1 Then
		'New Years day
		If (nDay)  = 1 Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "New Year's Day" & strEndTags
		End If	

		'ML King 3rd Monday of Jan
		If nDay =  Day(NDow(nYear, 1, 3, 2)) Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "Martin Luther King Day" & strEndTags
		End If
	End If

	'Presidents Day 3rd Monday of Feb Mon, Feb 21st 
	If nMonth = 2 Then
		If nDay =  Day(NDow(nYear, 2, 3, 2))Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "Presidents Day" & strEndTags
		End If
	End If

	
	'Easter
	If nMonth = 3 Or nMonth = 4 Then
		Easter= EasterDate(nYear)
		If nDay =  Day(Easter) And nMonth = Month(Easter) Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "Easter Sunday" & strEndTags
		End If
	End If
	
	'May Holidays
	If nMonth = 5 Then
		'2nd Sunday of May Mother's Day	
		If nDay =  Day(NDow(nYear, 5, 2, 1))Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "Mother's Day" & strEndTags
		End If
		'Memorial Day Last Monday of May Mon
		If nDay =  Day(NDow(nYear, 5, DOWsInMonth(nYear, 5, 2), 2))Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "Memorial Day" & strEndTags
		End If
	End If
	
	

	'3rd Sunday of June as Father's Day	
	If nMonth = 6 Then
		If nDay =  Day(NDow(nYear, 6, 3, 1))Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "Father's Day" & strEndTags
		End If
	End If

	'Fouth of July
	If nMonth = 7 And (nDay)  = 4 Then
		GetEventsForDay = GetEventsForDay & strBeginTags & "Independance Day" & strEndTags
	End If	



	'Labor Day 1st Monday of Sept
	If nMonth = 9 Then
		If nDay =  Day(NDow(nYear, 9, 1, 2))Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "Labor Day" & strEndTags
		End If
	End If

	'October Holidays
	If nMonth = 10 Then
		'Halloween
		If (nDay)  = 31 Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "Halloween" & strEndTags
		End If	
		'Columbus Day 2nd Monday of Oct
		If nDay =  Day(NDow(nYear, 10, 2, 2))Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "Columbus Day" & strEndTags
		End If
	End If

	
	'November Holidays
	If nMonth = 11 Then
	
		'Veterans Day 
		If nDay =  11 Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "Veteran's Day" & strEndTags
		End If
	
		'Thanksgiving Day 4th Thursday of nov
		If nDay =  Day(NDow(nYear, 11, 4, 5))Then
			GetEventsForDay = GetEventsForDay & strBeginTags & "Thanksgiving Day" & strEndTags
		End If
	
	
	End If
	
	'Christmas
	If nMonth = 12 And (nDay)  = 25 Then
		GetEventsForDay = GetEventsForDay & strBeginTags & "Christmas Day" & strEndTags
	End If	
	
	
	
	
	'Check for DB Holidays (Fixed)

	
	


	'Check for DB Events
	If HasRecords > 0 Then
		RS.MoveFirst
		Do While NOT RS.EOF

			If nDay = Day(RS("EventDate")) Then
				If nMonth = Month(RS("EventDate")) Then
					GetEventsForDay = GetEventsForDay & strBeginTags & EventLinksPrefix & RS("ID") & "&ColID=" & RS("CollectionID") & EventLinksSuffix & RS("Subject")& "</a>" & strEndTags
				End If
			End If
	
			RS.Movenext
		Loop
	End If
	


End Function



%>
<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<meta name="GENERATOR" content="Microsoft Notepad">
<title><%=strPageTitle%></title>
</head>


<body vlink="#0000FF" alink="#0000FF">

<script language="Javascript">
	//Sending the values when the month and year are selected from the combo boxes
	function SendVal()
	{
		if((document.Cal.MonthNames.options(document.Cal.MonthNames.selectedIndex).text)!="")
		{
			if((document.Cal.ListYears.options(document.Cal.ListYears.selectedIndex).text)!="")
			{
				mon=document.Cal.MonthNames.options(document.Cal.MonthNames.selectedIndex).text				
				yr=document.Cal.ListYears.options(document.Cal.ListYears.selectedIndex).text
				document.location.href="Calendar.asp?Show=Yes&CurMonth=" + mon + "&CurYear=" + yr
			}
		}
	}
</script>



<%
res=Request.QueryString("Show")
whichmonth=Request.QueryString("CurMonth")
whichyear=Request.QueryString("CurYear")
whataction=Request.QueryString("Action")
%>

<% 

dim nextmonth
dim acalandardays(42)
nextmonth = 0

dbcurrentdate = date()

if res="Yes" then
	whichdate="01" & "/" & whichmonth & "/" & whichyear
	if whataction="Next" then
		dbcurrentdate=dateadd("m",1,cdate(whichdate))
	elseif whataction="Prev" then
		dbcurrentdate=dateadd("m",-1,cdate(whichdate))
	else
		dbcurrentdate=whichdate
	end if
end if




Dim MonthLastDay
	
MonthLastDay = dbcurrentdate
MonthLastDay = dateadd("m",1,cdate(dbcurrentdate))
MonthLastDay = dateadd("d",-Day(dbcurrentdate),cdate(MonthLastDay))

Dim SQL
	
SQL = "SELECT * FROM CalendarEvents WHERE EventDate BETWEEN #" & Month(dbcurrentdate) & "/1/" & Year(dbcurrentdate) & "# AND #" & FormatDateTime(MonthLastDay , 0) & "# OR CollectionID = -1 ORDER BY ID"

'Response.Write SQL

Set RS = conn.execute(SQL) 

If NOT RS.EOF Then
	HasRecords = 1
Else
	HasRecords = 0
End If



dim ifirstweekday
ifirstweekday = datepart("W",dateserial(year(dbcurrentdate),month(dbcurrentdate),1))
dim idaysinmonth 
idaysinmonth = datepart("d",dateserial(year(dbcurrentdate),month(dbcurrentdate)+1,1-1))
dim iloop
for iloop = 1 to idaysinmonth
	acalandardays(iloop+ifirstweekday-1)=iloop
next

dim icolumns,irows
icolumns = 7 
irows = 6-int((42-(ifirstweekday+idaysinmonth))/7)
%>
<%If Daylinks = true Then%>
    <%=Request.Cookies("UserName")%>,<br />
	<p><b>To add an event, click on any day of the month.
	<br>To edit an event, click on the event.<b></p>
<%Else%>
	<p><img border="0" src="Calendar.gif"></p>
	<b><font face="Arial"><a href="Calendar.asp">Today is <%= FormatDateTime(Date, 1)%></font></b></a>
<%End If%>
<table align =left border=5 cellspacing = 1 width=60% height=60%>
<th colspan=7 bgcolor="<%=strTopColor%>">
<%
curmonth=monthname(month(dbcurrentdate))
curyear=year(dbcurrentdate)
%>
<b><font color="<%=strTopFontColor%>" font size=5 face="Arial">
<a href="Calendar.asp?Show=Yes&CurMonth=<%=left(curmonth,3)%>&CurYear=<%=curyear%>&Action=Prev"><img border="0" src="leftl1.gif"></a>
<%
Response.Write curmonth
Response.Write " " & curyear%> 
<a href="Calendar.asp?Show=Yes&CurMonth=<%=left(curmonth,3)%>&CurYear=<%=curyear%>&Action=Next"><img border="0" src="rightl1.gif"></a>
</b></font>
<%
Response.Write	"<tr>"
Response.Write "<td align=center bgcolor=""#C0C0C0"">"
Response.Write "<font color=red font face=""Arial""><b>"
Response.Write "Sun"
Response.Write "</td>"
Response.Write "<td align=center bgcolor=""#C0C0C0""><b>"
Response.Write "<font face=""Arial"">Mon"
Response.Write "</td>"
Response.Write "<td align=center bgcolor=""#C0C0C0""><b>"
Response.Write "<font face=""Arial"">Tues"
Response.Write "</td>"
Response.Write "<td align=center bgcolor=""#C0C0C0""><b>"
Response.Write "<font face=""Arial"">Wed"
Response.Write "</td>"
Response.Write "<td align=center bgcolor=""#C0C0C0""><b>"
Response.Write "<font face=""Arial"">Thur"
Response.Write "</td>"
Response.Write "<td align=center bgcolor=""#C0C0C0""><b>"
Response.Write "<font face=""Arial"">Fri"
Response.Write "</td>"
Response.Write "<td align=center bgcolor=""#C0C0C0""><b>"
Response.Write "<font face=""Arial"">Sat"
Response.Write "</td>"

Response.Write	"</tr>"
%>

</font>

<% 
dim irowsloop ,icolumnsloop, displaynDay, TodayBgColor
startday=weekday("01" & "/" & monthname(month(dbcurrentdate)) & "/" & year(dbcurrentdate))
ctr=1

TodayBgColor = "#FFFFFF"

for irowsloop = 1 to irows
	Response.Write "<tr>"
	if ctr=1 then
		colm = startday
	else
		colm=1
	end if
	if startday>1 then
		diff=colm - 1
	end if
		
		for icolumnsloop=colm to icolumns
			displaynDay = acalandardays((irowsloop-1)*7 + icolumnsloop)
			if displaynDay >= 0 then
				if diff>0 then
					for p=1 to diff
						%><td valign= top align=right width="14%">
						</td><%
					next
					diff=0
				end if
					
					If displaynDay = Day(Date()) Then
						If Year(dbcurrentdate)= Year(Date())And month(dbcurrentdate)= month(Date()) Then
							TodayBgColor = "#C0C0C0"
						End If
					Else
						TodayBgColor = "#FFFFFF"
					End If
					
				

					%><td valign= top align=right bgcolor="<%=TodayBgColor%>" width="14%"><%
					if icolumnsloop=1 then
						%><font color=red><%
					end if
					if month(dbcurrentdate)<month(Date()) then
						if displaynDay=day(Date()) and month(dbcurrentdate)=month(Date()) and year(dbcurrentdate)=year(Date()) then
							%><font color=blue><%
						end if
						Response.Write displaynDay 			
					elseif month(dbcurrentdate)=month(Date()) and displaynDay < day(Date()) then

						If Daylinks = true Then
							Response.Write "<a href=" & Daylink & "?Day=" & displaynDay & "-" & curmonth & "-" & curyear & ">" & displaynDay & "</a>"
						Else
							Response.Write displaynDay
						End If

					elseif month(dbcurrentdate)=month(Date()) and displaynDay >= day(Date())then
						If Daylinks = true Then
							Response.Write "<a href=" & Daylink & "?Day=" & displaynDay & "-" & curmonth & "-" & curyear & ">" & displaynDay & "</a>"
						Else
							Response.Write displaynDay
						End If
					elseif month(dbcurrentdate)>month(Date()) then
						If Daylinks = true Then
							Response.Write "<a href=" & Daylink & "?Day=" & displaynDay & "-" & curmonth & "-" & curyear & ">" & displaynDay & "</a>"
						Else
							Response.Write displaynDay
						End If
					end if
					'Response.Write "<a href=ToSomePage.asp?" & displaynDay & "-" & curmonth & "-" & curyear & ">" & displaynDay & "</a>"
					'Response.Write displaynDay
					'BEGIN FILL IN EVENT TITLE FOR THE DAY HERE
					
					%><%=GetEventsForDay(dbcurrentdate, displaynDay, "<br><font size=1>", "</font>")%><%

					'END FILL IN EVENT TITLE

					%></td><%
			else
				%><td>&nbsp;</td><%
			end if
			ctr=2
		next
		%><tr><%
	colm=2
next
%><br>
</table>
<br clear="left">
<form name="Cal">
<tr>
	<select name="MonthNames">
		<option selected><%=curmonth%></option>
		<option>January</option>
		<option>February</option>
		<option >March</option>
		<option>April</option>
		<option>May</option>
		<option>June</option>
		<option>July</option>
		<option>August</option>
		<option>September</option>
		<option>October</option>
		<option>November</option>
		<option>December</option>
	</select>

	<select name="ListYears">
		<option><%=Year(dbcurrentdate)%></option>
	<% 
	

	for yr = (Year(dbcurrentdate) - 5) to (Year(dbcurrentdate) + 5) %>
		<option><%=yr%></option>
	<% next %>
	</select>

	<input type=button value="Go To Month" onclick="SendVal()" id=button1 name=button1>
</tr>
</form>
</body>
</html>
<%conn.Close
Set conn = Nothing
Set RS = Nothing
%>














































































