using System;
using DataAccess;

namespace Form
{
	public partial class Form_NavigateForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
		protected void lbInvest_Click(object sender, EventArgs e)
		{
			string buttonAction;
			if (InvestDal.GetInvestBalance() > 0)
			{
				buttonAction = "go('Form/InvestmentForm.aspx?Action=1&InvestId=-1')";
			}
			else
			{
				var defaultInvestId = BankCardDal.GetDefaultInvestBankCard().CardId;
				buttonAction = "go('Form/ResultForm.aspx?Title=投资&Message=当前投资账户内可用余额为零！&ButtonText=转账到投资账户&ButtonUrl=Form/TransferForm.aspx?CardInId=" + defaultInvestId + "')";
			}

			Page.ClientScript.RegisterStartupScript(this.GetType(), "click", buttonAction, true);
		}
	}
}