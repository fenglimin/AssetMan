<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllBankCardsForm.aspx.cs" Inherits="Form.Form_AllBankCardsForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle">银行卡</div>
        <div class="DivSeperate"></div>
        <div>
            <asp:GridView ID="gvAllBankCards" runat="server" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="#333333" GridLines="Vertical" ShowHeaderWhenEmpty="True" Font-Size="9pt" Width="98%" RowStyle-Wrap="False" DataKeyNames="CardID" DataSourceID="SqlDataSource1" AllowSorting="True">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:CommandField HeaderText="" ShowEditButton="True" EditText="编缉" CancelText="取消" UpdateText="更新"/>
                    <asp:BoundField DataField="CardID" HeaderText="ID" SortExpression="CardID" InsertVisible="False" ReadOnly="True" />
                    <asp:BoundField DataField="BankName" HeaderText="银行" SortExpression="BankName" />
                    <asp:BoundField DataField="CardName" HeaderText="卡名" SortExpression="CardName" />
                    <asp:BoundField DataField="CardType" HeaderText="类型" SortExpression="CardType" />
                    <asp:BoundField DataField="CardUsage" HeaderText="用途" SortExpression="CardUsage" />
                    <asp:BoundField DataField="Account" HeaderText="余额" SortExpression="Account" />
                    <asp:BoundField DataField="Credit" HeaderText="额度" SortExpression="Credit"/>
                    <asp:BoundField DataField="BillDay" HeaderText="账单日" SortExpression="BillDay" />
                    <asp:BoundField DataField="PayDay" HeaderText="还款日" SortExpression="PayDay" />
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
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:AssetConnectionString %>" DeleteCommand="DELETE FROM [BankCard] WHERE [CardID] = ? AND (([BankName] = ?) OR ([BankName] IS NULL AND ? IS NULL)) AND (([CardName] = ?) OR ([CardName] IS NULL AND ? IS NULL)) AND (([CardType] = ?) OR ([CardType] IS NULL AND ? IS NULL)) AND (([CardUsage] = ?) OR ([CardUsage] IS NULL AND ? IS NULL)) AND (([Account] = ?) OR ([Account] IS NULL AND ? IS NULL)) AND (([Credit] = ?) OR ([Credit] IS NULL AND ? IS NULL)) AND (([BillDay] = ?) OR ([BillDay] IS NULL AND ? IS NULL)) AND (([PayDay] = ?) OR ([PayDay] IS NULL AND ? IS NULL))" InsertCommand="INSERT INTO [BankCard] ([CardID], [BankName], [CardName], [CardType], [CardUsage], [Account], [Credit], [BillDay], [PayDay]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)" OldValuesParameterFormatString="original_{0}" OnUpdating="SqlDataSource1_Updating" ProviderName="<%$ ConnectionStrings:AssetConnectionString.ProviderName %>" SelectCommand="SELECT [CardID], [BankName], [CardName], [CardType], [CardUsage], [Account], [Credit], [BillDay], [PayDay] FROM [BankCard] WHERE ([CardUsage] &lt;&gt; ?) ORDER BY [CardID]" UpdateCommand="UPDATE [BankCard] SET [BankName] = ?, [CardName] = ?, [CardType] = ?, [CardUsage] = ?, [Account] = ?, [Credit] = ?, [BillDay] = ?, [PayDay] = ? WHERE [CardID] = ? AND (([BankName] = ?) OR ([BankName] IS NULL AND ? IS NULL)) AND (([CardName] = ?) OR ([CardName] IS NULL AND ? IS NULL)) AND (([CardType] = ?) OR ([CardType] IS NULL AND ? IS NULL)) AND (([CardUsage] = ?) OR ([CardUsage] IS NULL AND ? IS NULL)) AND (([Account] = ?) OR ([Account] IS NULL AND ? IS NULL)) AND (([Credit] = ?) OR ([Credit] IS NULL AND ? IS NULL)) AND (([BillDay] = ?) OR ([BillDay] IS NULL AND ? IS NULL)) AND (([PayDay] = ?) OR ([PayDay] IS NULL AND ? IS NULL))">
                <DeleteParameters>
                    <asp:Parameter Name="original_CardID" Type="Int32" />
                    <asp:Parameter Name="original_BankName" Type="String" />
                    <asp:Parameter Name="original_BankName" Type="String" />
                    <asp:Parameter Name="original_CardName" Type="String" />
                    <asp:Parameter Name="original_CardName" Type="String" />
                    <asp:Parameter Name="original_CardType" Type="String" />
                    <asp:Parameter Name="original_CardType" Type="String" />
                    <asp:Parameter Name="original_CardUsage" Type="String" />
                    <asp:Parameter Name="original_CardUsage" Type="String" />
                    <asp:Parameter Name="original_Account" Type="Int32" />
                    <asp:Parameter Name="original_Account" Type="Int32" />
                    <asp:Parameter Name="original_Credit" Type="Int32" />
                    <asp:Parameter Name="original_Credit" Type="Int32" />
                    <asp:Parameter Name="original_BillDay" Type="String" />
                    <asp:Parameter Name="original_BillDay" Type="String" />
                    <asp:Parameter Name="original_PayDay" Type="String" />
                    <asp:Parameter Name="original_PayDay" Type="String" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="CardID" Type="Int32" />
                    <asp:Parameter Name="BankName" Type="String" />
                    <asp:Parameter Name="CardName" Type="String" />
                    <asp:Parameter Name="CardType" Type="String" />
                    <asp:Parameter Name="CardUsage" Type="String" />
                    <asp:Parameter Name="Account" Type="Int32" />
                    <asp:Parameter Name="Credit" Type="Int32" />
                    <asp:Parameter Name="BillDay" Type="String" />
                    <asp:Parameter Name="PayDay" Type="String" />
                </InsertParameters>
                <SelectParameters>
                    <asp:Parameter DefaultValue="停用" Name="CardUsage2" Type="String" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="BankName" Type="String" />
                    <asp:Parameter Name="CardName" Type="String" />
                    <asp:Parameter Name="CardType" Type="String" />
                    <asp:Parameter Name="CardUsage" Type="String" />
                    <asp:Parameter Name="Account" Type="Int32" />
                    <asp:Parameter Name="Credit" Type="Int32" />
                    <asp:Parameter Name="BillDay" Type="String" />
                    <asp:Parameter Name="PayDay" Type="String" />
                    <asp:Parameter Name="original_CardID" Type="Int32" />
                    <asp:Parameter Name="original_BankName" Type="String" />
                    <asp:Parameter Name="original_BankName" Type="String" />
                    <asp:Parameter Name="original_CardName" Type="String" />
                    <asp:Parameter Name="original_CardName" Type="String" />
                    <asp:Parameter Name="original_CardType" Type="String" />
                    <asp:Parameter Name="original_CardType" Type="String" />
                    <asp:Parameter Name="original_CardUsage" Type="String" />
                    <asp:Parameter Name="original_CardUsage" Type="String" />
                    <asp:Parameter Name="original_Account" Type="Int32" />
                    <asp:Parameter Name="original_Account" Type="Int32" />
                    <asp:Parameter Name="original_Credit" Type="Int32" />
                    <asp:Parameter Name="original_Credit" Type="Int32" />
                    <asp:Parameter Name="original_BillDay" Type="String" />
                    <asp:Parameter Name="original_BillDay" Type="String" />
                    <asp:Parameter Name="original_PayDay" Type="String" />
                    <asp:Parameter Name="original_PayDay" Type="String" />
                </UpdateParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
