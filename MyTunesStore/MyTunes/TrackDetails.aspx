<%@ Page Title="Track" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrackDetails.aspx.cs" Inherits="MyTunes.TrackDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <hgroup>
            <h1><%:Page.Title %> Details</h1>
        </hgroup>
        <br />
    </div>
    <asp:FormView ID="trackDetail" runat="server" ItemType="MyTunes.Models.Track" SelectMethod ="GetTrack" RenderOuterTable="false">        
        <ItemTemplate>
            <ul class="list-unstyled">
                <li><b>Track: </b><%#:Item.Name %></li>
                <li><b>Artist: </b><%#: Item.Album.Artist.Name %></li>
                <li><b>Album: </b><%#: Item.Album.Title %></li>
                <li><b>Composer: </b><%#: Item.Composer %></li>
                <li><b>Genre: </b><%#: Item.Genre.Name %></li>
                <li><b>Media: </b><%#: Item.MediaType.Name %></li>
                <li><b>Durartion: </b><%#: Item.Milliseconds/1000 %> seconds</li>
                <li><b>Price: </b><%#: String.Format("{0:c}", Item.UnitPrice) %></li>
                <li>
                    <a href="/AddToCart.aspx?productID=<%#:Item.TrackId %>">               
                        <span>
                            <b>Add To Cart<b>
                        </span>           
                    </a>
                </li>
            </ul>
        </ItemTemplate>
    </asp:FormView>
</asp:Content>
