using System;

namespace UserCtrl
{
	public partial class UserCtrl_SelectDateCtrl : System.Web.UI.UserControl
	{
		public string Date
		{
			get { return inputDate.Value; }
			set { inputDate.Value = value; }
		}

        public bool DisableInput { get; set; }
        public string Title { get; set; }
		public bool HideTitle { get; set; }
		public string Width { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (string.IsNullOrEmpty(Title))
					Title = "日期";
				lbTitle.Text = Title + "：";
				lbTitle.Visible = !HideTitle;

				if (string.IsNullOrEmpty(Date))
				{
					Date = DateTime.Today.ToString("yyyy-MM-dd");
				}

                inputDate.Disabled = DisableInput;

                if (!string.IsNullOrEmpty(Width))
					inputDate.Style["width"] = Width;
			}
		}
	}
}