<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectEditCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_SelectEditCtrl" %>

<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</head>

<div style="font-size: 13px">
    <table>
        <tr>
            <td><asp:Label ID="lbTitle" runat="server" Text="Label" Font-Bold="True"></asp:Label></td>
            <td><asp:TextBox ID="tbEdit" runat="server" CssClass="DataInput_TB_BIG"></asp:TextBox></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:RequiredFieldValidator ID="rfvEdit" runat="server" ControlToValidate="tbEdit" ForeColor="Red" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:ListBox ID="lbSelect" runat="server" CssClass="DataSelect_LB"></asp:ListBox></td>
        </tr>
        <tr>
            <td class="auto-style1"></td>
            <td class="auto-style1"><asp:CheckBox ID="cbSave" runat="server" Text="保存到模板" Enabled="False" />
                <asp:Button ID="btDelete" runat="server" Text="从模板中删除" OnClick="btDelete_Click" CssClass="Button RightButton"/></td>
        </tr>
    </table>
</div>