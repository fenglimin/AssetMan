<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MonthDetail.aspx.cs" Inherits="Form.Form_MonthDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle">月度收支明细</div>
        <div class="DivSeperate"></div>
        <div id="DivTodayNavigate">
            <asp:Button ID="btPrevMonth" runat="server" Text="前一月" CssClass="Button" Width="100px" OnClick="btPrevMonth_Click"/>
            <asp:Label ID="lblMonthAsset" runat="server" Text="Today" Font-Bold="True"  Font-Size="11pt"></asp:Label>
            <asp:Button ID="btNextMonth" runat="server" Text="后一月" CssClass="Button" Width="100px" OnClick="btNextMonth_Click"/>
        </div>
        <div style="height: 5px"></div>
        <div>
            <asp:GridView ID="gvMonthDetail" runat="server" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="#333333" GridLines="Vertical" ShowHeaderWhenEmpty="True" Font-Size="9pt" Width="98%" RowStyle-Wrap="False" OnRowDataBound="gvMonthDetail_RowDataBound">
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
