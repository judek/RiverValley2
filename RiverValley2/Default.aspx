<%@ Page Title="" Language="C#" MasterPageFile="HomePage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RiverValley2.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<tr>
      <td colspan="4"></td>
    </tr>

    <tr>
      <td colspan="4" valign="top" ><asp:Literal ID="LiteralBanner" runat="server"></asp:Literal></td>
	   
	   
	    <!--<td valign="top" >
	         <img src="images/logo.jpg" width="233" height="322" alt="" /> 
	        </td>-->
      </tr>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentEvents" runat="server">

<asp:Literal ID="LiteralComingEvents" runat="server"></asp:Literal>
<!--<a href="">Master's Touch Graduation Tuesday</a> 3/30/20101<br /> 
<a href="">Changes That Heal</a>  3/31/20101 <br />
<a href="">Hesed House</a>  4/10/20101<br />
<a href="">Beth Moore's So Long Insecurity</a>  4/24/20101<br /> -->
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentSermons" runat="server">
<asp:Literal ID="LiteralSermons" runat="server"></asp:Literal>

   <!--  <a href="">James 2:14-26</a> - Andy Morgan<br />
      <a href="">James 2:1-13</a> - Andy Morgan<br />
      <a href="">James 1:19-27</a> - Andy Morgan<br /> -->
</asp:Content>