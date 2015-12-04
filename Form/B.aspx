<%@ Page Language="C#" AutoEventWireup="true" CodeFile="B.aspx.cs" Inherits="Form_B" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script>
        function go(loc) {
            self.parent.document.getElementById("Iframe1").src = "http://www.baidu.com";
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <input name="calendarSelection" type="radio" onclick="go('http://calendar.zoho.com')"/>Day
    <div>
        This is page B
    </div>
    </form>
</body>
</html>
