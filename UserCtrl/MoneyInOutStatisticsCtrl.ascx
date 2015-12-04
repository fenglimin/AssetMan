<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MoneyInOutStatisticsCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_MoneyInOutStatisticsCtrl" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>

<div style="font-size: 13px">
    <asp:HiddenField ID="hfUrl" runat="server" />
    <table>
        <tr>
            <td><asp:Label ID="lbTitle" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label></td>
        </tr>
        <tr>
            <td>
                <div style="width:215px;display:block;float:left">
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
            </td>
            <td>
                 <div style="width:400px;display: block;float:right">
                    <asp:Chart ID="Chart1" runat="server" BackColor="Transparent" Width="400px">
                        <Series>
                            <asp:Series Name="Series1" YValuesPerPoint="6"></asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                                <Area3DStyle Enable3D="True" />
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>            
                </div>               
            </td>
        </tr>
    </table>

    

</div>

