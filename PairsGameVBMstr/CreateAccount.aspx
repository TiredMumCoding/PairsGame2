<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="CreateAccount.aspx.vb" Inherits="CreateAccount" %>
<%@ MasterType VirtualPath ="~/MasterPage2.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:Label ID="label1" runat="server" Text="This username exists already.  Please create another" Visible="false" />

    <fieldset>
        <h3>To create an account, make up a username and password click submit</h3>
        
          <div> <p class="form">Username:</p> <asp:TextBox ID="uname" runat="server" CssClass ="inp" Text="" /></div>
        <div><p class="form">Password:</p> <asp:TextBox ID="pword" runat="server" CssClass ="inp" Text="" /></div>
       <div><asp:Button ID="submit" runat="server" CssClass="inp button" Text="submit" OnClick="Submit_button" /></div> 
        </fieldset>

       <div> <asp:Label ID="notvalid" runat="server" Text="this username already exists. Please try again" Visible ="false" /></div>
</asp:Content>

