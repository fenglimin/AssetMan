<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectBankCardCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_SelectBankCardCtrl" %>

<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>

<div style="font-size: 13px">
    <table>
        <tr><asp:Label ID="lbTitle" runat="server" Text="" Font-Bold="True"></asp:Label></tr>
        <tr>
            <td><asp:Label ID="Label1" runat="server" Text="银行：" Font-Bold="True"></asp:Label></td>
            <td><asp:DropDownList ID="ddlBankName" runat="server" AutoPostBack="True" CssClass="DataInput_DDL" OnSelectedIndexChanged="ddlBankName_SelectedIndexChanged"></asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label2" runat="server" Text="卡名：" Font-Bold="True"></asp:Label></td>
            <td><asp:DropDownList ID="ddlCardName" runat="server" CssClass="DataInput_DDL" ></asp:DropDownList></td>
        </tr>
<%--        <tr>
            <td></td>
            <td><asp:RequiredFieldValidator ID="rfvCardName" runat="server" ErrorMessage="卡名不能为空" ControlToValidate="ddlCardName" ForeColor="Red"></asp:RequiredFieldValidator></td>
        </tr>--%>
    </table>
</div>
