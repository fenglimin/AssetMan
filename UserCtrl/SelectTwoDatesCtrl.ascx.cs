using System;
using System.Web;

namespace UserCtrl
{
	public partial class UserCtrl_SelectTwoDatesCtrl : System.Web.UI.UserControl
	{
		public string StartDate
		{
			get { return startDate.Value; }
			set { startDate.Value = value; }
		}

		public string EndDate
		{
			get { return endDate.Value; }
			set { endDate.Value = value; }
		}

		public void SetInitDate(DateTime start, DateTime end)
		{
			StartDate = start.ToString("yyyy-MM-dd");
			EndDate = end.ToString("yyyy-MM-dd");
		}

		public string StartTitle { get; set; }
		public string EndTitle { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (string.IsNullOrEmpty(StartTitle))
					StartTitle = "起始";
				lbStart.Text = StartTitle + "：";

				if (string.IsNullOrEmpty(StartDate))
				{
					StartDate = DateTime.Today.ToString("yyyy-MM-dd");
				}

				if (string.IsNullOrEmpty(EndTitle))
					EndTitle = "终止";
				lbEnd.Text = EndTitle + "：";

				if (string.IsNullOrEmpty(EndDate))
				{
					EndDate = DateTime.Today.ToString("yyyy-MM-dd");
				}

				cvDate.ErrorMessage = string.Format("{0}日期必须大于{1}日期！", EndTitle, StartTitle);
			}
		}
	}
}