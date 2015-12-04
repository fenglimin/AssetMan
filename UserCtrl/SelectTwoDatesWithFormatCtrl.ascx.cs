using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using DataAccess;
using Translation;

namespace UserCtrl
{
	public partial class UserCtrl_SelectTwoDatesWithFormatCtrl : System.Web.UI.UserControl
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

		public string InitStartDate { get; set; }
		public string InitEndDate { get; set; }

		public bool HideTitle { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				lbTitle.Visible = !HideTitle;
				ddlPeriodFormat.DataSource = CreateQueryPeriod();
				ddlPeriodFormat.DataBind();

				if (!string.IsNullOrEmpty(InitStartDate) && !string.IsNullOrEmpty(InitEndDate))
				{
					SetQueryPeriod(TranslationManager.Translate(DropListItemName.FreeSet));
					StartDate = InitStartDate;
					EndDate = InitEndDate;
				}
				else
				{
					var defaultPeriod = SettingDal.GetStringValues("默认查询时间范围")[0];
					SetQueryPeriod(defaultPeriod);	
				}
			}
		}

		private void SetQueryPeriod(string queryPeriod)
		{
			var selectedItem = ddlPeriodFormat.Items.FindByText(queryPeriod);
			if (selectedItem != null)
				selectedItem.Selected = true;
			else
				queryPeriod = ddlPeriodFormat.SelectedItem.Text;

			var startQueryDate = DateTime.Today;
			var endQueryDate = startQueryDate;

			var dtDayOneCurrentWeek = endQueryDate.AddDays(-(int)endQueryDate.DayOfWeek + 1);
			var dtDayOneCurrentMonth = endQueryDate.AddDays(-endQueryDate.Day + 1);
			var dtDayOneCurrentYear = dtDayOneCurrentMonth.AddMonths(-dtDayOneCurrentMonth.Month + 1);

			startDate.Disabled = false;
			endDate.Disabled = false;

			if (queryPeriod == TranslationManager.Translate(DropListItemName.ThisWeek))
				startQueryDate = dtDayOneCurrentWeek;
			else if (queryPeriod == TranslationManager.Translate(DropListItemName.LastWeek))
				startQueryDate = endQueryDate.AddDays(-6);
			else if (queryPeriod == TranslationManager.Translate(DropListItemName.PrevWeek))
			{
				endQueryDate = dtDayOneCurrentWeek.AddDays(-1);
				startQueryDate = dtDayOneCurrentWeek.AddDays(-7);
			}
			else if (queryPeriod == TranslationManager.Translate(DropListItemName.ThisMonth))
				startQueryDate = dtDayOneCurrentMonth;
			else if (queryPeriod == TranslationManager.Translate(DropListItemName.LastMonth))
				startQueryDate = endQueryDate.AddMonths(-1).AddDays(1);
			else if (queryPeriod == TranslationManager.Translate(DropListItemName.PrevMonth))
			{
				endQueryDate = dtDayOneCurrentMonth.AddDays(-1);
				startQueryDate = dtDayOneCurrentMonth.AddMonths(-1);
			}
			else if (queryPeriod == TranslationManager.Translate(DropListItemName.ThisYear))
				startQueryDate = dtDayOneCurrentYear;
			else if (queryPeriod == TranslationManager.Translate(DropListItemName.LastYear))
			{
				startQueryDate = endQueryDate.AddYears(-1).AddDays(1);
			}
			else if (queryPeriod == TranslationManager.Translate(DropListItemName.PrevYear))
			{
				endQueryDate = dtDayOneCurrentYear.AddDays(-1);
				startQueryDate = dtDayOneCurrentYear.AddYears(-1);
			}
			else if (queryPeriod == TranslationManager.Translate(DropListItemName.FromDay))
			{
				endQueryDate = dtDayOneCurrentYear.AddYears(500);
				endDate.Disabled = true;
			}
			else if (queryPeriod == TranslationManager.Translate(DropListItemName.ToDay))
			{
				startQueryDate = dtDayOneCurrentYear.AddYears(-50);
				startDate.Disabled = true;
			}

			StartDate = startQueryDate.ToString("yyyy-MM-dd");
			EndDate = endQueryDate.ToString("yyyy-MM-dd");
		}

		private IList<string> CreateQueryPeriod()
		{
			var list = new List<string>()
			{
				TranslationManager.Translate(DropListItemName.ThisWeek),
				TranslationManager.Translate(DropListItemName.LastWeek),
				TranslationManager.Translate(DropListItemName.PrevWeek),
				TranslationManager.Translate(DropListItemName.ThisMonth),
				TranslationManager.Translate(DropListItemName.LastMonth),
				TranslationManager.Translate(DropListItemName.PrevMonth),
				TranslationManager.Translate(DropListItemName.ThisYear),
				TranslationManager.Translate(DropListItemName.LastYear),
				TranslationManager.Translate(DropListItemName.PrevYear),
				TranslationManager.Translate(DropListItemName.FromDay),
				TranslationManager.Translate(DropListItemName.ToDay),
				TranslationManager.Translate(DropListItemName.FreeSet),
			};

			return list;
		}

		protected void ddlPeriodFormat_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetQueryPeriod(ddlPeriodFormat.SelectedItem.Text);
		}
	}
}