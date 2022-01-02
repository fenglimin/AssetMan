<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvestmentForm.aspx.cs" Inherits="Form.Form_InvestmentForm" %>

<%@ Register TagPrefix="uc1" TagName="SelectDateCtrl" Src="~/UserCtrl/SelectDateCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InputAmountCtrl" Src="~/UserCtrl/InputAmountCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InputTextCtrl" Src="~/UserCtrl/InputTextCtrl.ascx" %>
<%@ Register Src="~/UserCtrl/SelectCtrl.ascx" TagPrefix="uc1" TagName="SelectCtrl" %>
<%@ Register Src="~/UserCtrl/SelectEditCtrl.ascx" TagPrefix="uc1" TagName="SelectEditCtrl" %>
<%@ Register Src="~/UserCtrl/SelectTwoDatesCtrl.ascx" TagPrefix="uc1" TagName="SelectTwoDatesCtrl" %>





<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="DivTitle"><asp:Label ID="lbTitle" runat="server" Text="投资"></asp:Label></div>
    <div class="DivSeperate"></div>
    <div class="DivInput">
        <table>
            <tr>
                <td style="vertical-align: top">
                    <div><uc1:SelectEditCtrl runat="server" ID="ucName" /></div>
                </td>
                <td style="vertical-align: top; padding-left: 20px">
                    <div><uc1:SelectCtrl runat="server" ID="ucType" /></div>
                    <div><uc1:SelectTwoDatesCtrl runat="server" ID="ucInvestPeriod" /></div>
                    <div><uc1:InputAmountCtrl runat="server" ID="ucAmount" /></div>                    
                    <div><uc1:SelectCtrl runat="server" ID="ucMoneyInDelay" /></div>
                    <div><uc1:InputAmountCtrl runat="server" ID="ucBenifit" /></div>
                    <div><uc1:InputAmountCtrl runat="server" ID="ucBenifitRate" /></div>
                    <div><uc1:InputTextCtrl runat="server" ID="ucDesc" /></div>       
                    <div id="DivCommandButton">
                        <table>
                            <tr>
                                <td><asp:Button ID="btOk" runat="server" Text="确定" OnClick="btOk_Click" CssClass="Button"/></td>
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

