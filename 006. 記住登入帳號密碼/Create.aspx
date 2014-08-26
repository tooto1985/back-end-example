<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Create.aspx.cs" Inherits="Create" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div>
        帳號 : <asp:TextBox ID="AccountTextBox" runat="server"></asp:TextBox>
        <br/>
        密碼 : <asp:TextBox ID="PasswordTextBox" runat="server"></asp:TextBox>
        <br/>
        <asp:Button ID="CreateButton" runat="server" Text="建立帳號" OnClick="Create_Click" />
        <br/>
        <asp:Label ID="MessageLabel" runat="server"></asp:Label>
    </div>
</asp:Content>

