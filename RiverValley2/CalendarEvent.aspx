<%@ Page Title="" Language="C#" MasterPageFile="RiverValley.Master" AutoEventWireup="true" CodeBehind="CalendarEvent.aspx.cs" Inherits="RiverValley2.CalendarEvent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
<br /><asp:Label ID="LabelTitle" runat="server" Text=""></asp:Label>
<br /><span class="subtitle"><asp:Literal ID="LiteralDate" runat="server"></asp:Literal></span>
<br /><span class="smalltitle"><asp:Literal ID="LiteralTime" runat="server"></asp:Literal></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

<asp:Label ID="LabelMain" runat="server" Text=""></asp:Label>
<br />
<asp:Literal ID="LiteralDoneButton" runat="server">
<input type="button" value="Done" onclick="history.go(-1)" id="button1" name="button1" class="button-order" />
</asp:Literal>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PictureColumn" runat="server">
<asp:Literal ID="LiteralPictures" runat="server"></asp:Literal>
</asp:Content>

