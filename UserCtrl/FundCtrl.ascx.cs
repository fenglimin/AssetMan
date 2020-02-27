using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Business;
using Constants;
using UI;

namespace UserCtrl
{
    public partial class UserCtrl_FundCtrl : System.Web.UI.UserControl
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

                var dtFund = InvestmentManager.CreateDateTableFromAllFunds();
                gvAllFunds.DataSource = dtFund;
                gvAllFunds.DataBind();
            }
        }

        private void CreateGridViewColumn()
        {
            GridViewManager.AddHyperLinkFieldColumn(gvAllFunds, string.Empty, HorizontalAlign.Left);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.FundName, HorizontalAlign.Left);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.FundTotalAmount, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.FundTotalShare, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.FundNetWorth, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.FundTotalBenefit, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.WeightedBenefitRate, HorizontalAlign.Right);
        }

        protected void gvAllFunds_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                var hyperLink = e.Row.Cells[0].Controls[0] as HyperLink;
                hyperLink.Text = "更改净值";
                hyperLink.NavigateUrl = "~/Form/FundForm.aspx?Purchase=0')";
                GridViewManager.SetRowStyle(e.Row, Color.Black, true);
            }
        }
    }
}