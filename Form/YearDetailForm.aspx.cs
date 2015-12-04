using System;
using Business;

namespace Form
{
	public partial class Form_YearDetailForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			var year = Request.QueryString["Year"];
			if (string.IsNullOrEmpty(year))
				year = DateTime.Now.ToString("yyyy");

			if (!Page.IsPostBack)
			{
				ucAllYearStatistics.DataSource = AccountManager.CreateDataTableForAllYearStatistics();
				ucAllYearStatistics.Url = "~/Form/YearDetailForm.aspx?Year=";
				ucAllYearStatistics.Title = "历年来收支统计";

				ucTheYearStatistics.DataSource = AccountManager.CreateDataTableForOneYearStatistics(year);
				ucTheYearStatistics.Url = "~/Form/MonthDetail.aspx?Month=";
				ucTheYearStatistics.Title = year + "年收支统计";
			}
		}
	}
}