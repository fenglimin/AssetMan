<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DayDetailListCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_DayDetailListCtrl" %>

<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>

<div>
    <table>
        <%--<tr><asp:Label ID="lbTitle" runat="server" Text="账单" Font-Bold="True" CssClass="SubTitle"></asp:Label></tr>--%>
        <tr>
            <asp:GridView ID="gvDayDetail" runat="server" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="#333333" GridLines="Vertical" ShowHeaderWhenEmpty="True" Font-Size="9pt" Width="98%" RowStyle-Wrap="False" OnRowDataBound="gvDayDetail_RowDataBound" EmptyDataText="没有数据" OnRowCommand="gvDayDetail_RowCommand" OnRowDeleting="gvDayDetail_RowDeleting">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <EmptyDataRowStyle HorizontalAlign="Center" />
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
        
&nbsp;</div>