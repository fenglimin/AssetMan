using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.InteropServices;
using Business;
using DataAccess;
using Entities;

namespace Form
{
	public partial class Form_InvestmentForm : System.Web.UI.Page
	{
		private string actionType;
		private string investId;

		protected void Page_Load(object sender, EventArgs e)
		{
			actionType = Request.QueryString["Action"];
			investId = Request.QueryString["InvestId"];

			if (!Page.IsPostBack)
			{
				ucType.Title = "类型";
				ucType.Type = "投资类型";
				ucType.AutoPostBack = true;

				ucName.Title = "名称";
				ucName.Type = SettingDal.GetStringValues("默认投资类型")[0];
				ucName.Height = 725;
				ucName.InstanceName = "ucName";

				ucAmount.Title = "金额";
				ucAmount.MinimumValue = "1";

				ucInvestPeriod.StartTitle = "起始";
				ucInvestPeriod.EndTitle = "终止";

				ucMoneyInDelay.Title = "到账";
				ucMoneyInDelay.Type = "到账延期";
				ucMoneyInDelay.AutoPostBack = false;
				ucMoneyInDelay.Text = InvestmentManager.GetDelayString(1);

				ucBenifit.Title = "收益";
				ucBenifit.MinimumValue = "10";
				ucBenifit.MaximunValue = "100000";

				ucBenifitRate.Title = "利率";
				ucBenifitRate.MinimumValue = "0";
				ucBenifitRate.MaximunValue = "100";

				ApplyActionType();
			}
			else
			{
				ucName.Type = ucType.Text;
			}
		}

		private void ApplyActionType()
		{
			var investBalance = InvestDal.GetInvestBalance();

			// 1 : new invest, 2 : update invest, 3 : invest expired
			if (actionType == "1")
			{
				lbTitle.Text = "新增投资";
				ucBenifit.EnableInput = false;
				ucBenifit.EnableValidate = false;
                ucType.EnableInput = true;

				ucAmount.MaximunValue = investBalance.ToString(CultureInfo.InvariantCulture);
				btOk.Text = "增加";
			}
			else if (actionType == "2")
			{
				lbTitle.Text = "更改投资";
				ucBenifit.EnableInput = false;
				ucBenifit.EnableValidate = false;

				var investDetail = LoadInvestment();
				ucAmount.MaximunValue = (investBalance+investDetail.InvestAmount).ToString(CultureInfo.InvariantCulture);
				btOk.Text = "更改";
			}
			else if (actionType == "3")
			{
				lbTitle.Text = "投资到期";

				var investDetail = LoadInvestment();
				ucAmount.MaximunValue = (investBalance + investDetail.InvestAmount).ToString(CultureInfo.InvariantCulture);
				btOk.Text = "到期";
			}

			lbTitle.Text += " - 投资账户余额：" + investBalance.ToString(CultureInfo.InvariantCulture);
		}

		private InvestDetail LoadInvestment()
		{
			var investDetail = InvestDal.LoadInvestDetailById(investId);

			ucType.Text = investDetail.InvestType;
			ucName.Type = investDetail.InvestType;
			ucName.InitText = investDetail.InvestName;
			ucAmount.InitAmount = investDetail.InvestAmount.ToString(CultureInfo.InvariantCulture);
			ucInvestPeriod.StartDate = investDetail.InvestStartDate;
			ucInvestPeriod.EndDate = investDetail.InvestEndDate;
			ucMoneyInDelay.Text = InvestmentManager.GetDelayString(investDetail.InvestEndDate, investDetail.InvestAvailDate);
			ucBenifitRate.InitAmount = investDetail.InvestBenifitRate.TrimEnd('%');

			return investDetail;
		}

		protected void btOk_Click(object sender, EventArgs e)
		{
			var investDetail = CreateInvestDetail();

			if (actionType == "1")
			{
				InvestDal.InsertInvestDetail(investDetail);
			}
			else if (actionType == "2")
			{
				investDetail.InvestID = Common.SafeConvertToInt(investId);
				InvestDal.UpdateInvestDetail(investDetail);
			}
			else if (actionType == "3")
			{
				investDetail.InvestID = Common.SafeConvertToInt(investId);
				investDetail.InvestBenifitRate = InvestmentManager.CalculateBenifitRate(investDetail);
				InvestDal.UpdateInvestDetail(investDetail);

				var bankCard = BankCardDal.GetDefaultInvestBankCard();
				var dayDetail = CreateDayDetail(bankCard, investDetail.InvestBenifitRate);

				AssetDetailManager.AddDayDetail(bankCard.CardId, dayDetail);
			}
			else
			{
				return;
			}

			lbReslt.Text = "投资已" + btOk.Text + "！";
			var funCall = string.Format("RefreshAssetDetail('投资{0}，{1}');", btOk.Text, investDetail.InvestName);
			Page.ClientScript.RegisterStartupScript(this.GetType(), "click", funCall, true);
		}

		private InvestDetail CreateInvestDetail()
		{
			var investDetail = new InvestDetail
			{
				InvestType = ucType.Text,
				InvestName = ucName.Text,
				InvestAmount = Common.SafeConvertToInt(ucAmount.Amount),
				InvestStartDate = ucInvestPeriod.StartDate,
				InvestEndDate = ucInvestPeriod.EndDate,
				InvestAvailDate = InvestmentManager.GetActualDay(ucInvestPeriod.EndDate, ucMoneyInDelay.Text),
				InvestBenifit = Common.SafeConvertToInt(ucBenifit.Amount),
				InvestBenifitRate = ucBenifitRate.Amount
			};

			return investDetail;
		}

		private DayDetail CreateDayDetail(BankCard bankCard, string benifitRate)
		{
			var actualDate = InvestmentManager.GetActualDay(ucInvestPeriod.EndDate, ucMoneyInDelay.Text);
			var dayDetail = new DayDetail()
			{
				OperationDate = DateTime.Today.ToString("yyyy-MM-dd"),
				BankName = bankCard.BankName,
				CardName = bankCard.CardName,
				ActionType = "收入",
				Account = ucBenifit.Amount,
				AvailDate = actualDate,
				Avail = "Yes",
				Desc = ucType.Text + "：" + ucName.Text + "，期限：" + ucInvestPeriod.StartDate + "到" + ucInvestPeriod.EndDate + "，收益率" + benifitRate + "%"
			};

			return dayDetail;
		}
	}
}