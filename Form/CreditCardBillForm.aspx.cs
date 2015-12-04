using System;
using System.Activities.Statements;
using System.Web.Script.Services;

namespace Form
{
	public partial class Form_CreditCardBillForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				ucBillNotPublish.BillPublished = false;
				ucBillNotPublish.Title = "未出账单";
				ucBillNotPublish.IngorePaidBill = false;

				ucBillPublish.BillPublished = true;
				ucBillPublish.Title = "已出账单";
				ucBillPublish.IngorePaidBill = false;
			}
		}
	}
}