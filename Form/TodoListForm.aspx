<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TodoListForm.aspx.cs" Inherits="Form.Form_TodoListForm" %>
<%@ Register TagPrefix="uc1" TagName="CreditCardBillCtrl" Src="~/UserCtrl/CreditCardBillCtrl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InvestmentCtrl" Src="~/UserCtrl/InvestmentCtrl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle">待办事项</div>
        <div class="DivSeperate"></div>
        <div class="DivInput"><uc1:CreditCardBillCtrl runat="server" ID="ucBillPublish" /></div>
        <div class="DivSeperate"></div>
        <div class="DivInput"><uc1:InvestmentCtrl runat="server" ID="ucInvestment" /></div>
        <div class="DivSeperate"></div>
        <div class="DivInput" style="font-size: 13px">
            <div>
                <table>
                    <tr><asp:Label ID="lbTitle" runat="server" Text="更新基金净值" Font-Bold="True" CssClass="SubTitle"></asp:Label></tr>
                    <tr>
                        <asp:GridView ID="gvAllBankCards" runat="server" AutoGenerateColumns="False" HorizontalAlign = "Center" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="#333333" GridLines="Vertical" ShowHeaderWhenEmpty="True" Font-Size="9pt" Width="98%" RowStyle-Wrap="False" DataKeyNames="FundID" DataSourceID="SqlDataSource1">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:CommandField CancelText="取消" EditText="更新净值" ShowEditButton="True" UpdateText="确定" />
                                <asp:BoundField DataField="FundName" HeaderText="基金名称" SortExpression="FundName" ReadOnly="True" >
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TotalAmount" HeaderText="总投资额" SortExpression="TotalAmount" ReadOnly="True" >
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TotalShare" HeaderText="总份额" SortExpression="TotalShare" ReadOnly="True" >
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CurrentNetWorth" HeaderText="最新净值" SortExpression="CurrentNetWorth" >
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TotalBenefit" HeaderText="总收益" SortExpression="TotalBenefit" ReadOnly="True" >
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="WeightedBenefitRate" HeaderText="年化收益率" SortExpression="WeightedBenefitRate" ReadOnly="True" >
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
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AssetConnectionString %>" DeleteCommand="DELETE FROM [Fund] WHERE [FundID] = ?" InsertCommand="INSERT INTO [Fund] ([FundID], [FundName], [TotalAmount], [TotalShare], [CurrentNetWorth], [TotalBenefit], [WeightedBenefitRate]) VALUES (?, ?, ?, ?, ?, ?, ?)" OldValuesParameterFormatString="original_{0}" ProviderName="<%$ ConnectionStrings:AssetConnectionString.ProviderName %>" SelectCommand="SELECT * FROM [Fund]" UpdateCommand="UPDATE [Fund] SET [FundName] = ?, [TotalAmount] = ?, [TotalShare] = ?, [CurrentNetWorth] = ?, [TotalBenefit] = ?, [WeightedBenefitRate] = ? WHERE [FundID] = ?">
                            <DeleteParameters>
                                <asp:Parameter Name="original_FundID" Type="Int32" />
                            </DeleteParameters>
                            <InsertParameters>
                                <asp:Parameter Name="FundID" Type="Int32" />
                                <asp:Parameter Name="FundName" Type="String" />
                                <asp:Parameter Name="TotalAmount" Type="Double" />
                                <asp:Parameter Name="TotalShare" Type="Double" />
                                <asp:Parameter Name="CurrentNetWorth" Type="Double" />
                                <asp:Parameter Name="TotalBenefit" Type="Double" />
                                <asp:Parameter Name="WeightedBenefitRate" Type="String" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="FundName" Type="String" />
                                <asp:Parameter Name="TotalAmount" Type="Double" />
                                <asp:Parameter Name="TotalShare" Type="Double" />
                                <asp:Parameter Name="CurrentNetWorth" Type="Double" />
                                <asp:Parameter Name="TotalBenefit" Type="Double" />
                                <asp:Parameter Name="WeightedBenefitRate" Type="String" />
                                <asp:Parameter Name="original_FundID" Type="Int32" />
                            </UpdateParameters>
                        </asp:SqlDataSource>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
