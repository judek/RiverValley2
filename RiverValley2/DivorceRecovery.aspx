<%@ Page Title="" Language="C#" MasterPageFile="RiverValley.Master" AutoEventWireup="true" CodeBehind="DivorceRecovery.aspx.cs" Inherits="RiverValley2.DivorceRecovery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Divorce Recovery</h1>
    
    <br/>Divorce Recovery Part 1<br/>
    <video width="320" height="240" controls>
   <source src="DivorceRecovery/DivorceRecovery1.mp4" type="video/mp4">
   Your browser does not support the video tag.
</video> <br/><a href=GetFile.aspx?SF=DivorceRecovery/DivorceRecovery1.mp4><img src=picts/icon_download.gif alt="Download this version"> Click here</a> to download <br/> <br/> 

    <br/>Divorce Recovery Part 2<br/>
    <video width="320" height="240" controls>
   <source src="DivorceRecovery/DivorceRecovery2.mp4" type="video/mp4">
   Your browser does not support the video tag.
</video><br/><a href=GetFile.aspx?SF=DivorceRecovery/DivorceRecovery2.mp4><img src=picts/icon_download.gif alt="Download this version"> Click here</a> to download <br/> <br/> 

        <br/>Divorce Recovery Part 3<br/>
    <video width="320" height="240" controls>
   <source src="DivorceRecovery/DivorceRecovery3.mp4" type="video/mp4">
   Your browser does not support the video tag.
</video> <br/><a href=GetFile.aspx?SF=DivorceRecovery/DivorceRecovery3.mp4><img src=picts/icon_download.gif alt="Download this version"> Click here</a> to download <br/> <br/> 

        <br/>Divorce Recovery Part 4<br/>
    <video width="320" height="240" controls>
   <source src="DivorceRecovery/DivorceRecovery4.mp4" type="video/mp4">
   Your browser does not support the video tag.
</video> <br/><a href=GetFile.aspx?SF=DivorceRecovery/DivorceRecovery4.mp4><img src=picts/icon_download.gif alt="Download this version"> Click here</a> to download <br/> <br/> 


    <asp:Label ID="LabelMain" runat="server" Text="LabelMain"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PictureColumn" runat="server">
    <asp:Literal ID="LiteralPictures" runat="server"></asp:Literal>
</asp:Content>
