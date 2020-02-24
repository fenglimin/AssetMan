using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Business;
using Constants;
using DataAccess;
using UI;

public partial class _Default : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
    {
        // Access the connect to trigger db connect. (Fix connect issue when to page access db at the same time)
        var aa = DbManager.OleDbConn;
    }


	protected void LinkButton1_Command(object sender, CommandEventArgs e)
	{
		//Iframe1.Attributes["src"] = "~/Form/TodoListForm.aspx";

		//var Iframe11 = Session["Iframe1"] as System.Web.UI.HtmlControls.HtmlIframe;
		//Iframe11.Src = "~/Form/TodoListForm.aspx";

		//var urlFrameCenter1 = Session["urlFrameCenter"] as HtmlIframe;

		//urlFrameCenter1.Attributes["src"] = "~/Form/TodoListForm.aspx";
	}
}