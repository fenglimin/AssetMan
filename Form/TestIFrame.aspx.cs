using System;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Form
{
	public partial class Form_TestIFrame : System.Web.UI.Page, ICallbackEventHandler
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				RegisterCallBackReference();
			}
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public static dynamic StaticFunction(string paraName)
		{
			return new[]  
			{  
				new
				{  
					name = "This is the json data returned from static function call in code behind",  
					desc = "desc1"
				}  
			};
		}

		private void RegisterCallBackReference()
		{

			String callBack = Page.ClientScript.GetCallbackEventReference(this, "arg",
				"CallbackOnSucceeded", "context", "CallbackOnFailed", true);
			//String callBack = Page.ClientScript.GetCallbackEventReference(this, "arg",
			//   "", "context", "", true);
			String clientFunction = "function CallServer(arg, context){ "
			                        + callBack + "; }";
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
				"Call To Server", clientFunction, true);



		}

		public void RaiseCallbackEvent(string eventArgument)
		{
			//lbTest.Text = "fsafsfsfsf";
			Button3.Text = "aaaaaaaaaa";
		}

		public string GetCallbackResult()
		{
			return "This is the string returned from non static function call in code behind";
		}

	}
}