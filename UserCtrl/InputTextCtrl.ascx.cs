using System;

namespace UserCtrl
{
	public partial class UserCtrl_InputTextCtrl : System.Web.UI.UserControl
	{
        public string Title { get; set; }

        public bool EnableInput { get; set; }

        public bool EnableValidate { get; set; }

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

                if (!string.IsNullOrEmpty(Title))
                    lbTitle.Text = Title + "：";

                tbInput.Enabled = EnableInput;

                rfvEdit.ErrorMessage = Title + "不能为空";
                rfvEdit.Enabled = EnableValidate;
            }
		}
	}
}