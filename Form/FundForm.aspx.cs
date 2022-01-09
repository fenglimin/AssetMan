using Business;
using DataAccess;
using Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserCtrl;

public partial class Form_FundForm : System.Web.UI.Page
{
    public string FundId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            var opType = Request.QueryString["OpType"];
            FundId = Request.QueryString["fundId"];
            ViewState["FundIdChangeNetWorth"] = FundId;

            var emptyFund = string.IsNullOrEmpty(FundId);
            var fundInfo = emptyFund ? new FundInfo() : InvestDal.LoadFundList(string.Format("WHERE FundId = {0}", FundId))[0];

            if (opType == "ChangeNetWorth")
            {
                lbTitle.Text = "净值型产品 - 更改净值";
                ucAmount.Title = "份额";
                ucAmount.InitAmount = fundInfo.TotalAmount.ToString(CultureInfo.InvariantCulture);
                ucBankCard.EnableInput = false;
                ucAmount.EnableInput = false;
                ucDesc.EnableInput = false;
            }
            else if (opType == "Purchase")
            {
                lbTitle.Text = "净值型产品 - 申购";
                ucAmount.Title = "金额";
                ucBankCard.EnableInput = emptyFund;
                ucDesc.EnableInput = emptyFund;
            }
            else if (opType == "Redemption")
            {
                lbTitle.Text = "净值型产品 - 赎回";
                ucAmount.Title = "份额";
                ucAmount.InitAmount = fundInfo.TotalShare.ToString(CultureInfo.InvariantCulture);
                ucBankCard.EnableInput = false;
                ucDesc.EnableInput = false;
            }
            else if (opType == "Bonus")
            {
                lbTitle.Text = "净值型产品 - 分红";
                ucAmount.Title = "金额";
                ucBankCard.EnableInput = false;
                ucDesc.EnableInput = false;
                ucNetWorth.EnableInput = false;
            }

            ucFundType.Title = "类型";
            ucFundType.Type = "净值型产品";
            ucFundType.EnableInput = emptyFund;
            if (!emptyFund)
            {
                ucFundType.Text = fundInfo.FundType;
            }
            else
            {
                ucFundType.FixedOption = "自动获取";
            }

            ucFundCode.Title = "代码";
            ucFundCode.InitText = emptyFund ? "自动获取" : fundInfo.FundCode;
            ucFundCode.EnableInput = emptyFund;
            ucFundCode.EnableValidate = true;

            ucBankCard.CardUsage = "收入";
            ucBankCard.InitCardId = SettingDal.GetIntValues("默认基金卡")[0];
            ucBankCard.HideTitle = true;

            ucAmount.MinimumValue = "1";
            ucAmount.MaximunValue = "10000000";

            ucNetWorth.Title = "净值";
            ucNetWorth.MinimumValue = "0.0001";
            ucNetWorth.MaximunValue = "10000000";
            ucNetWorth.InitAmount = fundInfo.CurrentNetWorth.ToString(CultureInfo.InvariantCulture);

            ucDesc.Type = "基金";
            ucDesc.Title = "名称";
            ucDesc.Height = 725;
            ucDesc.InstanceName = "ucDesc";
            ucDesc.Text = fundInfo.FundName;

            ucDate.Date = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");

            ucNextDate.Title = "开放";
            ucNextDate.Date = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
            ucNextDate.DisableInput = !emptyFund && opType != "ChangeNetWorth";

            ViewState["InitAmount"] = ucNetWorth.InitAmount;
        }
    }

    protected void btOk_Click(object sender, EventArgs e)
    {
        var timeStamp = DateTime.Today.ToString("yyyy-MM-dd");
        var desc = lbTitle.Text + "【" + ucDesc.Text + "】";

        var defaultInvestCardId = SettingDal.GetIntValues("默认投资账户")[0];
        var investCard = BankCardDal.GetAllAvailableCards()[defaultInvestCardId] as BankCard;

        var currentNetWorth = Convert.ToDouble(ucNetWorth.Amount);
        var initNetWorth = Convert.ToDouble(ViewState["InitAmount"]);
        var netWorthDelta = (currentNetWorth * 10000 - initNetWorth * 10000) / 10000;

        if (lbTitle.Text == "净值型产品 - 申购")
        {
            var ret = InvestDal.PurchaseFund(ucDesc.Text, ucFundType.Text, ucFundCode.Text, Convert.ToDouble(ucAmount.Amount), Convert.ToDouble(ucNetWorth.Amount), ucDate.Date);
            if (!ret)
            {
                lbReslt.Text = "无法自动获取类型或代码！";
                return;
            }

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
        }
        else if (lbTitle.Text == "净值型产品 - 赎回")
        {
            double totalAmount;// 赎回份额对应的本金
            double totalBenefit;// 赎回份额产生的收益
            double weightedBenefitRate; // 所有赎回本金产生的加权收益率

            // 在赎回前，先更细赎回日的净值
            InvestDal.CalculateFund(Convert.ToInt32(ViewState["FundIdChangeNetWorth"]), currentNetWorth, netWorthDelta, ucDate.Date);
            InvestDal.RedemptionFund(ucDesc.Text, Convert.ToDouble(ucAmount.Amount), currentNetWorth, ucDate.Date, out totalAmount, out totalBenefit, out weightedBenefitRate);

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
            dayDetail1.Desc = string.Format("理财：{0} {1}份，收益率{2}%", desc, ucAmount.Amount,weightedBenefitRate.ToString("f2"));
            AssetDetailManager.AddDayDetail(ucBankCard.CardId, dayDetail1);
        }
        else if (lbTitle.Text == "净值型产品 - 更改净值")
        {
            InvestDal.CalculateFund(Convert.ToInt32(ViewState["FundIdChangeNetWorth"]), currentNetWorth, netWorthDelta, ucDate.Date);
        }
        else if (lbTitle.Text == "净值型产品 - 分红")
        {
            var amount = Convert.ToInt32(ucAmount.Amount) / 10;
            InvestDal.AddFundBonus(Convert.ToInt32(ViewState["FundIdChangeNetWorth"]), amount * 10, ucDate.Date);

            var dayDetail = new DayDetail()
            {
                OperationDate = timeStamp,
                BankName = investCard.BankName,
                CardName = investCard.CardName,
                ActionType = "收入",
                Account = amount.ToString() + "0",
                AvailDate = ucDate.Date,
                Avail = "Yes",
                Desc = "理财分红 - " + ucDesc.Text
            };
            AssetDetailManager.AddDayDetail(investCard.CardId, dayDetail);
        }

        ucDesc.AddToSetting();
        lbReslt.Text = lbTitle.Text + "【" + ucDesc.Text + "】 已录入！";

        var funCall = string.Format("MoneyInOut_OnOk('ucDesc', '{0} {1}， {2}_{3}');", lbTitle.Text, ucDesc.Text, ucBankCard.BankName, ucBankCard.CardName);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "click", funCall, true);
    }
}