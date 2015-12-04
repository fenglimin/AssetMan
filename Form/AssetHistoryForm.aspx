<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssetHistoryForm.aspx.cs" Inherits="Form_AssetHistoryForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle"><asp:Label ID="lbTitle" runat="server" Text="资产历史"></asp:Label></div>
        <div class="DivSeperate"></div>
        <div>
            <asp:GridView ID="gvAddOilHistory" runat="server" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="#333333" GridLines="Vertical" ShowHeaderWhenEmpty="True" Font-Size="9pt" Width="98%" RowStyle-Wrap="False" AllowPaging="True" AllowSorting="True" DataKeyNames="AssetDate" DataSourceID="SqlDataSource1" PageSize="35">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="AssetDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="日期" ReadOnly="True" SortExpression="AssetDate" />
                    <asp:BoundField DataField="AllAsset" HeaderText="可支配资产" SortExpression="AllAsset" >
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NetAsset" HeaderText="净资产" SortExpression="NetAsset" >
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="BankAccount" HeaderText="储蓄卡总余额" SortExpression="BankAccount" >
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalTaoXian" HeaderText="信用卡总欠款" SortExpression="TotalTaoXian" >
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Cash" HeaderText="现金" SortExpression="Cash" >
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ZFB" HeaderText="支付宝" SortExpression="ZFB" >
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TransferOnWay" HeaderText="转账途中" SortExpression="TransferOnWay" >
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Other" HeaderText="其他" SortExpression="Other" >
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
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
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AssetConnectionString %>" ProviderName="<%$ ConnectionStrings:AssetConnectionString.ProviderName %>" SelectCommand="SELECT [AssetDate], [AllAsset], [NetAsset], [BankAccount], [TotalTaoXian], [Cash], [ZFB], [TransferOnWay], [Other] FROM [AssetHistory] ORDER BY [AssetDate] DESC"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
