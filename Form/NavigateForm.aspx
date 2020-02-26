<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NavigateForm.aspx.cs" Inherits="Form.Form_NavigateForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>    
    <script src="../JS/AssetMan.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle">导航</div>    
        <div class="DivSeperate"></div>
        <div class="DivIndexTitle" >首页</div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/TodoListForm.aspx')">待办事项</a></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/TodayOperationsForm.aspx')">今日操作</a></div>

        <div class="DivIndexTitle">录入</div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/MoneyInOutForm.aspx?MoneyIn=1')">收入</a></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/MoneyInOutForm.aspx?MoneyIn=0')">支出</a></div>
        <div class="DivIndexItem"><asp:LinkButton ID="lbInvest" runat="server" OnClick="lbInvest_Click">投资（类固）</asp:LinkButton></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/FundForm.aspx?Purchase=1')">投资（基金）</a></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/FundForm.aspx?Purchase=0')">基金赎回</a></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/TransferForm.aspx')">转账</a></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/AddOilForm.aspx')">加油</a></div>

        <div class="DivIndexTitle">查询</div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/QueryMoneyInOutDetailForm.aspx')">收支明细</a></div>
		<div class="DivIndexItem"><a href="#" onclick="go('Form/AllInvestmentsForm.aspx')">投资明细</a></div>
		<div class="DivIndexItem"><a href="#" onclick="go('Form/CreditCardBillForm.aspx')">信用卡账单</a></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/QueryBankCardDetailForm.aspx')">银行卡明细</a></div>

        <div class="DivIndexTitle">统计</div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/MonthDetail.aspx')">月度收支</a></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/YearDetailForm.aspx')">年度收支</a></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/AddOilHistoryForm.aspx')">加油历史</a></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/AssetHistoryForm.aspx')">资产历史</a></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/AllBankCardsForm.aspx')">所有银行卡</a></div>
    
        <div class="DivIndexTitle">设置</div>
        <div class="DivIndexItem"><a href="#" onclick="ShowDayDetail(this)">显示资产详单</a></div>
        <div class="DivIndexItem"><a href="#" onclick="go('Form/TestIFrame.aspx')">测试</a></div>
    </form>
</body>
</html>
