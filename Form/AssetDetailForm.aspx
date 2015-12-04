<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssetDetailForm.aspx.cs" Inherits="Form_AssetDetailForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle">资产详单</div>
        <div class="DivSeperate"></div>
        <div id="DivTodayNavigate">
            <asp:Button ID="btPrevDay" runat="server" Text="前一天" CssClass="SmallButton" OnClick="btPrevDay_Click" />
            <asp:Label ID="lblTodayAsset" runat="server" Text="Today" Font-Bold="True"  Font-Size="11pt"></asp:Label>
            <asp:Button ID="btNextDay" runat="server" Text="后一天" CssClass="SmallButton" OnClick="btNextDay_Click" />
        </div>
        <div style="height: 5px"></div>
        <div>
            <asp:GridView ID="gvTodayAll" runat="server" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" Font-Bold="True" GridLines="Vertical" Height="100px" Width="95%" Font-Size="9pt" ForeColor="#333333">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
            &nbsp;  
        </div>
        <div id="DivRightBottom">
            <div id="DivTextBackground"><asp:Label ID="lblBackCardBalance" runat="server" Text="银行卡" Font-Bold="True"  Font-Size="11pt"></asp:Label></div>
            <asp:GridView ID="gvTodayBankCard" runat="server" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" Font-Bold="True" GridLines="Vertical" Height="50%" Width="95%" Font-Size="9pt" OnRowDataBound="gvTodayBankCard_RowDataBound" ShowHeaderWhenEmpty="True" ForeColor="#333333">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
            &nbsp;  
        </div>
        <div id="DivRightBottom">
            <div id="DivTextBackground"><asp:Label ID="Label1" runat="server" Text="本日变动" Font-Bold="True"  Font-Size="11pt"></asp:Label></div>
            <asp:GridView ID="gvDayAction" runat="server" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" Font-Bold="True" GridLines="Vertical" Height="50%" Width="95%" Font-Size="9pt" ShowHeaderWhenEmpty="True" ForeColor="#333333" OnRowDataBound="gvDayAction_RowDataBound"  >
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
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
