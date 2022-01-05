using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;

namespace UserCtrl
{
	public partial class UserCtrl_SelectEditCtrl : System.Web.UI.UserControl
	{
		public string Title { get; set; }
		public string Type { get; set; }
		public int Height { get; set; }
		public string InstanceName { get; set; }
		public bool EnableValidate { get; set; }
        public bool EnableInput { get; set; }

        public string Text
		{
			get { return tbEdit.Text; }
            set { tbEdit.Text = value; }
        }
		public string InitText { get; set; }

		public UserCtrl_SelectEditCtrl()
		{
			EnableValidate = true;
            EnableInput = true;
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				lbTitle.Text = Title + "：";
				rfvEdit.ErrorMessage = Title + "不能为空";
				rfvEdit.Enabled = EnableValidate;

				cbSave.Text = "保存到模板";

				lbSelect.Height = Height;
				lbSelect.Attributes.Add("onchange", "OnSelectEditChanged('" + InstanceName + "', this.options[this.selectedIndex].value)");

				tbEdit.Attributes.Add("oninput", "OnInput('" + InstanceName + "', this.value)");
				LoadSetting(Type);

                tbEdit.Enabled = EnableInput;
                lbSelect.Enabled = EnableInput;
                cbSave.Enabled = EnableInput;
            }
			else
			{
				if (!string.IsNullOrEmpty(Type) && Type != (string) ViewState["SelectEditType"])
				{
					LoadSetting(Type);
				}
			}

            if (!string.IsNullOrEmpty(InitText))
				tbEdit.Text = InitText;
		}

		private void LoadSetting(string type)
		{
			lbSelect.DataSource = SettingDal.GetStringValues(type).OrderBy(x => x);
			lbSelect.DataBind();

			ViewState["SelectEditType"] = type;
		}

		public void AddToSetting()
		{
			if (!cbSave.Checked)
				return;

			SettingDal.AddSetting(ViewState["SelectEditType"].ToString(), Text);
		}

        protected void btDelete_Click(object sender, EventArgs e)
        {
            var type = ViewState["SelectEditType"].ToString();
            if (SettingDal.DeleteSetting(type, Text))
            {
                LoadSetting(type);
            }
        }
    }
}