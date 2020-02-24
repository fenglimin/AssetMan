using System;
using System.Web.UI;

namespace Form
{
	public partial class Form_TodayOperationsForm : System.Web.UI.Page
	{
		private DateTime currentDay;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				currentDay = DateTime.Today;

				ucDayOperations.ShowDateColumn = true;
				ucDate.HideTitle = true;
				ucDate.Width = "100px";
			}
			else
			{
				currentDay = ViewState["CurrentDay"] is DateTime ? (DateTime)ViewState["CurrentDay"] : DateTime.Today;
			}
		}

		protected void btPrevDay_Click(object sender, EventArgs e)
        {
            bool hasData;
            do
            {
                currentDay = currentDay.AddDays(-1);
                hasData = OnCurrentDayChanged() > 0;
            } while (!hasData);
        }

		protected void btNextDay_Click(object sender, EventArgs e)
		{
            bool hasData;
            do
            {
                currentDay = currentDay.AddDays(1);
                hasData = OnCurrentDayChanged() > 0;
            } while (!hasData);
        }

		private int OnCurrentDayChanged()
		{
			ViewState["CurrentDay"] = currentDay;
			var loadDay = currentDay.ToString("yyyy-MM-dd");
			ucDate.Date = loadDay;
			return ucDayOperations.QueryDayDetail(loadDay, loadDay, string.Empty);
		}
	}
}