<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TodoListForm.aspx.cs" Inherits="Form.Form_TodoListForm" %>
<%@ Register TagPrefix="uc1" TagName="CreditCardBillCtrl" Src="~/UserCtrl/CreditCardBillCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InvestmentCtrl" Src="~/UserCtrl/InvestmentCtrl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle">待办事项</div>
        <div class="DivSeperate"></div>
        <div class="DivInput"><uc1:CreditCardBillCtrl runat="server" ID="ucBillPublish" /></div>
        <div class="DivSeperate"></div>
        <div class="DivInput"><uc1:InvestmentCtrl runat="server" ID="ucInvestment" /></div>
    </form>
</body>
</html>
