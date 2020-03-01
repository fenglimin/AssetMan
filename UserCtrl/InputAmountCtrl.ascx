<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputAmountCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_InputAmountCtrl" %>

<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>

<div style="font-size: 13px">
    <table>
        <tr>
            <td><asp:Label ID="lbTitle" runat="server" Text="Label" Font-Bold="True"></asp:Label></td>
            <td><asp:TextBox ID="tbAmount" runat="server" CssClass="DataInput_TB" onfocus = "this.select();"></asp:TextBox></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:RequiredFieldValidator ID="rfvAmount" runat="server" ErrorMessage="金额不能为空" ControlToValidate="tbAmount" ForeColor="Red" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator></td>   
        </tr>
        <tr>
            <td></td>
            <td><asp:RangeValidator ID="rvAmount" runat="server" ForeColor="Red" Type="Double" ControlToValidate="tbAmount" MinimumValue="0" MaximumValue="100000000" SetFocusOnError="True" Display="Dynamic"></asp:RangeValidator></td>   
        </tr>

    </table>
</div>