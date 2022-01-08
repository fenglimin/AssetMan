<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputTextCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_InputTextCtrl" %>

<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>

<div style="font-size: 13px">
    <table>
        <tr>
            <td><asp:Label ID="lbTitle" runat="server" Text="描述：" Font-Bold="True"></asp:Label></td>
            <td><asp:TextBox ID="tbInput" runat="server" CssClass="DataInput_TB"></asp:TextBox></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:RequiredFieldValidator ID="rfvEdit" runat="server" ControlToValidate="tbInput" ForeColor="Red" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator></td>
        </tr>
    </table>
</div>