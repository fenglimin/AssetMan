using System;

namespace UserCtrl
{
	public partial class UserCtrl_InputTextCtrl : System.Web.UI.UserControl
	{
		public string Text
		{
			get { return tbInput.Text; }
		}

		public string InitText { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(InitText))
					tbInput.Text = InitText;
			}
		}
	}
}