﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FundCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_FundCtrl" %>
<%@ Register TagPrefix="uc1" TagName="CheckBoxListCtrl" Src="~/UserCtrl/CheckBoxListCtrl.ascx" %>

<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>

<div style="font-size: 13px">
    <div>
        <table>
            <tr>
                <td><asp:Label ID="lbTitle" runat="server" Text="基金" Font-Bold="True" CssClass="SubTitle"></asp:Label></td>
                <td><asp:CheckBox ID="cbShowHistory" runat="server" Text="显示历史纪录" AutoPostBack="True" CssClass="SubTitle" OnCheckedChanged="cbShowHistory_CheckedChanged"/></td>
                <td><asp:Button ID="btRefresh" runat="server" Text="刷新净值" CssClass="Button" OnClick="btRefresh_Click" Width="128px"/>
                <td><uc1:CheckBoxListCtrl ID="cblFundType" runat="server"></uc1:CheckBoxListCtrl></td>
                <td><asp:Button ID="btQuery" runat="server" Text="查询" CssClass="Button" Width="128px" OnClick="btQuery_Click"/>
            </tr>
            <tr>
                <asp:GridView ID="gvAllFunds" runat="server" OnSorting="gridView_Sorting" AllowSorting="True" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="#333333" GridLines="Vertical" ShowHeaderWhenEmpty="True" Font-Size="9pt" Width="98%" RowStyle-Wrap="False" OnRowDataBound="gvAllFunds_RowDataBound">
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
            </tr>
            <tr><br /></tr>
            <asp:Label ID="lbFundDetail" runat="server" Text="" Font-Bold="True" CssClass="SubTitle"></asp:Label>
            <tr>
                <asp:GridView ID="gvFundDetail" runat="server" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="#333333" GridLines="Vertical" ShowHeaderWhenEmpty="True" Font-Size="9pt" Width="98%" RowStyle-Wrap="False">
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
            </tr>
        </table>
        
        
    </div>
</div>