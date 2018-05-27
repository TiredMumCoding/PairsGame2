<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="Level1Complete.aspx.vb" Inherits="Level1Complete" %>
<%@ MasterType VirtualPath ="~/MasterPage2.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div><asp:label ID ="textbox1" runat="server" CssClass="textbox1" Text="Well Done!"></asp:label></div>
    <asp:button ID="NextLevel" runat="server" CssClass="inp button" Text="click here for next level" OnClick="next_Level" />
    <asp:button ID="restartGame" runat="server" CssClass="inp button" Text="click here to restart" Visible="false" OnClick="restart_Game" />
    <asp:label id="thisScore" runat="server" CssClass="textbox2" text="Your score for this level is " visible="false" width="700"></asp:label>
    <asp:label id="lowScore" runat="server" CssClass="textbox2" text="Your best score for this level is " visible="false" width="700"></asp:label>
</asp:Content>

