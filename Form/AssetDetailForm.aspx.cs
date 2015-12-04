using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Constants;
using DataAccess;
using UI;

public partial class Form_AssetDetailForm : System.Web.UI.Page
{
    private DateTime currentDay;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CreateGridViewColumn();
            currentDay = DateTime.Today;
            OnCurrentDayChanged(currentDay, true);
			AssetDetailDal.UpdateTodayAssetDetail(Request.QueryString["Reason"]);

			//AssetDetailManager.Test();
        }
        else
        {
            currentDay = ViewState["CurrentDay"] is DateTime ? (DateTime)ViewState["CurrentDay"] : DateTime.Today;
        }
    }

    private void CreateGridViewColumn()
    {
        GridViewManager.AddBoundFieldColumn(gvTodayAll, TableFieldName.Type, HorizontalAlign.Left);
        GridViewManager.AddBoundFieldColumn(gvTodayAll, TableFieldName.Balance, HorizontalAlign.Right);

        GridViewManager.AddBoundFieldColumn(gvTodayBankCard, TableFieldName.BankName, HorizontalAlign.Left);
        GridViewManager.AddBoundFieldColumn(gvTodayBankCard, TableFieldName.CardName, HorizontalAlign.Left);
        GridViewManager.AddBoundFieldColumn(gvTodayBankCard, TableFieldName.Balance, HorizontalAlign.Right);

		GridViewManager.AddBoundFieldColumn(gvDayAction, TableFieldName.BankName, HorizontalAlign.Left);
		GridViewManager.AddBoundFieldColumn(gvDayAction, TableFieldName.CardName, HorizontalAlign.Left);
		GridViewManager.AddBoundFieldColumn(gvDayAction, TableFieldName.DayDetailActionType, HorizontalAlign.Left);
		GridViewManager.AddBoundFieldColumn(gvDayAction, TableFieldName.Balance, HorizontalAlign.Right);
    }

    protected void btPrevDay_Click(object sender, EventArgs e)
    {
        currentDay = currentDay.AddDays(-1);
        OnCurrentDayChanged(currentDay, true);
    }

    protected void btNextDay_Click(object sender, EventArgs e)
    {
        currentDay = currentDay.AddDays(1);
        OnCurrentDayChanged(currentDay, false);
    }

    private void OnCurrentDayChanged(DateTime targetDay, bool nearestOld)
    {
        var date = targetDay == DateTime.Today ? null : targetDay.ToString("yyyy-MM-dd");
        string loadedDate;
        var ds = AssetDetailManager.CreateAssetDetailDataSet(date, nearestOld, out loadedDate);

        lblTodayAsset.Text = loadedDate;
        DateTime.TryParse(loadedDate, out targetDay);
        ViewState["CurrentDay"] = targetDay;
        btNextDay.Enabled = targetDay < DateTime.Today;

        gvTodayAll.DataSource = ds.Tables[0];
        gvTodayAll.DataBind();
        gvTodayBankCard.DataSource = ds.Tables[1];
        gvTodayBankCard.DataBind();
		//gvTodayBankCard.AllowSorting = true;
		//gvTodayBankCard.Sort(TableFieldName.Balance, SortDirection.Ascending);
	    gvDayAction.DataSource = ds.Tables[2];
		gvDayAction.DataBind();
    }

    protected void gvTodayBankCard_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        if (e.Row.Cells[2].Text.StartsWith("-"))
        {
            GridViewManager.SetRowStyle(e.Row, Color.Green, true);
        }
    }
	protected void gvDayAction_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType != DataControlRowType.DataRow) return;

		var fontColor = Color.Black;
		if (e.Row.Cells[2].Text == "收入")
			fontColor = Color.Red;
		else if (e.Row.Cells[2].Text == "支出")
			fontColor = Color.Green;

		GridViewManager.SetRowStyle(e.Row, fontColor, true);
	}
}