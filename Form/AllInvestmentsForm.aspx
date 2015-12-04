<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllInvestmentsForm.aspx.cs" Inherits="Form.Form_AllInvestmentsForm" %>

<%@ Register Src="~/UserCtrl/InvestmentCtrl.ascx" TagPrefix="uc1" TagName="InvestmentCtrl" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle">所有投资</div>
        <div class="DivSeperate"></div>
        <div class="DivInput"><uc1:InvestmentCtrl runat="server" ID="ucInvestment" />
        </div>
    </form>
</body>
</html>
