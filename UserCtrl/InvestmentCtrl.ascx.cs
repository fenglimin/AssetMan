using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Business;
using Constants;
using UI;

namespace UserCtrl
{
	public partial class UserCtrl_InvestmentCtrl : System.Web.UI.UserControl
	{
		public string Title { get; set; }
		public int EndDayPeriod { get; set; }
		public bool HideEndedInvest { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				lbTitle.Text = Title;
				CreateGridViewColumn();

                if (EndDayPeriod != 0)
                {
                    cbShowHistory.Visible = false;
                }

				gvAllInvestment.DataSource = InvestmentManager.CreateDateTableFromAllInvestments(HideEndedInvest, EndDayPeriod);
                gvAllInvestment.DataBind();
			}
		}

		private void CreateGridViewColumn()
		{
			GridViewManager.AddHyperLinkFieldColumn(gvAllInvestment, string.Empty, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvAllInvestment, TableFieldName.InvestType, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvAllInvestment, TableFieldName.InvestName, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvAllInvestment, TableFieldName.InvestAmount, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvAllInvestment, TableFieldName.InvestStartDate, HorizontalAlign.Left);
			GridViewManager.AddBoundFieldColumn(gvAllInvestment, TableFieldName.InvestPeriod, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvAllInvestment, TableFieldName.InvestBenifit, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvAllInvestment, TableFieldName.InvestBenifitRate, HorizontalAlign.Right);
			GridViewManager.AddBoundFieldColumn(gvAllInvestment, TableFieldName.InvestAvailDate, HorizontalAlign.Left);
		}

		protected void gvAllInvestment_RowDataBound(object sender, GridViewRowEventArgs e)
		{
            var dataTable = gvAllInvestment.DataSource as DataTable;

            if (e.Row.RowIndex == 0)
            {
                var hyperLink = e.Row.Cells[0].Controls[0] as HyperLink;
                hyperLink.Text = (dataTable.Rows.Count - 1) + " 条记录";
                GridViewManager.SetRowStyle(e.Row, Color.Red, true);
                return;
            }

            if (e.Row.RowIndex >= 0)
			{
				if (e.Row.Cells[6].Text != "0")
				{
					e.Row.Cells[0].Text = String.Empty;
					return;
				}

				var amount = Convert.ToInt32(e.Row.Cells[3].Text);
				var rate = Convert.ToDouble(e.Row.Cells[7].Text.TrimEnd('%'));
				var period = Convert.ToInt32(e.Row.Cells[5].Text);
				var benifit = (int)(amount * rate / 365 * period / 100);
				e.Row.Cells[6].Text = "~" + benifit / 10 * 10;


				DateTime moneyInDate;
				DateTime.TryParse(e.Row.Cells[8].Text, out moneyInDate);

				
				var investId = dataTable.Rows[e.Row.RowIndex][TableFieldName.Id].ToString();

				var hyperLink = e.Row.Cells[0].Controls[0] as HyperLink;
				//var linkDelete = new HyperLink {Text = " 删除", NavigateUrl = "~/Form/AddOilForm.aspx"};
				//linkDelete.ControlStyle.Font.Underline = false;
				//e.Row.Cells[0].Controls.Add(linkDelete);

				if (moneyInDate <= DateTime.Today)
				{
					hyperLink.Text = "到期";
					hyperLink.NavigateUrl = "~/Form/InvestmentForm.aspx?Action=3&InvestId=" + investId;
					GridViewManager.SetRowStyle(e.Row, Color.Red, true);
				}
				else
				{
					hyperLink.Text = "更改";
					hyperLink.NavigateUrl = "~/Form/InvestmentForm.aspx?Action=2&InvestId=" + investId;
					GridViewManager.SetRowStyle(e.Row, Color.Black, true);
				}
			}
		}

        protected void cbShowHistory_CheckedChanged(object sender, EventArgs e)
        {
            HideEndedInvest = !cbShowHistory.Checked;
            gvAllInvestment.DataSource = InvestmentManager.CreateDateTableFromAllInvestments(HideEndedInvest, EndDayPeriod);
            gvAllInvestment.DataBind();
        }
    }
}