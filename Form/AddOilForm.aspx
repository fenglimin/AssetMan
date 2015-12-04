<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddOilForm.aspx.cs" Inherits="Form.Form_AddOilForm" %>
<%@ Register TagPrefix="uc1" TagName="SelectBankCardCtrl" Src="~/UserCtrl/SelectBankCardCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SelectDateCtrl" Src="~/UserCtrl/SelectDateCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InputAmountCtrl" Src="~/UserCtrl/InputAmountCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InputTextCtrl" Src="~/UserCtrl/InputTextCtrl.ascx" %>
<%@ Register Src="~/UserCtrl/SelectEditCtrl.ascx" TagPrefix="uc1" TagName="SelectEditCtrl" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="DivTitle"><asp:Label ID="lbTitle" runat="server" Text="加油"></asp:Label></div>
    <div class="DivSeperate"></div>
    <div class="DivInput">
        <div><uc1:SelectBankCardCtrl runat="server" ID="ucAddOilBankCard" /></div>
        <div class="DivSeperate"></div>
        <div><uc1:SelectDateCtrl runat="server" ID="ucAddOilDate" /></div>    
        <div><uc1:InputAmountCtrl runat="server" ID="ucAmount" /></div>
        <div><uc1:InputAmountCtrl runat="server" ID="ucOilPrice" /></div>
        <div><uc1:InputAmountCtrl runat="server" ID="ucOilMeteBeforeAdd" /></div>
        <div><uc1:InputAmountCtrl runat="server" ID="ucCurrentMileAge" /></div>
        <div><uc1:SelectEditCtrl runat="server" ID="ucOilStation" /></div>
        <div><uc1:InputTextCtrl runat="server" ID="ucDesc" /></div>  
        <div id="DivCommandButton">
            <table>
                <tr>
                    <td><asp:Button ID="btAddOil" runat="server" Text="加油" CssClass="Button" OnClick="btAddOil_Click"/></td>
                </tr>
                <tr>
                    <td><asp:Label ID="lbReslt" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
