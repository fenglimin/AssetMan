<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YearDetailForm.aspx.cs" Inherits="Form.Form_YearDetailForm" %>

<%@ Register Src="~/UserCtrl/MoneyInOutStatisticsCtrl.ascx" TagPrefix="uc1" TagName="MoneyInOutStatisticsCtrl" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle"><asp:Label ID="lbTitle" runat="server" Text="年度收支"></asp:Label></div>
        <div class="DivSeperate"></div>
        <div class="DivInput"><uc1:MoneyInOutStatisticsCtrl runat="server" ID="ucAllYearStatistics" /></div>
        <div class="DivSeperate"></div>
        <div class="DivInput"><uc1:MoneyInOutStatisticsCtrl runat="server" ID="ucTheYearStatistics" /></div>

<%--            <table>
                <tr>
                    <td><uc1:MoneyInOutStatisticsCtrl runat="server" ID="ucAllYearStatistics" /></td>
                    
                </tr>
                <tr>
                    <td><uc1:MoneyInOutStatisticsCtrl runat="server" ID="ucTheYearStatistics" /></td>
                </tr>
            </table>--%>
<%--            <uc1:MoneyInOutStatisticsCtrl runat="server" ID="ucAllYearStatistics" />
            <uc1:MoneyInOutStatisticsCtrl runat="server" ID="ucTheYearStatistics" />--%>
    </form>
    

</body>

</html>
