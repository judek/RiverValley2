<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RiverValley2.Edit.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<link href="../style.css" rel="stylesheet" type="text/css" />

    <title>River Valley Web Site Editor</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
       <span class="title">River Valley Web Site Editor</span>
       <span class="smalltitle"> 
       <asp:Button ID="ButtonLogoff" runat="server" Text="Stop Editing" 
                onclick="ButtonLogoff_Click" CssClass="button-edit" />
        <br />
        <br />Click any link below</span>
                   
        
        <br />
        <br />
        
        
        
        <ul>
        
        <br /><span class="subtitle"><a href="CalendarEdit/Default.aspx">Calendar</a></span><br />
        <ul><span class="phonetitle">Add, delete, and edit calendar events</span></ul>
        
        <br /><span class="subtitle"><a href="SendTweet/Default.aspx">Twitter</a></span><br />
        <ul><span class="phonetitle">Send Tweets on behalf of River Valley Church</span></ul>

        <br /><span class="subtitle"><a href="MultimediaEdit/Default.aspx">Mulitmedia (Sermons and videos)</a></span><br />
        <ul><span class="phonetitle">
        Delete, Edit and Tag(Classify) multimedia content.
        <br />Note:All Sermons need to be tagged with 'Sermon' and all videos need to be tagged with 'Video'</span></ul>
        
       <br /><span class="subtitle"><a href="GalleryEdit/Default.aspx">Picture Gallery</a></span><br />
        <ul><span class="phonetitle">Upload, delete, rotate, edit and change picture captions</span></ul>
       
        <br /><span class="subtitle"><a href="HomeworkUpload/Default.aspx">Add and remove page attachments</a></span><br />
        <ul><span class="phonetitle">Used for uploading daily homework, or to add any file to a page viewers can download
        <br />Navigate to ANY page (excluding indivigual calendar event and sermon pages) see controls just above the picture column.
        <br />Note:Upload only PDF files for best user experience</span></ul>
        
         <br /><span class="subtitle"><a href="contentedit/Default.aspx">Static Page Content</a></span><br />
        <ul><span class="phonetitle">Edit page contents; roll page back to previous date/time. Add file attachments to pages.</span></ul>
        
        </ul>
        
    <table><tr><td>
        <span class="subtitle"><br /><br />Help<br /></span>
        <ul>
            <span class="smalltitle"><br />Handy Short cuts</span>
            <br />Us the # symbol followed by a space then your text.  Use as shown below.<br />
            <h1># Page Title or Heading 1</h1>
            <h2>## Page Sub Title or Heading 2</h2>
            <h3>### Page Small Title or Heading 3</h3>
            <h4>#### This is text just bigger than normal use in place of bolding text or Heading 3</h4>
            <br />This is regular text or no Headings
            
            <br /><br />
            
            <span class="smalltitle"><br />Making italic text</span>
            <br /><i>&lt;i>Do this by wraping text with the i tag&lt;/i></i>
            <br /><span class="smalltitle"><br />Underling text</span>
            <br /><u>&lt;u>Do this by wraping text with the u tag&lt;/u></u>
              <br /><span class="smalltitle"><br />Bolding text</span>
            <br />Bolding is not supported cause all styles are already bolded

            <br /><span class="smalltitle"><br />Indenting text, bullenting and numbering</span>
            <br /><br />This can be done by adding a little html code see <a href="http://www.w3schools.com/html/html_lists.asp" target="_blank">this page</a> for details
           

        </ul>
        </td></tr></table>
        
        </div>
    </form>
</body>
</html>
