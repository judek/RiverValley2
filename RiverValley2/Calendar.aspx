<%@ Page Title="" Language="C#" MasterPageFile="RiverValley.Master" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="RiverValley2.Calendar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
<h1>River Valley Calendar</h1></h1><div class="page-splits">&nbsp;</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

<asp:Label ID="LabelMain" runat="server" Text=""></asp:Label>
<asp:Label ID="LabelTodayMessage" runat="server" 
        Text="LabelTodayMessage"></asp:Label>
        <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="White" 
        BorderWidth="1px" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
        Height="424px" Width="719px" CellPadding="0" 
        CaptionAlign="Top" ShowGridLines="True" 
    ondayrender="Calendar1_DayRender" 
    onvisiblemonthchanged="Calendar1_VisibleMonthChanged" 
            onselectionchanged="Calendar1_SelectionChanged">
    <SelectedDayStyle BackColor="#009999" ForeColor="#CCFF99" Font-Bold="True" />
    <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
    <WeekendDayStyle BackColor="#0F1B30" ForeColor="White" />
    <TodayDayStyle BackColor="White" ForeColor="Black" />
    <OtherMonthDayStyle ForeColor="#999999" />
    <DayStyle BackColor="#0F1B30" ForeColor="White" />
    <NextPrevStyle Font-Size="20pt" ForeColor="Black" />
    <DayHeaderStyle BackColor="White" ForeColor="#58780B" Height="1px" />
    <TitleStyle BackColor="White" BorderColor="#3366CC" BorderWidth="1px" 
        Font-Bold="True" Font-Size="20pt" ForeColor="Black" Height="25px" />
    </asp:Calendar>
   
        
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PictureColumn" runat="server">
<asp:Literal ID="LiteralComingEvents" runat="server"></asp:Literal>
</asp:Content>
