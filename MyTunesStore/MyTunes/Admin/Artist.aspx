﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Artist.aspx.cs" Inherits="MyTunes.Admin.Artist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            <h1>Administration</h1>
    <hr />
    <h3>Add Artist:</h3>
    <table>
        <tr>
            <td><asp:Label ID="LabelAddName" runat="server">Name:</asp:Label></td>
            <td>
                <asp:TextBox ID="AddName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="* Genre name required." ControlToValidate="AddName" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <p></p>
    <p></p>
    <asp:Button ID="AddProductButton" runat="server" Text="Add Product" OnClick="AddProductButton_Click"  CausesValidation="true"/>
    <asp:Label ID="LabelAddStatus" runat="server" Text=""></asp:Label>
    <p></p>
        <h3>Remove Artist:</h3>
    <table>
        <tr>
            <td><asp:Label ID="LabelRemove" runat="server">Artists:</asp:Label></td>
            <td><asp:DropDownList ID="DropDownRemove" runat="server" ItemType="MyTunes.Models.Artist" 
                    SelectMethod="GetArtists" AppendDataBoundItems="true" 
                    DataTextField="Name" DataValueField="ArtistId" >
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <p></p>
    <asp:Button ID="RemoveProductButton" runat="server" Text="Remove Product" OnClick="RemoveProductButton_Click" CausesValidation="false"/>
    <asp:Label ID="LabelRemoveStatus" runat="server" Text=""></asp:Label>
</asp:Content>
