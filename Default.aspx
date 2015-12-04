<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AssetMan</title>
    <link href="./CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="div0">
        <div id="DivTop">
            AssetMan
        </div>
        <div id="DivLeft">
            <iframe src="~/Form/NavigateForm.aspx" width="100%" height = "100%" id="urlFrameLeft" runat="server"></iframe>
        </div>
        <div id="DivCenter">
            <iframe src="~/Form/TodoListForm.aspx" width="100%"  height = "100%" id="urlFrameCenter" runat="server"></iframe>
        </div>
        <div id="DivRight">
            <iframe src="~/Form/AssetDetailForm.aspx?Reason=当日首次刷新" width="100%" height = "100%" id="urlFrameRight" runat="server"></iframe>
        </div>
    </div>   
    </form>
</body>
</html>
