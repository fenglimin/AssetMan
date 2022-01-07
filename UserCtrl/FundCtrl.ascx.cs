using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
                ViewState["ForTodoList"] = ForTodoList;
                HideEndedFund = true;
                lbTitle.Text = Title;
                CreateGridViewColumn();

                lbFundDetail.Visible = !ForTodoList;
                gvFundDetail.Visible = !ForTodoList;
                cbShowHistory.Visible = !ForTodoList;
                btRefresh.Visible = ForTodoList;
                btQuery.Visible = ForTodoList;
                cblFundType.Visible = ForTodoList;

                cblFundType.Title = "类型";
                cblFundType.Type = "净值型产品";
                cblFundType.MustSelect = true;
                cblFundType.InstanceName = "ucFund_cblFundType";

                var dtFund = InvestmentManager.CreateDateTableFromAllFunds(string.Empty);
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
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.NetWorthDelta, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.WeightedBenefitRate, HorizontalAlign.Right);
            GridViewManager.AddBoundFieldColumn(gvAllFunds, TableFieldName.FundTotalBonus, HorizontalAlign.Right);
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
            var forTodoList = (bool)ViewState["ForTodoList"];
            var dataTable = gvAllFunds.DataSource as DataTable;

            if (e.Row.RowIndex == 0)
            {
                if (!forTodoList)
                {
                    e.Row.Cells[0].ColumnSpan = 3;
                    e.Row.Cells.RemoveAt(1);
                    e.Row.Cells.RemoveAt(1);
                }

                var hyperLink = e.Row.Cells[0].Controls[0] as HyperLink;
                hyperLink.Text = (dataTable.Rows.Count-1) + " 条记录";
                GridViewManager.SetRowStyle(e.Row, Color.Red, true);
                return;
            }

            if (e.Row.RowIndex > 0)
            {
                var fundId = dataTable.Rows[e.Row.RowIndex][TableFieldName.FundID].ToString();
                var fundName = dataTable.Rows[e.Row.RowIndex][TableFieldName.FundName].ToString();

                var hyperLink = e.Row.Cells[0].Controls[0] as HyperLink;
                if (forTodoList)
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

                if (!forTodoList)
                {
                    hyperLink = e.Row.Cells[1].Controls[0] as HyperLink;
                    hyperLink.Text = "赎回";
                    hyperLink.NavigateUrl = "~/Form/FundForm.aspx?OpType=Redemption&FundId=" + fundId;

                    hyperLink = e.Row.Cells[2].Controls[0] as HyperLink;
                    hyperLink.Text = "分红";
                    hyperLink.NavigateUrl = "~/Form/FundForm.aspx?OpType=Bonus&FundId=" + fundId;

                    hyperLink = e.Row.Cells[3].Controls[0] as HyperLink;
                    hyperLink.NavigateUrl = string.Format("~/Form/AllInvestmentsForm.aspx?FundId={0}&FundName={1}", fundId, fundName);

                    if (e.Row.Cells[8].Text != "0.0000" && e.Row.Cells[4].Text == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        var key = Convert.ToDouble(e.Row.Cells[8].Text);
                        e.Row.Cells[8].ForeColor = key < 0 ? Color.Green : Color.Red;
                    }
                }
                else
                {
                    if (e.Row.Cells[6].Text != "0.0000" && e.Row.Cells[2].Text == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        var key = Convert.ToDouble(e.Row.Cells[6].Text);
                        e.Row.Cells[6].ForeColor = key < 0 ? Color.Green : Color.Red;
                    }
                }
            }
        }

        protected void cbShowHistory_CheckedChanged(object sender, EventArgs e)
        {
            HideEndedFund = !cbShowHistory.Checked;

            var dtFund = InvestmentManager.CreateDateTableFromAllFunds(string.Empty);
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

        protected void btRefresh_Click(object sender, EventArgs e)
        {
            var fundList = InvestDal.LoadFundList("WHERE TotalAmount <> 0");
            var fundTypeList = SettingDal.GetStringValues("净值型产品");
            var workDir = AppDomain.CurrentDomain.BaseDirectory;

            var netWorthLines = new List<string>();
            foreach (var fundType in fundTypeList)
            {
                var fileName = Path.Combine(workDir, "PY", fundType) + "净值.txt";
                if (File.Exists(fileName))
                {
                    netWorthLines = netWorthLines.Concat(File.ReadAllLines(fileName)).ToList();
                }
            }

            foreach (var fund in fundList)
            {
                foreach (var line in netWorthLines)
                {
                    var split = line.Split(' ');
                    if (split.Length == 2 && split[1] == fund.FundCode)
                    {
                        var newNetWorth = Convert.ToDouble(split[0]);
                        if (Math.Abs(fund.CurrentNetWorth - newNetWorth) > 0.00001)
                        {
                            InvestDal.CalculateFund(fund.FundId, newNetWorth, newNetWorth-fund.CurrentNetWorth, DateTime.Now.ToString("yyyy-MM-dd"));
                        }
                    }
                }

            }

            DoQuery();
        }

        protected void btQuery_Click(object sender, EventArgs e)
        {
            DoQuery();
        }

        private void DoQuery()
        {
            var joinedNames = cblFundType.SelectedOptions.Aggregate((a, b) => a + "', '" + b);
            var condition = "WHERE FundType IN ('" + joinedNames + "')";
            ViewState["ForTodoList"] = true;
            var dtFund = InvestmentManager.CreateDateTableFromAllFunds(condition);
            dtFund = AdjustFund(dtFund, true);
            gvAllFunds.DataSource = dtFund;
            gvAllFunds.DataBind();
        }
    }
}