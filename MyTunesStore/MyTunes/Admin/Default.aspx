<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyTunes.Admin.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="list-unstyled">
        <li><a runat="server" href="~/Admin/Album">Add Album</a></li>
        <li><a runat="server" href="~/Admin/Artist">Add Artist</a></li>
        <li><a runat="server" href="~/Admin/Genre">Add Genre</a></li>
        <li><a runat="server" href="~/Admin/Category">Add Media Category</a></li>
        <li><a runat="server" href="~/Admin/MediaType">Add Media Type</a></li>
        <li><a runat="server" href="~/Admin/Tracks">Add Track</a></li>
    </ul>
</asp:Content>
