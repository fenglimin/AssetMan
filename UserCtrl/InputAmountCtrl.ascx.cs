using System;

namespace UserCtrl
{
	public partial class UserCtrl_InputAmountCtrl : System.Web.UI.UserControl
	{
		public string Amount
		{
			get { return tbAmount.Text; }
		}

		public string InitAmount { get; set; }

		public string MinimumValue { get; set; }
		public string MaximunValue { get; set; }
		public bool EnableValidate { get; set; }
		public bool EnableInput { get; set; }

		public string Title { get; set; }

		public UserCtrl_InputAmountCtrl()
		{
			EnableValidate = true;
			EnableInput = true;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				lbTitle.Text = Title + "：";

				rfvAmount.ErrorMessage = Title + "不能为空";
				rfvAmount.Enabled = EnableValidate;

				rvAmount.ErrorMessage = string.Format("请输入{0}-{1}间的数字", MinimumValue, MaximunValue);
				rvAmount.MinimumValue = MinimumValue;
				rvAmount.MaximumValue = MaximunValue;
				rvAmount.Enabled = EnableValidate;

				tbAmount.Text = InitAmount;
				tbAmount.Enabled = EnableInput;
			}
		}

        public void SetAmount(int amount)
        {
            tbAmount.Text = amount.ToString();
        }

        public void SetFocus()
        {
            tbAmount.Focus();
        }
	}
}