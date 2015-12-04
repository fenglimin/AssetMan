<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddOilHistoryForm.aspx.cs" Inherits="Form.Form_AddOilHistoryForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle"><asp:Label ID="lbTitle" runat="server" Text="加油历史"></asp:Label></div>
        <div class="DivSeperate"></div>
        <div>
            <asp:GridView ID="gvAddOilHistory" runat="server" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="#333333" GridLines="Vertical" ShowHeaderWhenEmpty="True" Font-Size="9pt" Width="98%" RowStyle-Wrap="False" AllowPaging="True" AllowSorting="True" DataKeyNames="AddOilDate" DataSourceID="SqlDataSource1" PageSize="30" OnSorting="gvAddOilHistory_Sorting">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="AddOilDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="日期" ReadOnly="True" SortExpression="AddOilDate" />
                    <asp:BoundField DataField="CurMileAge" HeaderText="当前里程" SortExpression="CurMileAge" />
                    <asp:BoundField DataField="OilFee" HeaderText="邮费" SortExpression="OilFee" />
                    <asp:BoundField DataField="OilPrice" HeaderText="油价" SortExpression="OilPrice" />
                    <asp:BoundField DataField="OilMeteAdded" HeaderText="加油量" SortExpression="OilMeteAdded" />
                    <asp:BoundField DataField="OilMeteBeforeAdd" HeaderText="剩油量" SortExpression="OilMeteBeforeAdd" />
                    <asp:BoundField DataField="DrivedMileAge" HeaderText="行驶里程" SortExpression="DrivedMileAge" />
                    <asp:BoundField DataField="AverageOilUsage" HeaderText="油耗" SortExpression="AverageOilUsage" />
                    <asp:BoundField DataField="AddOilAddress" HeaderText="地址" SortExpression="AddOilAddress" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <EmptyDataTemplate>
                    <a href="javascript:__doPostBack('gvAddOilHistory','Sort$AddOilDate')" style="color:White;">日期</a>
                </EmptyDataTemplate>
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
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AssetConnectionString %>" ProviderName="<%$ ConnectionStrings:AssetConnectionString.ProviderName %>" SelectCommand="SELECT * FROM [AddOil] ORDER BY [AddOilDate] DESC"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
