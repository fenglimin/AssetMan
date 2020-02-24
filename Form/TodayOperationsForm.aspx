<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TodayOperationsForm.aspx.cs" Inherits="Form.Form_TodayOperationsForm" %>

<%@ Register Src="~/UserCtrl/DayDetailListCtrl.ascx" TagPrefix="uc1" TagName="DayDetailListCtrl" %>
<%@ Register Src="~/UserCtrl/SelectDateCtrl.ascx" TagPrefix="uc1" TagName="SelectDateCtrl" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle">今日操作</div>
        <div class="DivSeperate"></div>
        <div id="DivTodayNavigate">
            <table style="margin: auto">
                <tr >
                    <td><asp:Button ID="btPrevDay" runat="server" Text="前一操作日" CssClass="Button" Width="100px" OnClick="btPrevDay_Click" /></td>
                    <td><uc1:SelectDateCtrl runat="server" ID="ucDate" /></td>
                    <td><asp:Button ID="btNextDay" runat="server" Text="后一操作日" CssClass="Button" Width="100px"  OnClick="btNextDay_Click" /></td>
                </tr>
            </table>
            
        </div>
        <div style="height: 5px"></div>
        <div class="DivInput">
            <uc1:DayDetailListCtrl runat="server" ID="ucDayOperations" />
        </div>
    </form>
</body>
</html>
