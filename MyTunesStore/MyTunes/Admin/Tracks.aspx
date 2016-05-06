<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tracks.aspx.cs" Inherits="MyTunes.Admin.Tracks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Administration</h1>
    <hr />
    <h3>Add Track:</h3>
    <table>
        <tr>
            <td><asp:Label ID="LabelAddName" runat="server">Name:</asp:Label></td>
            <td>
                <asp:TextBox ID="AddName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="* Track name required." ControlToValidate="AddName" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="LabelAddPrice" runat="server">Price:</asp:Label></td>
            <td>
                <asp:TextBox ID="AddPrice" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Text="* Price required." ControlToValidate="AddPrice" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Text="* Must be a valid price without $." ControlToValidate="AddPrice" SetFocusOnError="True" Display="Dynamic" ValidationExpression="^[0-9]*(\.)?[0-9]?[0-9]?$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <!--Bytes-->
              <tr>
            <td><asp:Label ID="LabelAddBytes" runat="server">Bytes:</asp:Label></td>
            <td>
                <asp:TextBox ID="AddBytes" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Text="* Size required." ControlToValidate="AddBytes" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Text="* Must be a valid time without a decimal." ControlToValidate="AddBytes" SetFocusOnError="True" Display="Dynamic" ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <!-- mSeconds -->
        <tr>
            <td><asp:Label ID="LabelAddTime" runat="server">Milliseconds:</asp:Label></td>
            <td>
                <asp:TextBox ID="AddTime" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Text="* Time required." ControlToValidate="AddTime" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Text="* Must be a valid time without a decimal." ControlToValidate="AddTime" SetFocusOnError="True" Display="Dynamic" ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <!-- composer -->
        <tr>
            <td><asp:Label ID="LabeAddComposer" runat="server">Composer:</asp:Label></td>
            <td>
                <asp:TextBox ID="AddComposer" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Text="* Composer name required." ControlToValidate="AddComposer" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <!-- Album -->
        <tr>
            <td><asp:Label ID="LabelAddAlbum" runat="server">Album:</asp:Label></td>
            <td>
                <asp:DropDownList ID="DropDownAddAlbum" runat="server" 
                    ItemType="MyTunes.Models.Album" 
                    SelectMethod="GetAlbums" DataTextField="Title" 
                    DataValueField="AlbumId" >
                </asp:DropDownList>
            </td>
        </tr>
        <!-- Media Type -->
       <tr>
            <td><asp:Label ID="LabelAddMedia" runat="server">Media Type:</asp:Label></td>
            <td>
                <asp:DropDownList ID="DropDownAddMedia" runat="server" 
                    ItemType="MyTunes.Models.MediaType" 
                    SelectMethod="GetMedia" DataTextField="Name" 
                    DataValueField="MediaTypeId" >
                </asp:DropDownList>
            </td>
        </tr>
        <!-- Genre -->
         <tr>
            <td><asp:Label ID="LabelAddGenre" runat="server">Genre:</asp:Label></td>
            <td>
                <asp:DropDownList ID="DropDownAddGenre" runat="server" 
                    ItemType="MyTunes.Models.Genre" 
                    SelectMethod="GetGenre" DataTextField="Name" 
                    DataValueField="GenreId" >
                </asp:DropDownList>
            </td>
        </tr>

    </table>
    <p></p>
    <p></p>
    <asp:Button ID="AddProductButton" runat="server" Text="Add Product" OnClick="AddProductButton_Click"  CausesValidation="true"/>
    <asp:Label ID="LabelAddStatus" runat="server" Text=""></asp:Label>
    <p></p>
    <h3>Remove Tracks:</h3>
    <table>
        <tr>
            <td><asp:Label ID="LabelRemove" runat="server">Track:</asp:Label></td>
            <td><asp:DropDownList ID="DropDownRemove" runat="server" ItemType="MyTunes.Models.Track" 
                    SelectMethod="GetTracks" AppendDataBoundItems="true" 
                    DataTextField="Name" DataValueField="TrackId" >
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <p></p>
    <asp:Button ID="RemoveProductButton" runat="server" Text="Remove Product" OnClick="RemoveProductButton_Click" CausesValidation="false"/>
    <asp:Label ID="LabelRemoveStatus" runat="server" Text=""></asp:Label>
</asp:Content>
