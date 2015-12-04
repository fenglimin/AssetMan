using System;

namespace Form
{
	public partial class Form_ResultForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				lbTitle.Text = Request.QueryString["Title"];
				lbMessage.Text = Request.QueryString["Message"];
				btGo.Text = Request.QueryString["ButtonText"];
			}
		}
		protected void btGo_Click(object sender, EventArgs e)
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "click", "go('" + Request.QueryString["ButtonUrl"] + "');", true);
		}
}
}