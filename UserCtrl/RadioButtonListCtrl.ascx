<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RadioButtonListCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_RadioButtonListCtrl" %>

<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>

<div style="font-size: 13px">
    <table>
        <tr>
            <td><asp:Label ID="lbTitle" runat="server" Text="Label" Font-Bold="True"></asp:Label></td>
            <td><asp:Panel ID="pnlContainer" runat="server">
                <asp:RadioButtonList ID="rblOptions" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                </asp:RadioButtonList>
                    
                </asp:Panel>
            </td> 
        </tr>
    </table>
</div>