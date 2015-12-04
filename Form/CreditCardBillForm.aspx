<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreditCardBillForm.aspx.cs" Inherits="Form.Form_CreditCardBillForm" %>

<%@ Register Src="~/UserCtrl/CreditCardBillCtrl.ascx" TagPrefix="uc1" TagName="CreditCardBillCtrl" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle"><asp:Label ID="lbTitle" runat="server" Text="信用卡账单"></asp:Label></div>
        <div class="DivSeperate"></div>
        <div class="DivInput"><uc1:CreditCardBillCtrl runat="server" ID="ucBillPublish" /></div>
        <div class="DivSeperate"></div>
        <div class="DivInput"><uc1:CreditCardBillCtrl runat="server" ID="ucBillNotPublish" /></div>
    </form>
    

</body>
</html>