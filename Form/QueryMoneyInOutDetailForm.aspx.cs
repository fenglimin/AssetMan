using System;
using System.Drawing;
using System.Web.UI.WebControls;
using Business;
using Constants;
using DataAccess;
using UI;

namespace Form
{
    public partial class Form_QueryMoneyInOutDetailForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
	            ucQueryPeriod.HideTitle = true;

	            ucMinAmount.Title = "金额";
				ucMinAmount.Type = "最小金额_收支明细";

                //ucType.AddAllButton = true;
                ucType.Title = "类型";
                ucType.Type = "收支查询类型";
                ucType.InstanceName = "ucType";
	            ucType.MustSelect = true;

                CreateGridViewColumn();
            }
        }

        protected void btQuery_Click(object sender, EventArgs e)
        {
	        var minAmount = Convert.ToInt32(ucMinAmount.SelectedOption.TrimStart(new char[]{'>', '='}));
            gvResult.DataSource = AccountManager.CreateDateTableFromMoneyInOutDetailQuery(
                ucQueryPeriod.StartDate,
				ucQueryPeriod.EndDate, ucType.SelectedOptions, ucType.UnselectedOptions, ucDesc.Text, minAmount);
            gvResult.DataBind();
        }

        private void CreateGridViewColumn()
        {
            GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.Date, HorizontalAlign.Left);
            GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.BankName, HorizontalAlign.Left);
            GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.CardName, HorizontalAlign.Left);
            GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.Income, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.Outcome, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.Description, HorizontalAlign.Left);
        }

        protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
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