using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Constants;
using DataAccess;
using UI;

namespace UserCtrl
{
	public partial class UserCtrl_DayDetailListCtrl : System.Web.UI.UserControl
	{
		public bool ShowDateColumn { get; set; }
		public string TypeCondition { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				CreateGridViewColumn();
				var today = DateTime.Today.ToString("yyyy-MM-dd");
				QueryDayDetail(today, today, TypeCondition);
			}
		}

		public int QueryDayDetail(string startDate, string endDate, string condition)
		{
			var newCondition = string.Format("OperationDate >= DATEVALUE('{0}') AND OperationDate <= DATEVALUE('{1}')",
					startDate, endDate);
			if (!string.IsNullOrEmpty(condition))
				newCondition += " AND " + condition;

			var data = AccountManager.CreateDateTableForDayOperations(newCondition);
            if (data.Rows.Count > 0)
            {
                gvDayDetail.DataSource = data;
                ViewState["Data"] = data;
                gvDayDetail.DataBind();
            }

            return data.Rows.Count;
        }

		private void CreateGridViewColumn()
		{
			GridViewManager.AddButtonFieldColumn(gvDayDetail, string.Empty, HorizontalAlign.Left);
			if (ShowDateColumn)
				GridViewManager.AddBoundFieldColumn(gvDayDetail, TableFieldName.Date, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvDayDetail, TableFieldName.BankName, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvDayDetail, TableFieldName.CardName, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvDayDetail, TableFieldName.Type, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvDayDetail, TableFieldName.Balance, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvDayDetail, TableFieldName.Description, HorizontalAlign.Left);
		}

		protected void gvDayDetail_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.DataRow) return;

			var account = Common.SafeConvertToInt(e.Row.Cells[5].Text);
			var color = account > 0 ? Color.Red : Color.Green;
			var bold = Math.Abs(account) >= 500;

			GridViewManager.SetRowStyle(e.Row, color, bold);

			var dataTable = gvDayDetail.DataSource as DataTable;

			var button = e.Row.Cells[0].Controls[0] as LinkButton;
			button.Text = "撤销";
			button.OnClientClick = "javascript:return   confirm('真的要撤销此次操作吗？');";
		}

		protected void gvDayDetail_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			var dataTable = ViewState["Data"] as DataTable;
			if (dataTable == null)
				return;

			var rowIndex = Convert.ToInt32(e.CommandArgument);
			var id = dataTable.Rows[rowIndex][TableFieldName.Id].ToString();
			var bankName = dataTable.Rows[rowIndex][TableFieldName.BankName].ToString();
			var cardName = dataTable.Rows[rowIndex][TableFieldName.CardName].ToString();
			var actionType = dataTable.Rows[rowIndex][TableFieldName.Type].ToString();
			var accountChanged = Convert.ToInt32(dataTable.Rows[rowIndex][TableFieldName.Balance].ToString());
			var dateChanged = dataTable.Rows[rowIndex][TableFieldName.Date].ToString();

			var bankCard = BankCardDal.GetCardByBankNameAndCardName(bankName, cardName);
			if (bankCard == null)
				return;

			AssetDetailManager.DeleteDayDetail(id, bankCard.CardId, -accountChanged, dateChanged);

			dataTable.Rows[rowIndex].Delete();
			dataTable.AcceptChanges();
			gvDayDetail.DataSource = dataTable;
			gvDayDetail.DataBind();

			var funCall = string.Format("RefreshAssetDetail('撤销{0} {1}， {2}_{3}');",
				actionType, Math.Abs(accountChanged), bankName, cardName);
			var jsScript = "<script type='text/javascript'>" + funCall + "</script>";
				
			ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", jsScript, false);
		}

		protected void gvDayDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			
		}
	}
}