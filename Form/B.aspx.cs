using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Form_B : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
	protected void Button1_Click(object sender, EventArgs e)
	{
		//Response.Redirect("~/Form/A.aspx");

		var Iframe1 = Session["Iframe1"] as HtmlIframe;
		Iframe1.Src = "~/Form/TodoListForm.aspx";


//		urlFrameCenter.Attributes["src"] = "~/Form/TodoListForm.aspx";


	}
}