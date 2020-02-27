using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Business;
using Constants;
using UI;

namespace Form
{
	public partial class Form_AllInvestmentsForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
            {
                ucFund.Title = "基金";
                ucFund.ForTodoList = false;
                ucFund.FundId = Request.QueryString["FundId"];
                ucFund.FundName = Request.QueryString["FundName"];

                ucInvestment.Title = "固定收益";
                ucInvestment.HideEndedInvest = true;
            }
		}
	}
}
