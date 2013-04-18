<%@ Page Title="" Language="C#" MasterPageFile="RiverValley.Master" AutoEventWireup="true" CodeBehind="Discipleship.aspx.cs" Inherits="RiverValley2.Discipleship" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
   <%-- <asp:Button ID="ButtonUpload" runat="server" Text="Upload Homework" 
        onclick="ButtonUpload_Click" CssClass="button-edit" />
<asp:FileUpload ID="FileUpload1" runat="server" />
 <font color="red"><b><asp:Literal ID="LiteralMessage" runat="server"></asp:Literal></b></font>--%>
                
<asp:Label ID="LabelMain" runat="server" Text="LabelMain"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PictureColumn" runat="server">
  <%--  <br /><asp:Literal ID="LiteralHomeworkLink" runat="server"></asp:Literal><br />
Or <a href="Multimedia.aspx">view Sermon Archive</a><br />--%>
<asp:Literal ID="LiteralPictures" runat="server"></asp:Literal>
</asp:Content>
