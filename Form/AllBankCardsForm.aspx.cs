using System;

namespace Form
{
	public partial class Form_AllBankCardsForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			//this.SqlDataSource1.SelectParameters[0].DefaultValue = "aa";
		}
		protected void SqlDataSource1_Updating(object sender, System.Web.UI.WebControls.SqlDataSourceCommandEventArgs e)
		{
		}
}
}