using System;
using Business;
using DataAccess;
using Entities;
using UserCtrl;

namespace Form
{
	public partial class Form_TransferForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				ucTransferOutBankCard.CardUsage = "转出";
				ucTransferOutBankCard.InitCardId = Common.SafeConvertToInt(Request.QueryString["CardOutId"]);

				ucTransferInBankCard.CardUsage = "转入";
				ucTransferInBankCard.InitCardId = Common.SafeConvertToInt(Request.QueryString["CardInId"]);

				ucAmount.Title = "金额";
				ucAmount.InitAmount = Request.QueryString["Amount"];
				ucAmount.MinimumValue = "0";
				ucAmount.MaximunValue = "1000000";
			}
		}

		protected void btTransfer_Click(object sender, EventArgs e)
		{
			ProcessTransfer(ucTransferOutBankCard, "-", "转至 " + ucTransferInBankCard.BankName + ":" + ucTransferInBankCard.CardName);
			ProcessTransfer(ucTransferInBankCard, string.Empty, "转自 " + ucTransferOutBankCard.BankName + ":" + ucTransferOutBankCard.CardName);
			lbReslt.Text = "转账成功！";

			var funCall = string.Format("Transfer_OnOk('转账 {0}， 从 {1}_{2} 到 {3}_{4}');", ucAmount.Amount, ucTransferOutBankCard.BankName,
				ucTransferOutBankCard.CardName, ucTransferInBankCard.BankName, ucTransferInBankCard.CardName);
			Page.ClientScript.RegisterStartupScript(this.GetType(), "click", funCall, true);
		}

		private bool ProcessTransfer(UserCtrl_SelectBankCardCtrl bankCardCtrl, string sign, string desc)
		{
			var dayDetail = new DayDetail()
			{
				OperationDate = DateTime.Today.ToString("yyyy-MM-dd"),
				BankName = bankCardCtrl.BankName,
				CardName = bankCardCtrl.CardName,
				ActionType = bankCardCtrl.CardUsage,
				Account = sign + ucAmount.Amount,
				AvailDate = ucTransferDate.Date,
				Avail = "Yes",
				Desc = string.IsNullOrEmpty(ucDesc.Text)? desc : desc + " - " + ucDesc.Text
			};

			AssetDetailManager.AddDayDetail(bankCardCtrl.CardId, dayDetail);
			return true;
		}
	}
}