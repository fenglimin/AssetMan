using Business;
using DataAccess;
using Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserCtrl;

public partial class Form_FundForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            var purchase = Request.QueryString["Purchase"];
            lbTitle.Text = (purchase == "1") ? "基金申购" : "基金赎回";

            ucBankCard.CardUsage = (purchase == "1") ? "转出" : "转入";
            ucBankCard.InitCardId = SettingDal.GetIntValues("默认" + ucBankCard.CardUsage + "卡")[0];
            ucBankCard.HideTitle = true;

            ucAmount.Title = (purchase == "1") ? "金额" : "份额";
            ucAmount.MinimumValue = "0";
            ucAmount.MaximunValue = "10000000";

            ucNetWorth.Title = "净值";
            ucNetWorth.MinimumValue = "0";
            ucNetWorth.MaximunValue = "10000000";

            ucDesc.Type = "基金";
            ucDesc.Title = ucDesc.Type;
            ucDesc.Height = 300;
            ucDesc.InstanceName = "ucDesc";
        }
    }

    protected void btOk_Click(object sender, EventArgs e)
    {
        var timeStamp = DateTime.Today.ToString("yyyy-MM-dd");
        var desc = lbTitle.Text + "【" + ucDesc.Text + "】";

        var defaultInvestCardId = SettingDal.GetIntValues("默认投资账户")[0];
        var investCard = BankCardDal.GetAllAvailableCards()[defaultInvestCardId] as BankCard;

        if (lbTitle.Text == "基金申购")
        {
            var dayDetail1 = new DayDetail()
            {
                OperationDate = timeStamp,
                BankName = ucBankCard.BankName,
                CardName = ucBankCard.CardName,
                ActionType = lbTitle.Text,
                Account = "-" + ucAmount.Amount,
                AvailDate = ucDate.Date,
                Avail = "Yes",
                Desc = desc
            };
            AssetDetailManager.AddDayDetail(ucBankCard.CardId, dayDetail1);

            
            var dayDetail2 = new DayDetail()
            {
                OperationDate = timeStamp,
                BankName = investCard.BankName,
                CardName = investCard.CardName,
                ActionType = lbTitle.Text,
                Account = ucAmount.Amount,
                AvailDate = ucDate.Date,
                Avail = "Yes",
                Desc = desc
            };
            AssetDetailManager.AddDayDetail(investCard.CardId, dayDetail2);

            InvestDal.PurchaseFund(ucDesc.Text, Convert.ToDouble(ucAmount.Amount), Convert.ToDouble(ucNetWorth.Amount), ucDate.Date);
        }
        else
        {
            double totalAmount;// 赎回份额对应的本金
            double totalBenefit;// 赎回份额产生的收益
            InvestDal.RedemptionFund(ucDesc.Text, Convert.ToDouble(ucAmount.Amount), Convert.ToDouble(ucNetWorth.Amount), ucDate.Date, out totalAmount, out totalBenefit);

            var strAmount = Math.Round(totalAmount / 10) + "0";
            var dayDetail1 = new DayDetail()
            {
                OperationDate = timeStamp,
                BankName = ucBankCard.BankName,
                CardName = ucBankCard.CardName,
                ActionType = lbTitle.Text,
                Account = strAmount,
                AvailDate = ucDate.Date,
                Avail = "Yes",
                Desc = desc + " - 本金"
            };
            AssetDetailManager.AddDayDetail(ucBankCard.CardId, dayDetail1);

            var dayDetail2 = new DayDetail()
            {
                OperationDate = timeStamp,
                BankName = investCard.BankName,
                CardName = investCard.CardName,
                ActionType = lbTitle.Text,
                Account = "-" + strAmount,
                AvailDate = ucDate.Date,
                Avail = "Yes",
                Desc = desc + " - 本金"
            };
            AssetDetailManager.AddDayDetail(investCard.CardId, dayDetail2);

            dayDetail1.Account = Math.Round(totalBenefit / 10) + "0";
            dayDetail1.ActionType = "收入";
            dayDetail1.Desc = "理财：" + desc + " - 收益";
            AssetDetailManager.AddDayDetail(ucBankCard.CardId, dayDetail1);
        }

        ucDesc.AddToSetting();
        lbReslt.Text = lbTitle.Text + "【" + ucDesc.Text + "】 已录入！";

        var funCall = string.Format("MoneyInOut_OnOk('ucDesc', '{0} {1}， {2}_{3}');", lbTitle.Text, ucAmount.Amount, ucBankCard.BankName, ucBankCard.CardName);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "click", funCall, true);
    }
}