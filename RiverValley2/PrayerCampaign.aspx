<%@ Page Title="" Language="C#" MasterPageFile="RiverValley.Master" AutoEventWireup="true" CodeBehind="PrayerCampaign.aspx.cs" Inherits="RiverValley2.PrayerCampaign" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 154px;
        }
        .style3
        {
            width: 129px;
        }
        .style4
        {
            width: 304px;
        }
        .style5
        {
            width: 154px;
            height: 18px;
        }
        .style6
        {
            width: 129px;
            height: 18px;
        }
        .style7
        {
            width: 304px;
            height: 18px;
        }
        .style8
        {
            width: 154px;
            height: 20px;
        }
        .style9
        {
            width: 129px;
            height: 20px;
        }
        .style10
        {
            width: 304px;
            height: 20px;
        }
        .style11
        {
            width: 154px;
            height: 22px;
        }
        .style12
        {
            width: 129px;
            height: 22px;
        }
        .style13
        {
            width: 304px;
            height: 22px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<asp:Label ID="LabelMainBeforeForm" runat="server" Text=""></asp:Label>


<table border="0" cellpadding="1" cellspacing="0" width="100%">

<tr><td class="style2">

        First Name<br />

</td><td class="style3">

        Last Name</td><td class="style4">

            <span class="smalltitle">Select a Prayer Campaign to join<span></span></td></tr>



    <tr><td class="style2">

<asp:TextBox  ID="TextBoxFirstName" runat="server" size="30" class="shadeform"></asp:TextBox>
        <br />

</td><td class="style3">

<asp:TextBox  ID="TextBoxLastName" runat="server" size="30" class="shadeform"></asp:TextBox>

</td><td class="style4">

            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                <asp:ListItem Value="Nothing">--SELECT--</asp:ListItem>
                <asp:ListItem Value="ChurchMove">Church Move</asp:ListItem>
                <asp:ListItem>Lighthouses</asp:ListItem>
            </asp:DropDownList>

        </td></tr>


    <tr><td class="style2">

            Email<br />

</td><td class="style3">

            Email Confirm

</td><td class="style4">

            &nbsp;</td></tr>


    <tr><td class="style2">

<asp:TextBox  ID="TextBoxEmail" runat="server" size="30" class="shadeform"></asp:TextBox></td><td class="style3">

<asp:TextBox  ID="TextBoxEmailConfirm" runat="server" size="30" class="shadeform"></asp:TextBox></td>
        <td class="style4" rowspan="5">

            <asp:Literal ID="LiteralMessage" runat="server"></asp:Literal>
        </td></tr>

    <tr><td class="style5">

            Address<br />

</td><td class="style6">

            Phone (Optional)</td></tr>


    <tr><td class="style2">

<asp:TextBox  ID="TextBoxAddress" runat="server" size="30" class="shadeform"></asp:TextBox>

</td><td class="style3">

<asp:TextBox  ID="TextBoxPhone" runat="server" size="30" class="shadeform" Width="140px"></asp:TextBox>

        </td></tr>



    <tr><td class="style2">

            City</td><td class="style3">

        State</td></tr>




    <br />



    <tr><td class="style2">

<asp:TextBox  ID="TextBoxCity" runat="server" size="30" class="shadeform"></asp:TextBox>
        
</td><td class="style3">

            <asp:DropDownList id="UserState" runat="server" class="dropdown">
<asp:ListItem Value="" Selected="true">Select State</asp:ListItem>
<asp:ListItem Value="AL">Alabama</asp:ListItem>
<asp:ListItem Value="AK">Alaska</asp:ListItem>
<asp:ListItem Value="AZ">Arizona</asp:ListItem>
<asp:ListItem Value="AR">Arkansas</asp:ListItem>
<asp:ListItem Value="CA">California</asp:ListItem>
<asp:ListItem Value="CO">Colorado</asp:ListItem>
<asp:ListItem Value="CT">Connecticut</asp:ListItem>
<asp:ListItem Value="DC">District of Columbia</asp:ListItem>
<asp:ListItem Value="DE">Delaware</asp:ListItem>
<asp:ListItem Value="FL">Florida</asp:ListItem>
<asp:ListItem Value="GA">Georgia</asp:ListItem>
<asp:ListItem Value="HI">Hawaii</asp:ListItem>
<asp:ListItem Value="ID">Idaho</asp:ListItem>
<asp:ListItem Value="IL">Illinois</asp:ListItem>
<asp:ListItem Value="IN">Indiana</asp:ListItem>
<asp:ListItem Value="IA">Iowa</asp:ListItem>
<asp:ListItem Value="KS">Kansas</asp:ListItem>
<asp:ListItem Value="KY">Kentucky</asp:ListItem>
<asp:ListItem Value="LA">Louisiana</asp:ListItem>
<asp:ListItem Value="ME">Maine</asp:ListItem>
<asp:ListItem Value="MD">Maryland</asp:ListItem>
<asp:ListItem Value="MA">Massachusetts</asp:ListItem>
<asp:ListItem Value="MI">Michigan</asp:ListItem>
<asp:ListItem Value="MN">Minnesota</asp:ListItem>
<asp:ListItem Value="MS">Mississippi</asp:ListItem>
<asp:ListItem Value="MO">Missouri</asp:ListItem>
<asp:ListItem Value="MT">Montana</asp:ListItem>
<asp:ListItem Value="NE">Nebraska</asp:ListItem>
<asp:ListItem Value="NV">Nevada</asp:ListItem>
<asp:ListItem Value="NH">New Hampshire</asp:ListItem>
<asp:ListItem Value="NJ">New Jersey</asp:ListItem>
<asp:ListItem Value="NM">New Mexico</asp:ListItem>
<asp:ListItem Value="NY">New York</asp:ListItem>
<asp:ListItem Value="NC">North Carolina</asp:ListItem>
<asp:ListItem Value="ND">North Dakota</asp:ListItem>
<asp:ListItem Value="OH">Ohio</asp:ListItem>
<asp:ListItem Value="OK">Oklahoma</asp:ListItem>
<asp:ListItem Value="OR">Oregon</asp:ListItem>
<asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
<asp:ListItem Value="RI">Rhode Island</asp:ListItem>
<asp:ListItem Value="SC">South Carolina</asp:ListItem>
<asp:ListItem Value="SD">South Dakota</asp:ListItem>
<asp:ListItem Value="TN">Tennessee</asp:ListItem>
<asp:ListItem Value="TX">Texas</asp:ListItem>
<asp:ListItem Value="UT">Utah</asp:ListItem>
<asp:ListItem Value="VT">Vermont</asp:ListItem>
<asp:ListItem Value="VA">Virginia</asp:ListItem>
<asp:ListItem Value="WA">Washington</asp:ListItem>
<asp:ListItem Value="WV">West Virginia</asp:ListItem>
<asp:ListItem Value="WI">Wisconsin</asp:ListItem>
<asp:ListItem Value="WY">Wyoming</asp:ListItem>
</asp:DropDownList>
            
            
        </td></tr>



    <tr><td class="style2">

        Zip Code</td><td class="style3">

            &nbsp;</td><td class="style4">

            &nbsp;</td></tr>



    <tr><td class="style8">

<asp:TextBox  ID="TextBoxZip" runat="server" size="30" class="shadeform" Width="109px"></asp:TextBox>
        
            
</td><td class="style9">

            </td><td class="style10" align="center">

            <a href="privacystatement.aspx" onclick="window.open(this.href,'newWindow','width=300,height=400', 'modal');return false"><b>Privacy Statement</b></a>
            
            </td></tr>



    <tr><td class="style11">

        &nbsp;</td><td class="style12">

            </td><td class="style13" align="center">

            &nbsp;<asp:Button ID="submit" runat="server" Text="sign up" class="button-order" 
                onclick="submit_Click" Visible="False" Width="248px" />
        </td></tr>



    </table>

<asp:Label ID="LabelMainAfterForm" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PictureColumn" runat="server">
<asp:Literal ID="LiteralPictures" runat="server"></asp:Literal>
</asp:Content>
