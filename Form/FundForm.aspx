<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FundForm.aspx.cs" Inherits="Form_FundForm" %>
<%@ Register TagPrefix="uc1" TagName="InputAmountCtrl" Src="~/UserCtrl/InputAmountCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SelectDateCtrl" Src="~/UserCtrl/SelectDateCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SelectBankCardCtrl" Src="~/UserCtrl/SelectBankCardCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SelectEditCtrl" Src="~/UserCtrl/SelectEditCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SelectCtrl" Src="~/UserCtrl/SelectCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InputTextCtrl" Src="~/UserCtrl/InputTextCtrl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>
<body>
    <form id="form1" runat="server" DefaultButton="btOk">
        <div id="DivTitle"><asp:Label ID="lbTitle" runat="server" Text=""></asp:Label></div>
        <div class="DivSeperate"></div>
        <div class="DivInput">
            <table>
                <tr>
                    <td style="vertical-align: top">
                        <div><uc1:SelectEditCtrl runat="server" ID="ucDesc" /></div>
                    </td>
                    <td style="vertical-align: top; padding-left: 20px">
                        <div><uc1:SelectCtrl runat="server" ID="ucFundType" /></div>
                        <div><uc1:InputTextCtrl runat="server" ID="ucFundCode" /></div>
                        <div><uc1:SelectBankCardCtrl runat="server" ID="ucBankCard" /></div>
                        <div><uc1:SelectDateCtrl runat="server" ID="ucDate" /></div>    
                        <div><uc1:SelectDateCtrl runat="server" ID="ucNextDate" /></div>    
                        <div><uc1:InputAmountCtrl runat="server" ID="ucAmount" /></div>  
                        <div><uc1:InputAmountCtrl runat="server" ID="ucNetWorth" /></div>
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
