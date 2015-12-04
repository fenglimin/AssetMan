using System;
using Business;
using DataAccess;
using Entities;

namespace Form
{
	public partial class Form_AddOilForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				ucAddOilBankCard.CardUsage = "支出";
				ucAddOilBankCard.InitCardId = SettingDal.GetIntValues("默认加油卡")[0];

				ucAmount.Title = "金额";
				ucAmount.MinimumValue = "0";
				ucAmount.MaximunValue = "1000";

				ucOilPrice.Title = "油价";
				ucOilPrice.MinimumValue = "0";
				ucOilPrice.MaximunValue = "10";

				ucOilMeteBeforeAdd.Title = "剩油";
				ucOilMeteBeforeAdd.MinimumValue = "0";
				ucOilMeteBeforeAdd.MaximunValue = "100";

				ucCurrentMileAge.Title = "里程";
				ucCurrentMileAge.MinimumValue = "0";
				ucCurrentMileAge.MaximunValue = "1000000";

				ucOilStation.Type = "加油站";
				ucOilStation.Title = "地址";
				ucOilStation.Height = 100;
				ucOilStation.InstanceName = "ucOilStation";
			}
		}

		protected void btAddOil_Click(object sender, EventArgs e)
		{
			var addOilDetail = new AddOilDetail
			{
				AddOilDate = ucAddOilDate.Date,
				AddOilAddress = ucOilStation.Text,
				CurMileAge = Convert.ToInt32(ucCurrentMileAge.Amount),
				OilFee = Convert.ToInt32(ucAmount.Amount),
				OilPrice = Convert.ToDouble(ucOilPrice.Amount),
				OilMeteBeforeAdd = Convert.ToDouble(ucOilMeteBeforeAdd.Amount)
			};

			var oilMeteAdded = Convert.ToInt32(addOilDetail.OilFee * 100 / addOilDetail.OilPrice);
			addOilDetail.OilMeteAdded = Convert.ToDouble(oilMeteAdded) / 100;
			addOilDetail.DrivedMileAge = 0;
			addOilDetail.AverageOilUsage = 0;

			AddOilDal.AddOilDetail(addOilDetail);
			ucOilStation.AddToSetting();

			var dayDetail = new DayDetail()
			{
				OperationDate = DateTime.Today.ToString("yyyy-MM-dd"),
				BankName = ucAddOilBankCard.BankName,
				CardName = ucAddOilBankCard.CardName,
				ActionType = "支出",
				Account = "-" + ucAmount.Amount,
				AvailDate = ucAddOilDate.Date,
				Avail = "Yes",
				Desc = string.Format("加油：油价{0}，地址：{1}", addOilDetail.OilPrice, addOilDetail.AddOilAddress)
			};

			AssetDetailManager.AddDayDetail(ucAddOilBankCard.CardId, dayDetail);
			lbReslt.Text = "加油已录入！";

			Page.ClientScript.RegisterStartupScript(this.GetType(), "click", "AddOil_OnOk('ucOilStation');", true);
		}
	}
}