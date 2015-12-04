using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Constants;
using DataAccess;
using UI;

namespace UserCtrl
{
	public partial class UserCtrl_MoneyInOutStatisticsCtrl : System.Web.UI.UserControl
	{
		public DataTable DataSource { get; set; }
		public string Url { get; set; }
		public string Title { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				lbTitle.Text = Title;
				hfUrl.Value = Url;
				CreateGridViewColumn();

				gvResult.DataSource = DataSource;
				gvResult.DataBind();

			    var dataTable = gvResult.DataSource as DataTable;
			    if (dataTable != null)
			    {
			        dataTable.Rows.RemoveAt(0);
                    Chart1.DataSource = DataSource;
                    Chart1.Series[0].XValueMember = TableFieldName.Date;
                    Chart1.Series[0].YValueMembers = TableFieldName.Surplus;
			    }

				
			}
		}

		private void CreateGridViewColumn()
		{
			GridViewManager.AddHyperLinkFieldColumn(gvResult, TableFieldName.Date, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.Income, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.Outcome, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvResult, TableFieldName.Surplus, HorizontalAlign.Right);
		}

		protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.DataRow) return;

			var key = Convert.ToInt32(e.Row.Cells[3].Text);
			var color = key < 0 ? Color.Green : Color.Red;

			if (e.Row.RowIndex == 0)
			{
				e.Row.Cells[0].Text = "汇总";
				e.Row.BackColor = color;
				GridViewManager.SetRowStyle(e.Row, Color.White, true);
				return;
			}



			GridViewManager.SetRowStyle(e.Row, color, Math.Abs(key) >= 10000);

			var hyperLink = e.Row.Cells[0].Controls[0] as HyperLink;
			if (hyperLink == null)
				return;

			hyperLink.NavigateUrl = hfUrl.Value + hyperLink.Text;

		}
	}
}