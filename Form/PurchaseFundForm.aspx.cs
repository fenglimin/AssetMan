using Business;
using DataAccess;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserCtrl;

public partial class Form_PurchaseFundForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            lbTitle.Text = "基金申购";

            ucBankCard.CardUsage = "转出";
            ucBankCard.InitCardId = SettingDal.GetIntValues("默认转出卡")[0];
            ucBankCard.HideTitle = true;

            ucAmount.Title = "金额";
            ucAmount.MinimumValue = "0";
            ucAmount.MaximunValue = "10000000";

            ucNetWorth.Title = "净值";
            ucNetWorth.MinimumValue = "0";
            ucNetWorth.MaximunValue = "10000000";

            ucDesc.Type = lbTitle.Text;
            ucDesc.Title = "基金";
            ucDesc.Height = 300;
            ucDesc.InstanceName = "ucDesc";
        }
    }

    protected void btOk_Click(object sender, EventArgs e)
    {
        var timeStamp = DateTime.Today.ToString("yyyy-MM-dd");
        var desc = "申购基金【" + ucDesc.Text + "】";

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


        var defaultInvestCardId = SettingDal.GetIntValues("默认投资账户")[0];
        var investCard = BankCardDal.GetAllAvailableCards()[defaultInvestCardId] as BankCard;
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

        InvestDal.PurchaseFund(ucDesc.Text, Convert.ToInt32(ucAmount.Amount), Convert.ToDouble(ucNetWorth.Amount), ucDate.Date);
        ucDesc.AddToSetting();
        lbReslt.Text = lbTitle.Text + "【" + ucDesc.Text + "】 已录入！";

        var funCall = string.Format("MoneyInOut_OnOk('ucDesc', '{0} {1}， {2}_{3}');", lbTitle.Text, ucAmount.Amount, dayDetail1.BankName, dayDetail1.CardName);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "click", funCall, true);
    }
}