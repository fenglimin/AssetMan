using System;
using DataAccess;

namespace UserCtrl
{
	public partial class UserCtrl_SelectCtrl : System.Web.UI.UserControl
	{
		public string Title { get; set; }
		public string Type { get; set; }
		public bool AutoPostBack { get; set; }

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

				ddlSelect.DataSource = SettingDal.GetStringValues(Type);
				ddlSelect.DataBind();
			}
		}
	}
}