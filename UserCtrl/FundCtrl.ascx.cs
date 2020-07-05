using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Business;
using Constants;
using DataAccess;
using UI;

namespace UserCtrl
{
    public partial class UserCtrl_FundCtrl : System.Web.UI.UserControl
    {
        public string Title { get; set; }

        public bool ForTodoList { get; set; }

        public string FundId { get; set; }

        public string FundName { get; set; }

        public bool HideEndedFund { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HideEndedFund = true;
                lbTitle.Text = Title;
                CreateGridViewColumn();

                lbFundDetail.Visible = !ForTodoList;
                gvFundDetail.Visible = !ForTodoList;
                cbShowHistory.Visible = !ForTodoList;

                var dtFund = InvestmentManager.CreateDateTableFromAllFunds();
                dtFund = AdjustFund(dtFund, HideEndedFund);

                gvAllFunds.DataSource = dtFund;
                gvAllFunds.DataBind();

                if (!ForTodoList)
                {
                    CreateDetailGridViewColumn();

                    RefreshFundDetail(dtFund);
                }
                
            }
        }

        private DataTable AdjustFund(DataTable dtFund, bool hideEndedFund)
        {
            if (hideEndedFund)
            {
                var count = dtFund.Rows.Count;
                for (var i = 0; i < count; i++)
                {
                    if (dtFund.Rows[i][TableFieldName.FundTotalAmount].ToString() == "0")
                    {
                        dtFund.Rows.RemoveAt(i);
                        count = dtFund.Rows.Count;
                        i--;
                    }
                }
            }
            
            return dtFund;
        }

        private void CreateGridViewColumn()
        {
            GridViewManager.AddHyperLinkFieldColumn(gvAllFunds, string.Empty, HorizontalAlign.Left);
            if (!ForTodoList)
            {
                GridViewManager.AddHyperLinkFieldColumn(gvAllFunds, string.Empty, HorizontalAlign.Left);
                GridViewManager.AddHyperLinkFieldColumn(gvAllFunds, TableFieldName.FundName, HorizontalAlign.Left);
            }
            else
            {
                GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.FundName, HorizontalAlign.Left);
            }

            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.Date, HorizontalAlign.Left);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.FundTotalAmount, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.FundTotalShare, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.FundNetWorth, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.WeightedBenefitRate, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.FundTotalBenefit, HorizontalAlign.Right);
        }

        private void CreateDetailGridViewColumn()
        {
            GridViewManager.AddBoundFieldColumn(gvFundDetail, TableFieldName.Date, HorizontalAlign.Left);
            GridViewManager.AddBoundFieldColumn(gvFundDetail, TableFieldName.Type, HorizontalAlign.Left);
            GridViewManager.AddBoundFieldColumn(gvFundDetail, TableFieldName.Balance, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvFundDetail, TableFieldName.NetWorth, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvFundDetail, TableFieldName.Share, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvFundDetail, TableFieldName.ShareAvailable, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvFundDetail, TableFieldName.InvestBenifitRate, HorizontalAlign.Right);
        }

        protected void gvAllFunds_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex == 0)
            {
                var hyperLink = e.Row.Cells[0].Controls[0] as HyperLink;
                hyperLink.Text = "汇总";
                GridViewManager.SetRowStyle(e.Row, Color.Red, true);
                return;
            }

            if (e.Row.RowIndex > 0)
            {
                var dataTable = gvAllFunds.DataSource as DataTable;
                var fundId = dataTable.Rows[e.Row.RowIndex][TableFieldName.FundID].ToString();
                var fundName = dataTable.Rows[e.Row.RowIndex][TableFieldName.FundName].ToString();

                var hyperLink = e.Row.Cells[0].Controls[0] as HyperLink;
                if (ForTodoList)
                {
                    hyperLink.Text = "更改净值";
                    hyperLink.NavigateUrl = "~/Form/FundForm.aspx?OpType=ChangeNetWorth&FundId=" + fundId;
                }
                else
                {
                    hyperLink.Text = "申购";
                    hyperLink.NavigateUrl = "~/Form/FundForm.aspx?OpType=Purchase&FundId=" + fundId;
                }
                GridViewManager.SetRowStyle(e.Row, Color.Black, true);

                if (!ForTodoList)
                {
                    hyperLink = e.Row.Cells[1].Controls[0] as HyperLink;
                    hyperLink.Text = "赎回";
                    hyperLink.NavigateUrl = "~/Form/FundForm.aspx?OpType=Redemption&FundId=" + fundId;

                    hyperLink = e.Row.Cells[2].Controls[0] as HyperLink;
                    hyperLink.NavigateUrl = string.Format("~/Form/AllInvestmentsForm.aspx?FundId={0}&FundName={1}", fundId, fundName);
                }
            }
        }

        protected void cbShowHistory_CheckedChanged(object sender, EventArgs e)
        {
            HideEndedFund = !cbShowHistory.Checked;

            var dtFund = InvestmentManager.CreateDateTableFromAllFunds();
            dtFund = AdjustFund(dtFund, HideEndedFund);
            gvAllFunds.DataSource = dtFund;
            gvAllFunds.DataBind();

            RefreshFundDetail(dtFund);
        }

        private void RefreshFundDetail(DataTable dtFund)
        {
            if (string.IsNullOrEmpty(FundId) && dtFund.Rows.Count > 0)
            {
                FundId = dtFund.Rows[1][TableFieldName.FundID].ToString();
                FundName = dtFund.Rows[1][TableFieldName.FundName].ToString();
            }

            if (string.IsNullOrEmpty(FundId))
            {
                return;
            }

            lbFundDetail.Text = string.Format("【{0}】交易明细", FundName);
            var condition = string.Format("WHERE FundID = {0}", FundId);
            var dtFundDetail = InvestmentManager.CreateDateTableFromFund(condition);
            gvFundDetail.DataSource = dtFundDetail;
            gvFundDetail.DataBind();
        }
    }
}