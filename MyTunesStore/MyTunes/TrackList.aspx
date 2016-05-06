<%@ Page Title="Tracks" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrackList.aspx.cs" Inherits="MyTunes.TrackList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div>
        <hgroup>
            <h1><%:Page.Title %></h1>
        </hgroup>
    </div>
     <div class="table-responsive">
    <asp:GridView ID="TracksGrid" runat="server" AutoGenerateColumns="False" ShowFooter="True" GridLines="Vertical" CellPadding="4" 
        ItemType="MyTunes.Models.Track" SelectMethod="GetTracks"
        CssClass="table table-striped table-bordered" >   
        <Columns>
            <asp:TemplateField HeaderText="Track">            
                <ItemTemplate>
                    <a href="TrackDetails.aspx?trackID=<%#: Item.TrackId %>"><%#: Item.Name %></a>
                </ItemTemplate>        
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="Track"/>
            <asp:BoundField DataField="Genre.Name" HeaderText="Genre"/>  
            <asp:BoundField DataField="Album.Title" HeaderText="Album"/>
            <asp:BoundField DataField="Album.Artist.Name" HeaderText="Artist" />
            <asp:BoundField DataField="UnitPrice" HeaderText="Price"/>
            <asp:TemplateField HeaderText="">            
                <ItemTemplate>
                    <a href="/AddToCart.aspx?productID=<%#:Item.TrackId %>">Buy</a>
                </ItemTemplate>        
            </asp:TemplateField>
        </Columns>   
    </asp:GridView>
         </div>
</asp:Content>
