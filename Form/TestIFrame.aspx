<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestIFrame.aspx.cs" Inherits="Form.Form_TestIFrame" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>

    </title>
    
            <script src="../JS/jquery-1.10.2.js"></script>
        <script src="../JS/jquery-ui-1.11.2.js"></script>
    <script>

        function CallStaticFunctionOfCodeBehind() {
            $.ajax({
                type: "POST",
                url: "TestIFrame.aspx/StaticFunction",
                data: "{ 'paraName': 'ParaValue' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function(data, status) {
                    alert(data.d[0].name);
                },

                failure: function(data) {
                    alert('failed');
                },
                error: function(data) {
                    alert('error');
                }
            });
        }

        function CallNonStaticFunctionOfCodeBehind() {
            // This function is injected by code behind
            CallServer('arg', 'context');
        }

        function CallbackOnSucceeded(result, context) {
            alert('succeeded - ' + result);
        }

        function CallbackOnFailed(result, context) {
            alert('failed - ' + result);
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"/>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <input id="Button1" type="button" value="Call Static Function Of Code Behind" onclick="CallStaticFunctionOfCodeBehind();"/>
                <input id="Button2" type="button" value="Call NonStatic Function Of Code Behind" onclick="CallNonStaticFunctionOfCodeBehind();"/>
                <asp:Label runat="server" Text="Label" ID="lbTest"></asp:Label>
                <asp:Button ID="Button3" runat="server" Text="Button" />
            </ContentTemplate>
            
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="lbTest"/>--%>
                <asp:PostBackTrigger ControlID="Button3"/>
            </Triggers>
        </asp:UpdatePanel>
    </form>

</body>
</html>
