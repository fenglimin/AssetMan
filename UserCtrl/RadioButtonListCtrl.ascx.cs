using System;
using System.Linq;
using DataAccess;

namespace UserCtrl
{
	public partial class UserCtrl_RadioButtonListCtrl : System.Web.UI.UserControl
	{
		public string Title { get; set; }
		public string Type { get; set; }
		public string InitSelection { get; set; }

		public string SelectedOption
		{
			get { return rblOptions.SelectedItem.Text; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				lbTitle.Text = Title + "：";

				rblOptions.DataSource = SettingDal.GetStringValues(Type).OrderBy(x => Convert.ToUInt32(x.TrimStart('>', '=')));
				rblOptions.DataBind();

				if (rblOptions.Items.Count > 0)
				{
					rblOptions.Items[0].Selected = true;
					if (!string.IsNullOrEmpty(InitSelection))
					{
						for (var i = 0; i < rblOptions.Items.Count; i++)
						{
							if (rblOptions.Items[i].Text == InitSelection)
							{
								rblOptions.SelectedIndex = i;
								break;
							}
						}
					}
				}
			}
		}
	}
}