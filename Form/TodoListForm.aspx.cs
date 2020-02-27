using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Business;
using Constants;
using DataAccess;
using UI;

namespace Form
{
	public partial class Form_TodoListForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				var payDayPeriod = SettingDal.GetIntValues("已出账单检查期限")[0];
				ucBillPublish.BillPublished = true;
				ucBillPublish.Title = string.Format("{0}天内需还清的信用卡账单", payDayPeriod);
				ucBillPublish.IngorePaidBill = true;
				ucBillPublish.PayDayPeriod = payDayPeriod;

				var endInvestDayPeriod = SettingDal.GetIntValues("到期投资检查期限")[0];
				ucInvestment.EndDayPeriod = endInvestDayPeriod;
				ucInvestment.HideEndedInvest = true;
				ucInvestment.Title = string.Format("{0}天内到期的投资产品", endInvestDayPeriod);

                ucFund.Title = "今日基金净值";
            }
		}
	}
}