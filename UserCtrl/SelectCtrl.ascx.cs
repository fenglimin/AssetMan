using System;
using DataAccess;

namespace UserCtrl
{
	public partial class UserCtrl_SelectCtrl : System.Web.UI.UserControl
	{
		public string Title { get; set; }
		public string Type { get; set; }
		public bool AutoPostBack { get; set; }
        public bool EnableInput { get; set; }
        public string FixedOption { get; set; }

        public string Text
		{
			get { return ddlSelect.SelectedItem.Text; }
			set { ddlSelect.Text = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				lbTitle.Text = Title + "：";
				ddlSelect.AutoPostBack = AutoPostBack;

                var dataSource = SettingDal.GetStringValues(Type);
                if (!string.IsNullOrEmpty(FixedOption))
                {
                    dataSource.Insert(0, FixedOption);
                }

                ddlSelect.DataSource = dataSource;
				ddlSelect.DataBind();

                ddlSelect.Enabled = EnableInput;
            }
		}
	}
}