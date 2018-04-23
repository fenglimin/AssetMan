<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectTwoDatesWithFormatCtrl.ascx.cs" Inherits="UserCtrl.UserCtrl_SelectTwoDatesWithFormatCtrl" %>
<%@ Register Src="~/UserCtrl/SelectTwoDatesCtrl.ascx" TagPrefix="uc1" TagName="SelectTwoDatesCtrl" %>


<head>
    <title></title>
    <link href="../CSS/stylesheet.css" rel="stylesheet" type="text/css"/>
    
    <link href="../CSS/jquery-ui.css" rel="stylesheet" type="text/css"/>
    <script src="../JS/jquery-1.10.2.js"></script>
    <script src="../JS/jquery-ui-1.11.2.js"></script>
    <script>
        $(function () {
            $('.datepicker').datepicker(
              {
                  inline: false,
                  disabled: false,
                  dateFormat: 'yy-mm-dd',         // 设置日期格式  
                  dayNames: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
                  dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],
                  monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                  monthNamesShort: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                  changeMonth: true,              // 下拉框选择月份  
                  changeYear: true,               // 下拉框选择年份  
                  yearRange: "1950:2050",     // 下拉列表中年份范围  
                  showOtherMonths: true,          // 显示其他月份的日期  
                  selectOtherMonths: true,       // 允许选择其他月份的日期  
                  showAnim: 'drop',               // 动画效果风格  

                  minDate: new Date(1950, 1 - 1, 1),    // 本控件可以选的最小日期  
                  maxDate: new Date(2050, 12 - 1, 31),  // 本控件可以选的最大日期  

                  showMonthAfterYear: true,       // 是否在面板的头部年份后面显示月份  
                  nextText: '下个月',                // 更改按钮提示文本  
                  prevText: '上一月',                // 更改按钮提示文本  
                  closeText: '关闭',                // 更改按钮提示文本  
                  currentText: '本月',              // 更改按钮提示文本  
                  showButtonPanel: true,          // 显示按钮面板  

                  buttonText: '日历',               // 日历按钮提示文本  
                  showOn: "focus",               // 日历按钮触发 ['focus', 'button', 'both'] 三选一  
                  //buttonImage: basePath + "/images/calendar.gif", // 日历按钮  
                  buttonImageOnly: true           // 按钮不显示文字  
                  //onClose: function(selectedDate) {
                  //    alert(selectedDate);
                  //}
              }
            );

            //$('.datepicker').datepicker("setDate", new Date());
            $("#ui-datepicker-div").css('font-size', '14px'); //改变大小
        });
    </script>
</head>

<div style="font-size: 13px">
    <table>
        <tr><asp:Label ID="lbTitle" runat="server" Text="设定时间：" Font-Bold="True"></asp:Label></tr>
        <tr>
            <td><asp:Label ID="lbPeriod" runat="server" Text="范围：" Font-Bold="True"></asp:Label></td>
            <td><asp:DropDownList ID="ddlPeriodFormat" runat="server" CssClass="DataInput_DDL" AutoPostBack="True" OnSelectedIndexChanged="ddlPeriodFormat_SelectedIndexChanged" ></asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:Label ID="lbDate" runat="server" Text="日期：" Font-Bold="True"></asp:Label></td>
            <td>
                <table >
                    <tr>
                        <td><input type="text" class="datepicker" ID="startDate" runat="server" readonly="readonly" style="height:17px;width:102px" /></td>
                        <td>-</td>
                        <td><input type="text" class="datepicker" ID="endDate" runat="server" readonly="readonly" style="height:17px;width:102px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>