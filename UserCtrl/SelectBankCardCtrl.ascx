<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectBankCardCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_SelectBankCardCtrl" %>

<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    
<%--    <script>
        function aliasClicked(id) {
            var cb = document.getElementById("ucBankCard_rbCard" + id);
            var list = cb.value.split("_");
            document.getElementById("ucBankCard_ddlBankName").value = list[0];
            document.getElementById("ucBankCard_ddlCardName").value = list[1];
        }
    </script>--%>
</head>

<div style="font-size: 13px">
    <table>
        <tr><asp:Label ID="lbTitle" runat="server" Text="" Font-Bold="True"></asp:Label></tr>
        <tr>
            <td><asp:Label ID="Label1" runat="server" Text="银行：" Font-Bold="True"></asp:Label></td>
            <td><asp:DropDownList ID="ddlBankName" runat="server" AutoPostBack="True" CssClass="DataInput_DDL" OnSelectedIndexChanged="ddlBankName_SelectedIndexChanged"></asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label2" runat="server" Text="卡名：" Font-Bold="True"></asp:Label></td>
            <td><asp:DropDownList ID="ddlCardName" runat="server" CssClass="DataInput_DDL" ></asp:DropDownList></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <table>
                    <tr>
<%--                        <td><asp:RadioButton ID="rbCard1" runat="server" GroupName="Card" onclick="aliasClicked(1)"/></td>
                        <td><asp:RadioButton ID="rbCard2" runat="server" GroupName="Card" onclick="aliasClicked(2)"/></td>
                        <td><asp:RadioButton ID="rbCard3" runat="server" GroupName="Card" onclick="aliasClicked(3)"/></td>
                        <td><asp:RadioButton ID="rbCard4" runat="server" GroupName="Card" onclick="aliasClicked(4)"/></td>--%>
                        
                        <td><asp:RadioButton ID="rbCard1" runat="server" GroupName="Card" AutoPostBack="True" OnCheckedChanged="rbCard1_CheckedChanged" Visible="False" /></td>
                        <td><asp:RadioButton ID="rbCard2" runat="server" GroupName="Card" AutoPostBack="True" OnCheckedChanged="rbCard2_CheckedChanged" Visible="False" /></td>
                        <td><asp:RadioButton ID="rbCard3" runat="server" GroupName="Card" AutoPostBack="True" OnCheckedChanged="rbCard3_CheckedChanged" Visible="False" /></td>
                        <td><asp:RadioButton ID="rbCard4" runat="server" GroupName="Card" AutoPostBack="True" OnCheckedChanged="rbCard4_CheckedChanged" Visible="False" /></td>
                    </tr>
                   
                </table>
            </td>
        </tr>
<%--        <tr>
            <td></td>
            <td><asp:RequiredFieldValidator ID="rfvCardName" runat="server" ErrorMessage="卡名不能为空" ControlToValidate="ddlCardName" ForeColor="Red"></asp:RequiredFieldValidator></td>
        </tr>--%>
    </table>
</div>
