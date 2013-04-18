<%LANGUAGE="VBScript"%>

<%Session("LoginPage") = "editEvent.asp"%>

<%If Request.Cookies("UserName") = "" Then%>
<%Response.Redirect("../../../login.aspx?ReturnUrl=%2frivervalley%2fEdit%2fDefault.aspx") %>
<%End If%>

<%
If Request.QueryString("ID") = "" Then
		Response.Write "Unable to edit empty record." 
		Response.End
End If


If Request.QueryString("ColID") = "" Then
		Response.Write "Unable to edit empty record." 
		Response.End
End If

Dim nCollectionID

nCollectionID = CInt(Request.QueryString("ColID"))


If nCollectionID < 1 Then
		Response.Write "This is not a collection." 
		Response.End
End If


Function GetRepeatString(nMode1, nMode2)
	Dim RepeatString1, RepeatString2
	
	RepeatString1 = ""
	RepeatString2 = ""

	Select Case nMode1
	  case 1
	    RepeatString1 = "Every "
	  case 2
	    RepeatString1 = "Every other "
	  case 3
	    RepeatString1 = "Every third "
	  case 4
	    RepeatString1 = "Every fourth "
	 end select

	Select Case nMode2
	  case 1
	    RepeatString2 = "Day"
	  case 7
	    RepeatString2 = "Week"
	  case 30
	    RepeatString2 = "Month"
	  case 365
	    RepeatString2 = "Year"
	 end select


GetRepeatString = RepeatString1 & RepeatString2
	
	
End Function


Set conn = Server.CreateObject("ADODB.Connection") 
conn.open "Provider=Microsoft.Jet.OLEDB.4.0; Data Source="& Server.MapPath("../../../database/RiverValleyCalendar.mdb") 

SQL = "SELECT * FROM CalendarEvents WHERE CollectionID = " & nCollectionID  & " ORDER BY EventDate"


Set RS = conn.execute(SQL) 

If RS.EOF Then
		%><p>Documents not found. Please Try again.</p><%
		conn.Close
		Set conn = Nothing
		Set RS = Nothing
		Response.End
End if

RS.MoveFirst

Dim strHour

If Hour(RS("EventTime")) = 0 Then
	strHour = "12 am"

ElseIf Hour(RS("EventTime")) < 13 Then
	strHour = Hour(RS("EventTime")) & " am"
ElseIf Hour(RS("EventTime")) > 12 Then
	strHour = (Hour(RS("EventTime"))-12) & " pm"
Else
	strHour = " "
End If

Dim strPageTitle

If RS("CollectionID") = -1 Then
	strPageTitle = "Edit Yearly Event"
ElseIf RS("CollectionID") > 0 Then
	strPageTitle = "Repating Event"
Else
	strPageTitle = "Edit Event"
End If


%>

<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<meta name="GENERATOR" content="Microsoft Notepad">
<title><%=strPageTitle%></title>
</head>

<body>

    <font size="5" face="Arial"><b><%=strPageTitle%></b>
<form method="POST" action="updateEvents.asp?ID=<%=RS("ID")%>&ColID=<%=nCollectionID%>" id=form1 name=form1>    
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
              <td width="334"><input maxLength="200" size="40" name="Subject" value="<%=RS("Subject")%>"></td>
            </tr>
          </tbody>
        </table>
      </td>
    </tr>
    <tr>
      <td noWrap align="right" width="130">
        <b></b>
       </td>
      <td width="871">
        <table cellSpacing="0" cellPadding="0" border="0">
          <tbody>
            <tr><input type=hidden name="StartMon" value="<%=Left(monthname(Month(RS("EventDate"))), 3)%>">
				<input type=hidden name="StartDay" value=="<%=Day(RS("EventDate"))%>">
				<input type=hidden name="StartYear" value="<%=Year(RS("EventDate"))%>">
              <td><i><b>Starts</b></i> <%= FormatDateTime(RS("EventDate"), 1)%></td>
               <td vAlign="top" noWrap></td>
            </tr>
             <tr>
              <td><i><b>Repeats</b></i> <%=GetRepeatString(RS("RepeatPattern"), RS("RepeatFreq"))%></td>
               <td vAlign="top" noWrap></td>
            </tr>
             <tr>
              <td><i><b>Ends</b></i> <%= FormatDateTime(RS("EndDate"), 1)%></td>
               <td vAlign="top" noWrap></td>
            </tr>
             <tr>
              <td><i><font size="1" face="Arial">(Note: Repeat date and patterns are read only. To modify,<br>delete this collection and create a new one with your<br>desired repeating properties.)</i></td></font>
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
			<%If RS("IsAllDayEvent") = 1 Then%>
              <td vAlign="center"><input type="radio" CHECKED value="1" name="alldayevent"></td>
              <td vAlign="top">This is an <b>all day</b> event.</td>
            </tr>
            <tr>
              <td vAlign="center"><input type="radio" value="0" name="alldayevent"></td>
            <%Else%>
              <td vAlign="center"><input type="radio" value="1" name="alldayevent"></td>
              <td vAlign="top">This is an <b>all day</b> event.</td>
            </tr>
            <tr>
              <td vAlign="center"><input type="radio" CHECKED value="0" name="alldayevent"></td>
			<%End If%>
              <td vAlign="top" noWrap>Starts at: <select name="StartHourMerid">
                  <option value="<%=Hour(RS("EventTime"))%>"><%=strHour%></option>
                  <option value="0">12 am</option>
                  <option value="1">1 am</option>
                  <option value="2">2 am</option>
                  <option value="3">3 am</option>
                  <option value="4">4 am</option>
                  <option value="5">5 am</option>
                  <option value="6">6 am</option>
                  <option value="7">7 am</option>
                  <option value="8">8 am</option>
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
                  <option value="<%=Minute(RS("EventTime"))%>"><%=(":" & Minute(RS("EventTime")))%></option>
                  <option value="0">:00</option>
                  <option value="15">:15</option>
                  <option value="30">:30</option>
                  <option value="45">:45</option>
                </select></td>
            </tr>
            <tr>
              <td>&nbsp;</td>
              <td noWrap>Duration: <select name="DurHour">
                  <option value="<%=RS("LengthHrs")%>"><%=(RS("LengthHrs") & " hrs")%></option>
                  <option value="0">0 hrs</option>
                  <option value="1">1 hr</option>
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
                  <option value="<%=RS("LengthMins")%>"><%=(RS("LengthMins") & " mins")%></option>
                  <option value="0" >0 mins</option>
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
      <td align="right" width="130" valign="top"><b>Details:</b></td>
      <td width="871"><textarea name="Details" rows="15" wrap="virtual" cols="100"><%=RS("Details")%></textarea>
<p><input type="submit" value="Cancel" name="Button">
<input type="submit" value="Save Collection" name="Button">
<input type="submit" value="DELETE Collection" name="Button"></p>

</td>
    </tr>
    
  </tbody>
</table>
</form>


</body>
</html>










</html>
<%conn.Close
Set conn = Nothing
Set RS = Nothing
%>



