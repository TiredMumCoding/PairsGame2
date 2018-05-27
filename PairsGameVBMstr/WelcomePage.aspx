<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage2.master" AutoEventWireup="false" CodeFile="WelcomePage.aspx.vb" Inherits="WelcomePage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
     
    <fieldset>
        <h3>Welcome to the Pairs Game!</h3>
        <h3>Log in:</h3>
        <div>
       <p class ="form">Username:</p> <asp:TextBox ID="uname" runat="server" CssClass="inp"/>
            </div>
        <div>
        <p class="form">Password:</p> <asp:TextBox ID="pword" runat="server" CssClass="inp" />
            </div>
        <div>
        <asp:Button ID="submit" runat="server" Text="Submit" CssClass="inp button" OnClick="Submit_button" />
        </div>
            <asp:label ID="invalid" runat="server" Text="This Username and Password do not Exist. Please Try again or Create an account" Visible="false" />
        </fieldset>

    <fieldset>
        <h3 class="form">Create Account:</h3>
        <asp:Button ID="account" runat="server" Text="Create Account" CssClass="inp button" OnClick="Create_Account" />
        </fieldset>

    <fieldset>
        <h3 class="form">Just Play:</h3>
        <asp:Button ID="Play" runat="server" Text="Just Play" CssClass="inp button" OnClick="Just_Play" />
        </fieldset>
</asp:Content>

