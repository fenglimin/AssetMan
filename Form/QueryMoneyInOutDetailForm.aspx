﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryMoneyInOutDetailForm.aspx.cs" Inherits="Form.Form_QueryMoneyInOutDetailForm" %>
<%@ Register TagPrefix="uc1" TagName="SelectBankCardCtrl" Src="~/UserCtrl/SelectBankCardCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SelectTwoDatesWithFormatCtrl" Src="~/UserCtrl/SelectTwoDatesWithFormatCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InputTextCtrl" Src="~/UserCtrl/InputTextCtrl.ascx" %>
<%@ Register Src="~/UserCtrl/CheckBoxListCtrl.ascx" TagPrefix="uc1" TagName="CheckBoxListCtrl" %>
<%@ Register Src="~/UserCtrl/RadioButtonListCtrl.ascx" TagPrefix="uc1" TagName="RadioButtonListCtrl" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle"><asp:Label ID="lbTitle" runat="server" Text="收支明细查询"></asp:Label></div>
        <div class="DivSeperate"></div>
        <div class="DivInput">
            <table>
                <tr>
                    <td>
                        <uc1:CheckBoxListCtrl runat="server" ID="ucType" />
                        <uc1:InputTextCtrl runat="server" ID="ucDesc" />
                    </td>
                    <td style="padding-left: 34px">
                        <uc1:SelectTwoDatesWithFormatCtrl runat="server" ID="ucQueryPeriod" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td><uc1:RadioButtonListCtrl runat="server" ID="ucMinAmount" /></td>
                    <td><asp:Button ID="btQuery" runat="server" Text="查询" CssClass="Button" Width="190px" OnClick="btQuery_Click" /></td>
                </tr>
            </table>
        </div>
        <div class="DivResult">
            <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="#333333" GridLines="Vertical" ShowHeaderWhenEmpty="True" Font-Size="9pt" Width="98%" RowStyle-Wrap="False" OnRowDataBound="gvResult_RowDataBound">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
        </div>
    </form>
    

</body>

</html>

