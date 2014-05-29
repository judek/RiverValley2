<%@ Page Title="" Language="C#" MasterPageFile="RiverValley.Master" AutoEventWireup="true" CodeBehind="Multimedia.aspx.cs" Inherits="RiverValley2.Multimedia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
<asp:Literal ID="LiteralPageTitle" runat="server"></asp:Literal>
<br /><span class="phonetitle">Click on Sermon name to listen to audio</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<asp:Label ID="LabelMain" runat="server" Text=""></asp:Label>
<table border="0"><tr><td>
<asp:Label ID="LabelTagCloud" runat="server" Text=""></asp:Label>
</td></tr></table>
<asp:Label ID="LabelMultiMediaFiles" runat="server" Text=""></asp:Label>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PictureColumn" runat="server">
<asp:Label ID="LabelRightSideBarArea" runat="server" Text=""></asp:Label>
</asp:Content>
