<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransferForm.aspx.cs" Inherits="Form.Form_TransferForm" %>

<%@ Register Src="~/UserCtrl/SelectBankCardCtrl.ascx" TagPrefix="uc1" TagName="SelectBankCardCtrl" %>
<%@ Register Src="~/UserCtrl/SelectDateCtrl.ascx" TagPrefix="uc1" TagName="SelectDateCtrl" %>
<%@ Register Src="~/UserCtrl/InputAmountCtrl.ascx" TagPrefix="uc1" TagName="InputAmountCtrl" %>
<%@ Register Src="~/UserCtrl/InputTextCtrl.ascx" TagPrefix="uc1" TagName="InputTextCtrl" %>





<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="DivTitle"><asp:Label ID="lbTitle" runat="server" Text="转账"></asp:Label></div>
    <div class="DivSeperate"></div>
    <div class="DivInput">
        <table>
            <tr>
                <td><div><uc1:SelectBankCardCtrl runat="server" ID="ucTransferOutBankCard" /></div></td>
                <td><div><uc1:SelectBankCardCtrl runat="server" ID="ucTransferInBankCard" /></div></td>
            </tr>
            <tr>
                <td><div><uc1:SelectDateCtrl runat="server" ID="ucTransferDate" /></div></td>
                <td><div><uc1:InputTextCtrl runat="server" ID="ucDesc" /></div></td>
            </tr>
            <tr>
                <td><div><uc1:InputAmountCtrl runat="server" ID="ucAmount" /></div></td>
                <td>
                    <div id="DivCommandButton">
                        <table>
                            <tr>
                                <td><asp:Button ID="btTransfer" runat="server" Text="确定" OnClick="btTransfer_Click" CssClass="Button"/></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="lbReslt" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
