using System;
using System.Drawing;
using System.Web.UI.WebControls;
using Business;
using Constants;
using DataAccess;
using UI;

namespace Form
{
	public partial class Form_MonthDetail : System.Web.UI.Page
	{
		private DateTime currentMonth;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				var month = Request.QueryString["Month"];
				if (string.IsNullOrEmpty(month))
					currentMonth = DateTime.Today;
				else
				{
					month += "-01";
					DateTime.TryParse(month, out currentMonth);
				}

				CreateGridViewColumn();
				OnCurrentMonthChanged(currentMonth);
			}
			else
			{
				currentMonth = ViewState["CurrentMonth"] is DateTime ? (DateTime)ViewState["CurrentMonth"] : DateTime.Today;
			}
		}

		private void CreateGridViewColumn()
		{
			GridViewManager.AddBoundFieldColumn(gvMonthDetail, TableFieldName.Date, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvMonthDetail, TableFieldName.BankName, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvMonthDetail, TableFieldName.CardName, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvMonthDetail, TableFieldName.Income, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvMonthDetail, TableFieldName.Outcome, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvMonthDetail, TableFieldName.Description, HorizontalAlign.Left);
		}

		private void OnCurrentMonthChanged(DateTime current)
		{
			lblMonthAsset.Text = current.ToString("yyyy年MM月");
			ViewState["CurrentMonth"] = current;
		
			gvMonthDetail.DataSource = AccountManager.CreateDateTableFromMonthDetailQuery(current, true);
			gvMonthDetail.DataBind();
		}

		protected void btPrevMonth_Click(object sender, EventArgs e)
		{
			currentMonth = currentMonth.AddMonths(-1);
			OnCurrentMonthChanged(currentMonth);
		}

		protected void btNextMonth_Click(object sender, EventArgs e)
		{
			currentMonth = currentMonth.AddMonths(1);
			OnCurrentMonthChanged(currentMonth);
		}

		protected void gvMonthDetail_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.DataRow) return;

			if (e.Row.RowIndex == 0)
			{
				var desc = e.Row.Cells[e.Row.Cells.Count - 1].Text;

				e.Row.Cells[0].ColumnSpan = 3;
				e.Row.Cells.RemoveAt(e.Row.Cells.Count - 1);
				e.Row.Cells.RemoveAt(e.Row.Cells.Count - 1);
				e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
				e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
				e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;

				e.Row.BackColor = desc == "1" ? Color.Red : Color.Green;

				GridViewManager.SetRowStyle(e.Row, Color.White, true);
				return;
			}
		

			if (Common.SafeConvertToInt(e.Row.Cells[3].Text) >= 500)
				GridViewManager.SetRowStyle(e.Row, Color.Red, true);

			if (Common.SafeConvertToInt(e.Row.Cells[4].Text) <= -500)
				GridViewManager.SetRowStyle(e.Row, Color.Green, true);
		}
	}
}