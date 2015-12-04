using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Business;
using Constants;
using UI;

namespace UserCtrl
{
	public partial class UserCtrl_CreditCardBillCtrl : System.Web.UI.UserControl
	{
		public string Title { get; set; }
		public bool BillPublished { get; set; }
		public bool IngorePaidBill { get; set; }
		public int PayDayPeriod { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				lbTitle.Text = Title;

				if (BillPublished)
				{
					CreateGridViewColumn_BillPublish();
					var dtBill = CreditCardManager.CreateDateTable_BillPublish(IngorePaidBill);
					gvCreditCardsBill.DataSource = RemoveBillByPayDayPeriod(dtBill, PayDayPeriod);
					gvCreditCardsBill.DataBind();
				}
				else
				{
					CreateGridViewColumn_BillNotPublish();
					gvCreditCardsBill.DataSource = CreditCardManager.CreateDateTable_BillNotPublish();
					gvCreditCardsBill.DataBind();
				}
			}
		}

		private DataTable RemoveBillByPayDayPeriod(DataTable dtBill, int payDayPeriod)
		{
			if (payDayPeriod >= 0) return dtBill;

			var count = dtBill.Rows.Count;
			var today = DateTime.Today;
			for (var i = 0; i < count; i ++)
			{
				DateTime payDay;
				DateTime.TryParse(dtBill.Rows[i][TableFieldName.BillPayDay].ToString(), out payDay);
				if (today.AddDays(payDayPeriod) < payDay)
				{
					dtBill.Rows.RemoveAt(i);
					count = dtBill.Rows.Count;
					i--;
				}
			}

			return dtBill;
		}

		private void CreateGridViewColumn_BillPublish()
		{
			GridViewManager.AddHyperLinkFieldColumn(gvCreditCardsBill, string.Empty, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvCreditCardsBill, TableFieldName.BankName, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvCreditCardsBill, TableFieldName.CardName, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvCreditCardsBill, TableFieldName.BillPeriod, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvCreditCardsBill, TableFieldName.BillPayDay, HorizontalAlign.Left);
			GridViewManager.AddHyperLinkFieldColumn(gvCreditCardsBill, TableFieldName.BillAccount, HorizontalAlign.Right);
			GridViewManager.AddHyperLinkFieldColumn(gvCreditCardsBill, TableFieldName.BillPaid, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvCreditCardsBill, TableFieldName.BillNotPaid, HorizontalAlign.Right);
		}

		private void CreateGridViewColumn_BillNotPublish()
		{
			GridViewManager.AddBoundFieldColumn(gvCreditCardsBill, TableFieldName.BankName, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvCreditCardsBill, TableFieldName.CardName, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvCreditCardsBill, TableFieldName.BillPeriod, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvCreditCardsBill, TableFieldName.BillPayDay, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvCreditCardsBill, TableFieldName.CreditAll, HorizontalAlign.Right);
			GridViewManager.AddHyperLinkFieldColumn(gvCreditCardsBill, TableFieldName.BillAccount, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvCreditCardsBill, TableFieldName.CreditAvailable, HorizontalAlign.Right);
		}

		protected void gvCreditCardsBill_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowIndex < 0) return;

			var dataTable = gvCreditCardsBill.DataSource as DataTable;
			if (dataTable == null)
				return;
			//dataTable = dataTable.DefaultView.ToTable();
			var cardId = dataTable.Rows[e.Row.RowIndex][TableFieldName.Id].ToString();

			var hyperLink = e.Row.Cells[5].Controls[0] as HyperLink;
			if (hyperLink == null)
				return;

			if (BillPublished)
			{
				var period = dataTable.Rows[e.Row.RowIndex][TableFieldName.BillPeriod].ToString().Split('~');
				hyperLink.NavigateUrl = string.Format("~/Form/QueryBankCardDetailForm.aspx?CardId={0}&&Types=支出:转出&StartDate={1}&EndDate={2}&QueryNow=1",
					cardId, period[0].Trim(' '), period[1].Trim(' '));

				if (dataTable.Rows[e.Row.RowIndex][TableFieldName.BillPaid].ToString() != "0")
				{
					hyperLink = e.Row.Cells[6].Controls[0] as HyperLink;
					if (hyperLink == null)
						return;
					hyperLink.NavigateUrl = string.Format("~/Form/QueryBankCardDetailForm.aspx?CardId={0}&&Types=收入:转入&StartDate={1}&EndDate={2}&QueryNow=1",
						cardId, period[0].Trim(' '), DateTime.Today.ToString("yyyy-MM-dd"));
				}

				if (e.Row.Cells[7].Text != "已还清")
				{
					var amount = dataTable.Rows[e.Row.RowIndex][TableFieldName.BillNotPaid].ToString();

					hyperLink = e.Row.Cells[0].Controls[0] as HyperLink;
					if (hyperLink == null)
						return;
					hyperLink.Text = "还款";
					hyperLink.NavigateUrl = string.Format("~/Form/TransferForm.aspx?CardInId={0}&Amount={1}", cardId, amount);


					DateTime payDay;
					DateTime.TryParse(dataTable.Rows[e.Row.RowIndex][TableFieldName.BillPayDay].ToString(), out payDay);
					var daySpan = payDay - DateTime.Today;
					if (daySpan.Days < 3)
						GridViewManager.SetRowStyle(e.Row, Color.Red, true);
					else if (daySpan.Days < 7)
						GridViewManager.SetRowStyle(e.Row, Color.Red, false);
				}
			}
			else
			{
				if (dataTable.Rows[e.Row.RowIndex][TableFieldName.BillAccount].ToString() != "0")
				{
					var period = dataTable.Rows[e.Row.RowIndex][TableFieldName.BillPeriod].ToString().Split('~');
					hyperLink.NavigateUrl = string.Format("~/Form/QueryBankCardDetailForm.aspx?CardId={0}&&Types=支出:转出&StartDate={1}&EndDate={2}&QueryNow=1",
						cardId, period[0].Trim(' '), DateTime.Today.ToString("yyyy-MM-dd"));					
				}
			}
		}
	}
}