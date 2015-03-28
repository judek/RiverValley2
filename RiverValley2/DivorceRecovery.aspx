<%@ Page Title="" Language="C#" MasterPageFile="RiverValley.Master" AutoEventWireup="true" CodeBehind="DivorceRecovery.aspx.cs" Inherits="RiverValley2.DivorceRecovery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Divorce Recovery</h1>
    
    <br/><h2>Part 1</h2><br/>
    <video controls poster="DivorceRecovery/DivorceRecovery1.png" width="320" height="240" controls>
   <source src="DivorceRecovery/DivorceRecovery1.mp4" type="video/mp4">
   Your browser does not support the video tag.
</video> <br/><a href=DivorceRecovery/DivorceRecovery1.mp4><img src=picts/icon_download.gif alt="Download this version"> Right click here and select save as</a> to download <br/> <br/> 

    <br/><h2>Part 2</h2><br/>
    <video controls poster="DivorceRecovery/DivorceRecovery2.png" width="320" height="240" controls>
   <source src="DivorceRecovery/DivorceRecovery2.mp4" type="video/mp4">
   Your browser does not support the video tag.
</video><br/><a href=DivorceRecovery/DivorceRecovery2.mp4><img src=picts/icon_download.gif alt="Download this version"> Right click here and select save as</a> to download <br/> <br/> 

        <br/><h2>Part 3</h2><br/>
    <video controls poster="DivorceRecovery/DivorceRecovery3.png" width="320" height="240" controls>
   <source src="DivorceRecovery/DivorceRecovery3.mp4" type="video/mp4">
   Your browser does not support the video tag.
</video> <br/><a href=DivorceRecovery/DivorceRecovery3.mp4><img src=picts/icon_download.gif alt="Download this version"> Right click here and select save as</a> to download <br/> <br/> 

        <br/><h2>Part 4</h2><br/>
    <video controls poster="DivorceRecovery/DivorceRecovery4.png" width="320" height="240" controls>
   <source src="DivorceRecovery/DivorceRecovery4.mp4" type="video/mp4">
   Your browser does not support the video tag.
</video> <br/><a href=DivorceRecovery/DivorceRecovery4.mp4><img src=picts/icon_download.gif alt="Download this version"> Right click here and select save as</a> to download <br/> <br/> 


    <asp:Label ID="LabelMain" runat="server" Text="LabelMain"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PictureColumn" runat="server">
    <asp:Literal ID="LiteralPictures" runat="server"></asp:Literal>
</asp:Content>
