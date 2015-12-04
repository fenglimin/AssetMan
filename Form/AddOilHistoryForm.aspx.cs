using System;
using System.Web.UI.WebControls;
using Constants;
using UI;

namespace Form
{
	public partial class Form_AddOilHistoryForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		private void CreateGridViewColumn()
		{
			GridViewManager.AddBoundFieldColumn(gvAddOilHistory, TableFieldName.Date, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvAddOilHistory, TableFieldName.BankName, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvAddOilHistory, TableFieldName.CardName, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvAddOilHistory, TableFieldName.Income, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvAddOilHistory, TableFieldName.Outcome, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvAddOilHistory, TableFieldName.Description, HorizontalAlign.Left);
		}

		protected void gvAddOilHistory_Sorting(object sender, GridViewSortEventArgs e)
		{
			var sortExpression = e.SortExpression;
		}
}
}