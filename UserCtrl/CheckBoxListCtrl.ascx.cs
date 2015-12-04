using System;
using System.Activities.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DataAccess;
using Translation;

namespace UserCtrl
{
	public partial class UserCtrl_CheckBoxListCtrl : System.Web.UI.UserControl
	{
		public string Title { get; set; }
		public string Type { get; set; }
		public string InstanceName { get; set; }
		public bool AddAllButton { get; set; }
		public bool MustSelect { get; set; }
		public IList<string> InitSelectedOptions { get; set; }

		private IList<CheckBox> listOptions;

        public IList<string> SelectedOptions
        {
            get
            {
                var selectedOptions = (from checkBox in listOptions where checkBox.Checked select checkBox.Text.TrimEnd(' ')).ToList();
                return selectedOptions;
            }
        }

		public IList<string> UnselectedOptions
		{
			get
			{
				var unselectedOptions = (from checkBox in listOptions where !checkBox.Checked select checkBox.Text.TrimEnd(' ')).ToList();
				return unselectedOptions;
			}
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lbTitle.Text = Title + "：";
                hfType.Value = Type;
                hfInstanceName.Value = InstanceName;
                hfAddAll.Value = AddAllButton ? "1" : "0";
	            hfMustSelect.Value = MustSelect ? "1" : "0";
            }

            CheckBox cbAll = null;
            if (hfAddAll.Value == "1")
            {
                cbAll = CreateCheckBox("All");
	            cbAll.Font.Bold = true;
            }

            listOptions = new List<CheckBox>();
            var clientIdNames = string.Empty;
            var idList = SettingDal.GetStringValues(hfType.Value);
            foreach (var id in idList)
            {
	            var checkBox = CreateCheckBox(id);
                listOptions.Add(checkBox);
                clientIdNames += " " + hfInstanceName.Value + "_" + id;
            }

	        if (hfMustSelect.Value == "1")
	        {
		        foreach (var checkBox in listOptions)
		        {
					checkBox.Attributes["onclick"] = "checkMustSelect(this,'" + clientIdNames + "');";
		        }
	        }

            if (hfAddAll.Value == "1")
            {
                cbAll.Attributes["onclick"] = "checkAll(this,'" + clientIdNames + "');";
            }

	        if (InitSelectedOptions != null)
	        {
		        foreach (var checkBox in listOptions)
		        {
			        checkBox.Checked = InitSelectedOptions.Contains(checkBox.Text.TrimEnd(' '));
		        }
	        }
        }

        private CheckBox CreateCheckBox(string id)
        {
            var control = new CheckBox
            {
                ID = id,
                Text = TranslationManager.Translate(id) + "    ",
                Checked = true
            };

            pnlContainer.Controls.Add(control);

            return control;
        }
    }
}