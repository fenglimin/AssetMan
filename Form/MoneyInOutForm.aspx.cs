using System;
using System.Collections.Generic;
using Business;
using DataAccess;
using Entities;

namespace Form
{
	public partial class Form_MonyInOutForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				var moneyIn = Request.QueryString["MoneyIn"];
				lbTitle.Text = (moneyIn == "1") ? "收入" : "支出";

				ucBankCard.CardUsage = lbTitle.Text;
				ucBankCard.InitCardId = SettingDal.GetIntValues("默认" + lbTitle.Text + "卡")[0];
				ucBankCard.HideTitle = true;

				ucAmount.Title = "金额";
				ucAmount.MinimumValue = "0";
				ucAmount.MaximunValue = "10000000";

				ucDesc.Type = lbTitle.Text;
				ucDesc.Title = "描述";
				ucDesc.Height = 300;
				ucDesc.InstanceName = "ucDesc";
			}
		}

		protected void btOk_Click(object sender, EventArgs e)
		{
			var dayDetail = new DayDetail()
			{
				OperationDate = DateTime.Today.ToString("yyyy-MM-dd"),
				BankName = ucBankCard.BankName,
				CardName = ucBankCard.CardName,
				ActionType = lbTitle.Text,
				Account = lbTitle.Text == "支出" ? "-" + ucAmount.Amount : ucAmount.Amount,
				AvailDate = ucDate.Date,
				Avail = "Yes",
				Desc = ucDesc.Text
			};

			AssetDetailManager.AddDayDetail(ucBankCard.CardId, dayDetail);
			ucDesc.AddToSetting();
			lbReslt.Text = lbTitle.Text + " 已录入！";

			var funCall = string.Format("MoneyInOut_OnOk('ucDesc', '{0} {1}， {2}_{3}');", lbTitle.Text, ucAmount.Amount, dayDetail.BankName, dayDetail.CardName);
			Page.ClientScript.RegisterStartupScript(this.GetType(), "click", funCall, true);
		}
	}
}