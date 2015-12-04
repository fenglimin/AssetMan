<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CheckBoxListCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_CheckBoxListCtrl" %>

<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>

<div style="font-size: 13px">
    <asp:HiddenField ID="hfType" runat="server" />
    <asp:HiddenField ID="hfAddAll" runat="server" />
    <asp:HiddenField ID="hfInstanceName" runat="server" />
    <asp:HiddenField ID="hfMustSelect" runat="server" />
    <table>
        <tr>
            <td><asp:Label ID="lbTitle" runat="server" Text="Label" Font-Bold="True"></asp:Label></td>
            <td><asp:Panel ID="pnlContainer" runat="server"></asp:Panel></td> 
        </tr>
    </table>
</div>