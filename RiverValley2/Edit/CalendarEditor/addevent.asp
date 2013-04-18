<%LANGUAGE="VBScript"%>

<%Session("LoginPage") = "addevent.asp"%>

<%If Request.Cookies("UserName") = "" Then%>
<%Response.Redirect("../../../login.aspx?ReturnUrl=%2frivervalley%2fEdit%2fDefault.aspx") %>
<%End If%>

<%
Dim IsDebug
IsDebug = true
%>
<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<meta name="GENERATOR" content="Microsoft Notepad">
<title>Add Event</title>
</head>

<body>



<%

Function HandleASPErrors()
	If Err.number <> 0 Then
		Response.Write "<b><font color=#FF0000>"
		Response.Write Err.Description & "</font></b><br>Unable to complete operation<br>Hit the back button and try again"
		Response.End
	End If
End Function

Function GetNextDate(CurrentDate)

	Dim nMode
	nMode = CInt(Request.Form("RepeatFreq1"))
	
	'Day
	If Request.Form("RepeatFreq2") = 1 Then
		GetNextDate = DateAdd("d", (1*nMode), DefaultDate)
	End If
	
	'Week
	If Request.Form("RepeatFreq2") = 7 Then
		GetNextDate = DateAdd("d", (7*nMode), DefaultDate)
	End If
	
	'Month
	If Request.Form("RepeatFreq2") = 30 Then
		GetNextDate = DateAdd("M", (1*nMode), DefaultDate)
	End If

	'Year
	If Request.Form("RepeatFreq2") = 365 Then
		GetNextDate = DateAdd("yyyy", (1*nMode), DefaultDate)
	End If

	
End Function



Dim sErrors

Dim DefaultDate
Dim DefaultEndDate  

If true = IsDate(Request.QueryString("Day")) Then
	DefaultDate = CDate(Request.QueryString("Day"))
	DefaultEndDate = CDate(Request.QueryString("Day"))
Else
	DefaultDate = Date()
	DefaultEndDate = Date()
End If



sErrors = ""

If Request.Form("Button") = "ADD Event" Then

	Dim strStartDate
	Dim strEndDate
	Dim strStartTime
	Dim DoneUrl

	strStartDate = Request.Form("StartMon") & "-"  & Request.Form("StartDay") & "-" & Request.Form("StartYear")
	strEndDate = Request.Form("EndMon") & "-"  & Request.Form("EndDay") & "-" & Request.Form("EndYear")		
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
	CalReturnDate = CDate(strStartDate)

	If Request.Form("Repeat") = 1 Then
		DefaultEndDate = CDate(strEndDate)	
	Else
		DefaultEndDate = CDate(strStartDate)	
	End If


	

	
	'BEGIN ADD EVENT
	On Error Resume Next
	Err.Clear
	
	Set conn = Server.CreateObject("ADODB.Connection") 
	conn.open "Provider=Microsoft.Jet.OLEDB.4.0; Data Source="& Server.MapPath("../../../database/RiverValleyCalendar.mdb")  
	HandleASPErrors()
	
	Dim NextAutoNumberValue
	NextAutoNumberValue = 0
	
	If Request.Form("Repeat") = 2 Then
		NextAutoNumberValue = -1'Setting collection ID to -1 means simple yearly repeating event with no end date
	End If
	
	
	
	SQL="INSERT INTO CalendarEvents (Subject, Details, EventDate, EndDate, HasEndDate, IsAllDayEvent, EventTime, LengthMins, LengthHrs, RepeatPattern, RepeatFreq, CollectionID) Values (" &_ 
	"'" & replace(Request.Form("Subject"), "'", "''") & "', " &_ 
	"'" & replace(Request.Form("Details"), "'", "''") & "', " &_
	"'" & DefaultDate & "', " &_
	"'" & DefaultEndDate & "', " &_
	"1" & "," &_
	Request.Form("alldayevent") & "," &_
	"#" & strStartTime & "#" & "," &_
	Request.Form("DurMin") & "," &_
	Request.Form("DurHour") & "," &_
	Request.Form("RepeatFreq1") & "," &_
	Request.Form("RepeatFreq2") & "," &_
	NextAutoNumberValue & ")" 
	
	Set RS = conn.execute(SQL)
	HandleASPErrors()
	

	If Request.Form("Repeat") = 1 Then
		SQL = "select max(ID) as maxvalue from CalendarEvents"
		Set RS = conn.execute(SQL)
		HandleASPErrors()

		If RS.EOF Then
		    NextAutoNumberValue = 1
		Else
		    NextAutoNumberValue = RS("maxvalue") + 1
		End If

		SQL = "DELETE FROM CalendarEvents WHERE ID =" & RS("maxvalue")
		Set RS = conn.execute(SQL)
		HandleASPErrors()

	End If



	If Request.Form("Repeat") = 1 Then
	
		Dim Count
		Dim MaxCount
		Dim CollectionID
		
		Count = 0
		MaxCount = 20
		CollectionID = 0
		
		Response.Write "Repeating..."

		
		Do While (DateDiff("d",DefaultDate,DefaultEndDate) > -1) And (Count < MaxCount)
		
			SQL="INSERT INTO CalendarEvents (Subject, Details, EventDate, EndDate, HasEndDate, IsAllDayEvent, EventTime, LengthMins, LengthHrs, RepeatPattern, RepeatFreq, CollectionID) Values (" &_ 
			"'" & replace(Request.Form("Subject"), "'", "''") & "', " &_ 
			"'" & replace(Request.Form("Details"), "'", "''") & "', " &_
			"'" & DefaultDate & "', " &_
			"'" & DefaultEndDate & "', " &_
			"1" & "," &_
			Request.Form("alldayevent") & "," &_
			"#" & strStartTime & "#" & "," &_
			Request.Form("DurMin") & "," &_
			Request.Form("DurHour") & "," &_
			Request.Form("RepeatFreq1") & "," &_
			Request.Form("RepeatFreq2") & "," &_
			NextAutoNumberValue & ")" 
	
			Set RS = conn.execute(SQL)
			HandleASPErrors()
		
			DefaultDate = GetNextDate(DefaultDate)
			Count = Count + 1
		Loop

	
	End If






	conn.Close
	Set conn = Nothing
	Set RS = Nothing
	HandleASPErrors()
	'END ADD EVENT	
	On Error GoTo 0	
	
	
	DoneUrl = "Calendar.asp?Show=Yes&CurMonth=" & MonthName(Month(CalReturnDate)) & "&CurYear=" & Year(CalReturnDate)
	Response.Redirect(DoneUrl)

End If


%>


    <h2>Add Calendar Event</h2>
<form method="POST" action="addevent.asp">    
<table cellSpacing="0" cellPadding="4" width="1021" border="0">
  <tbody>
    <tr >
      <td colSpan="2" width="1011">
        <h4>Primary Information</h4>
      </td>
    </tr>
    <tr vAlign="top">
      <td align="right" width="130"><strong>Subject:</strong></td>
      <td width="871">
        <table cellSpacing="0" cellPadding="0" border="0" width="336">
          <tbody>
            <tr>
              <td width="334"><input maxLength="200" size="40" name="Subject" value=""></td>
            </tr>
          </tbody>
        </table>
      </td>
    </tr>
    <tr>
      <td noWrap align="right" width="130">
        <b>Date:</b>
       </td>
      <td width="871">
        <table cellSpacing="0" cellPadding="0" border="0">
          <tbody>
            <tr>
              <td><select name="StartMon">
 `                <option value="<%=Left(monthname(Month(DefaultDate)), 3)%>"><%=Left(monthname(Month(DefaultDate)), 3)%></option>
                  <option value="Jan">Jan</option>
                  <option value="Feb">Feb</option>
                  <option value="Mar">Mar</option>
                  <option value="Apr">Apr</option>
                  <option value="May">May</option>
                  <option value="Jun">Jun</option>
                  <option value="Jul">Jul</option>
                  <option value="Aug">Augt</option>
                  <option value="Sep">Sep</option>
                  <option value="Oct">Oct</option>
                  <option value="Nov">Nov</option>
                  <option value="Dec">Dec</option>
                </select> <select name="StartDay">
                  
                  <option value="<%=Day(DefaultDate)%>"><%=Day(DefaultDate)%></option>
                  <option value="1">1</option>
                  <option value="2">2</option>
                  <option value="3">3</option>
                  <option value="4">4</option>
                  <option value="5">5</option>
                  <option value="6">6</option>
                  <option value="7">7</option>
                  <option value="8">8</option>
                  <option value="9">9</option>
                  <option value="10">10</option>
                  <option value="11">11</option>
                  <option value="12">12</option>
                  <option value="13">13</option>
                  <option value="14">14</option>
                  <option value="15">15</option>
                  <option value="16">16</option>
                  <option value="17">17</option>
                  <option value="18">18</option>
                  <option value="19">19</option>
                  <option value="20">20</option>
                  <option value="21">21</option>
                  <option value="22">22</option>
                  <option value="23">23</option>
                  <option value="24">24</option>
                  <option value="25">25</option>
                  <option value="26">26</option>
                  <option value="27">27</option>
                  <option value="28">28</option>
                  <option value="29">29</option>
                  <option value="30">30</option>
                  <option value="31">31</option>
                </select> <select name="StartYear">
                <option value="<%=Year(DefaultDate)%>"><%=Year(DefaultDate)%></option>
                  <option value="2004">2004</option>
                  <option value="2005">2005</option>
                  <option value="2006">2006</option>
                  <option value="2007">2007</option>
                  <option value="2008">2008</option>
                  <option value="2009">2009</option>
                  <option value="2010">2010</option>
                  <option value="2011">2011</option>
                </select></td>
              <td vAlign="top" noWrap></td>
            </tr>
          </tbody>
        </table>
      </td>
    </tr>
    <tr>
      <td vAlign="top" noWrap align="right" width="130">
       <b>Time:</b>
      </td>
      <td vAlign="top" width="871">
        <table cellSpacing="0" cellPadding="3" border="0">
          <tbody>
            <tr>
              <td vAlign="center"><input type="radio" CHECKED value="1" name="alldayevent"></td>
              <td vAlign="top">This is an <b>all day</b> event.</td>
            </tr>
            <tr>
              <td vAlign="center"><input type="radio" value="0" name="alldayevent"></td>
              <td vAlign="top" noWrap>Starts at: <select name="StartHourMerid">
                  <option value="0">12 am</option>
                  <option value="1">1 am</option>
                  <option value="2">2 am</option>
                  <option value="3">3 am</option>
                  <option value="4">4 am</option>
                  <option value="5">5 am</option>
                  <option value="6">6 am</option>
                  <option value="7">7 am</option>
                  <option value="8" SELECTED>8 am</option>
                  <option value="9">9 am</option>
                  <option value="10">10 am</option>
                  <option value="11">11 am</option>
                  <option value="12">12 pm</option>
                  <option value="13">1 pm</option>
                  <option value="14">2 pm</option>
                  <option value="15">3 pm</option>
                  <option value="16">4 pm</option>
                  <option value="17">5 pm</option>
                  <option value="18">6 pm</option>
                  <option value="19">7 pm</option>
                  <option value="20">8 pm</option>
                  <option value="21">9 pm</option>
                  <option value="22">10 pm</option>
                  <option value="23">11 pm</option>
                </select> <select name="StartMin">
                  <option value="0">:00</option>
                  <option value="15">:15</option>
                  <option value="30">:30</option>
                  <option value="45">:45</option>
                </select></td>
            </tr>
            <tr>
              <td>&nbsp;</td>
              <td noWrap>Duration: <select name="DurHour">
                  <option value="0">0 hrs</option>
                  <option value="1" SELECTED>1 hr</option>
                  <option value="2">2 hrs</option>
                  <option value="3">3 hrs</option>
                  <option value="4">4 hrs</option>
                  <option value="5">5 hrs</option>
                  <option value="6">6 hrs</option>
                  <option value="7">7 hrs</option>
                  <option value="8">8 hrs</option>
                  <option value="9">9 hrs</option>
                  <option value="10">10 hrs</option>
                  <option value="11">11 hrs</option>
                  <option value="12">12 hrs</option>
                </select> <select name="DurMin">
                  <option value="0" selected>0 mins</option>
                  <option value="15">15 mins</option>
                  <option value="30">30 mins</option>
                  <option value="45">45 mins</option>
                </select></td>
            </tr>
          </tbody>
        </table>
      </td>
    </tr>
    <tr>
      <td align="right" width="130"><b>Details:</b></td>
      <td width="871"><textarea name="Details" rows="5" wrap="virtual" cols="40"></textarea>
</td>
    </tr>
    <tr>
      <td colSpan="2" width="1011">
          <h4>Repeating Information</h4>
      </td>
    </tr>
    <tr>
      <td width="130">&nbsp;</td>
      <td vAlign="top" width="871">
          <table cellSpacing="0" cellPadding="3" border="0">
            <tbody>
              <tr>
                <td><input type="radio" CHECKED value="0" name="Repeat">This event does not repeat.</td>
              </tr>
              <tr>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td><input type="radio" value="2" name="Repeat">Repeat as Yearly Event&nbsp(Holiday Aniversery Birthday, etc.)</td>
              </tr>
              <tr>
                <td>&nbsp;</td>
              </tr>

              <tr>
                <td><input type="radio" value="1" name="Repeat">Repeat <select name="RepeatFreq1">
                    <option value="0" >-SELECT-</option>
                    <option value="1">Every</option>
                    <option value="2">Every other</option>
                    <option value="3">Every third</option>
                    <option value="4">Every fourth</option>
                  </select> <select name="RepeatFreq2">
					<option value="0" >-SELECT-</option>
                    <option value="1" >Day</option>
                    <option value="7">Week</option>
                    <option value="30">Month</option>
                    <option value="365">Year</option>
                    <!--<option value="6">Mon, Wed, Fri</option>
                    <option value="7">Tue &amp; Thu</option>
                    <option value="8">Mon Thru Fri</option>
                    <option value="9">Sat &amp; Sun</option>-->
                  </select><br>
                </td>
              </tr>
                      <tr>
                        <td vAlign="top"><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Until:</b><!--
                          <br>
                          <input type="radio" CHECKED value="a" name="until">&nbsp;No
                          End Date.<br>
                          <input type="radio" value="u" name="until">&nbsp;Until-->
                          <select name="EndMon">
                            <option value="<%=Left(monthname(Month(DefaultDate)), 3)%>"><%=Left(monthname(Month(DefaultDate)), 3)%></option>
							<option value="Jan">Jan</option>
							<option value="Feb">Feb</option>
							<option value="Mar">Mar</option>
							<option value="Apr">Apr</option>
							<option value="May">May</option>
							<option value="Jun">Jun</option>
							<option value="Jul">Jul</option>
							<option value="Aug">Augt</option>
							<option value="Sep">Sep</option>
							<option value="Oct">Oct</option>
							<option value="Nov">Nov</option>
							<option value="Dec">Dec</option>
                          </select> <select name="EndDay">
                            <option value="<%=Day(DefaultDate)%>"><%=Day(DefaultDate)%></option>
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9">9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                            <option value="13">13</option>
                            <option value="14">14</option>
                            <option value="15">15</option>
                            <option value="16">16</option>
                            <option value="17">17</option>
                            <option value="18">18</option>
                            <option value="19">19</option>
                            <option value="20">20</option>
                            <option value="21">21</option>
                            <option value="22">22</option>
                            <option value="23">23</option>
                            <option value="24">24</option>
                            <option value="25">25</option>
                            <option value="26">26</option>
                            <option value="27">27</option>
                            <option value="28">28</option>
                            <option value="29">29</option>
                            <option value="30">30</option>
                            <option value="31">31</option>
                          </select> <select name="EndYear">
                          <option value="<%=Year(DefaultDate)%>"><%=Year(DefaultDate)%></option>
                          <%For yr = (Year(DefaultDate) - 5) to (Year(DefaultDate) + 10) %>
									<option value="<%=yr%>"><%=yr%></option>
								<% next %>
								</select>
                          </select></td>
                      </tr>

              <!--<tr>
                <td><input type="radio" value="2" name="Repeat">Repeat on the <select name="WF">
                    <option value="1" selected>First</option>
                    <option value="2">Second</option>
                    <option value="3">Third</option>
                    <option value="4">Fourth</option>
                    <option value="-1">Last</option>
                  </select> <select name="WD">
                    <option value="0" selected>Sun</option>
                    <option value="1">Mon</option>
                    <option value="2">Tue</option>
                    <option value="3">Wed</option>
                    <option value="4">Thu</option>
                    <option value="5">Fri</option>
                    <option value="6">Sat</option>
                  </select> of the month every <select name="WX">
                    <option value="1" selected>month</option>
                    <option value="2">other month</option>
                    <option value="3">3 months</option>
                    <option value="4">4 months</option>
                    <option value="6">6 months</option>
                    <option value="7">year</option>
                  </select></td>
              </tr>
              <tr>
                <td>&nbsp;</td>
              </tr>-->
              <tr>
                <td>
                  <table cellSpacing="0" cellPadding="3" width="100%" border="0">
                    <tbody>
                    </tbody>
                  </table>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </td>
    </tr>
  </tbody>
</table>
<p><input type="submit" value="ADD Event" name="Button"></p>
</form>


</body>
</html>






































