<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Album.aspx.cs" Inherits="MyTunes.Admin.Album" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h1>Administration</h1>
    <hr />
    <h3>Add Album:</h3>
    <table>
        <tr>
            <td><asp:Label ID="LabelAddName" runat="server">Title:</asp:Label></td>
            <td>
                <asp:TextBox ID="AddName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="* Track name required." ControlToValidate="AddName" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <!-- Album -->
        <tr>
            <td><asp:Label ID="LabelAddArtist" runat="server">Artist:</asp:Label></td>
            <td>
                <asp:DropDownList ID="DropDownAddArtist" runat="server" 
                    ItemType="MyTunes.Models.Artist" 
                    SelectMethod="GetArtists" DataTextField="Name" 
                    DataValueField="ArtistId" >
                </asp:DropDownList>
            </td>
        </tr>
       
    </table>
    <p></p>
    <p></p>
    <asp:Button ID="AddProductButton" runat="server" Text="Add Product" OnClick="AddProductButton_Click"  CausesValidation="true"/>
    <asp:Label ID="LabelAddStatus" runat="server" Text=""></asp:Label>
    <p></p>
        <h3>Remove Album:</h3>
    <table>
        <tr>
            <td><asp:Label ID="LabelRemove" runat="server">Album:</asp:Label></td>
            <td><asp:DropDownList ID="DropDownRemove" runat="server" ItemType="MyTunes.Models.Album" 
                    SelectMethod="GetAlbums" AppendDataBoundItems="true" 
                    DataTextField="Title" DataValueField="AlbumId" >
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <p></p>
    <asp:Button ID="RemoveProductButton" runat="server" Text="Remove Product" OnClick="RemoveProductButton_Click" CausesValidation="false"/>
    <asp:Label ID="LabelRemoveStatus" runat="server" Text=""></asp:Label>
</asp:Content>
