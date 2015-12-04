using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Form_TestUpdatePanel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	protected void UpdateButton_Click(object sender, EventArgs e)
	{
		DateTimeLabel1.Text = DateTime.Now.ToString();
		DateTimeLabel2.Text = DateTime.Now.ToString();
	}
}