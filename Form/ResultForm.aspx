<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResultForm.aspx.cs" Inherits="Form.Form_ResultForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/AssetMan.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DivTitle"><asp:Label ID="lbTitle" runat="server" Text=""></asp:Label></div>
        <div class="DivSeperate"></div>
        <div id="DivResult">
            <div><asp:Label ID="lbMessage" runat="server" Text="" CssClass="SubTitle"></asp:Label></div>
            <div class="DivSeperate"></div>
            <div><asp:Button ID="btGo" runat="server" Text="Button" CssClass="Button" OnClick="btGo_Click" /></div>
        </div>
    </form>
</body>
</html>
