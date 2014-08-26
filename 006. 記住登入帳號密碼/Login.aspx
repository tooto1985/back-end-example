<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
        <div>
        帳號 : <asp:TextBox ID="AccountTextBox" runat="server"></asp:TextBox>
        <br/>
        密碼 : <asp:TextBox ID="PasswordTextBox" runat="server"></asp:TextBox>
        <br/>
        <asp:CheckBox ID="AutoLoginCheckBox" runat="server" Text="記得帳號密碼" />
        <asp:Button ID="CreateButton" runat="server" Text="登入" OnClick="CreateButton_Click" />
        <br/>
        <asp:Label ID="MessageLabel" runat="server"></asp:Label>
    </div>

</asp:Content>

