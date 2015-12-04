using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;
using Business;
using Constants;
using DataAccess;
using UI;

namespace Form
{
	public partial class Form_QueryBankCardDetailForm : System.Web.UI.Page
	{
		protected override void OnLoadComplete(EventArgs e)
		{
			base.OnLoadComplete(e);
			var queryNow = Request.QueryString["QueryNow"];
			if (!string.IsNullOrEmpty(queryNow) && queryNow == "1")
			{
				DoQuery();
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				ucBankCard.CardUsage = string.Empty;
				var initCardId = Request.QueryString["CardId"];
				if (!string.IsNullOrEmpty(initCardId))
					ucBankCard.InitCardId = Convert.ToInt32(initCardId);

				ucMinAmount.Title = "金额";
				ucMinAmount.Type = "最小金额_银行卡明细";
				var initMinAmount = Request.QueryString["MinAmount"];
				if (!string.IsNullOrEmpty(initCardId))
					ucMinAmount.InitSelection = ">=" + initMinAmount;

                ucType.AddAllButton = true;
                ucType.Title = "类型";
                ucType.Type = "操作类型";
                ucType.InstanceName = "ucType";
				var initTypes = Request.QueryString["Types"];
				if (!string.IsNullOrEmpty(initTypes))
					ucType.InitSelectedOptions = initTypes.Split(':');

				var startDate = Request.QueryString["StartDate"];
				if (!string.IsNullOrEmpty(startDate))
					ucQueryPeriod.InitStartDate = startDate;
				var endtDate = Request.QueryString["EndDate"];
				if (!string.IsNullOrEmpty(endtDate))
					ucQueryPeriod.InitEndDate = endtDate;

				var desc = Request.QueryString["Desc"];
				if (!string.IsNullOrEmpty(desc))
					ucDesc.InitText = desc;

				CreateGridViewColumn();
			}
		}

		private void CreateGridViewColumn()
		{
			GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.Date, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.Type, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.Balance, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.Description, HorizontalAlign.Left);
		}

		private void DoQuery()
		{
			var minAmount = Convert.ToInt32(ucMinAmount.SelectedOption.TrimStart(new char[] { '>', '=' }));
			gvResult.DataSource = AccountManager.CreateDateTableFromBankCardDetailQuery(ucBankCard.BankName, ucBankCard.CardName,
				ucQueryPeriod.StartDate, ucQueryPeriod.EndDate, ucType.SelectedOptions, ucType.UnselectedOptions, ucDesc.Text, minAmount);
			gvResult.DataBind();			
		}

		protected void btQuery_Click(object sender, EventArgs e)
		{
			DoQuery();
		}

		protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.DataRow) return;

			int account = Common.SafeConvertToInt(e.Row.Cells[2].Text);
			if (e.Row.RowIndex == 0)
			{
				e.Row.BackColor = account>0 ? Color.Red : Color.Green;
				GridViewManager.SetRowStyle(e.Row, Color.White, true);
				return;
			}


			if ( account >= 500)
				GridViewManager.SetRowStyle(e.Row, Color.Red, true);
			else if (account <= -500)
				GridViewManager.SetRowStyle(e.Row, Color.Green, true);
		}
}
}