<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_SelectCtrl" %>

<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>

<div style="font-size: 13px">
    <table>
        <tr>
            <td><asp:Label ID="lbTitle" runat="server" Text="Label" Font-Bold="True"></asp:Label></td>
            <td><asp:DropDownList ID="ddlSelect" runat="server" CssClass="DataInput_DDL" ></asp:DropDownList></td>
        </tr>
    </table>
</div>