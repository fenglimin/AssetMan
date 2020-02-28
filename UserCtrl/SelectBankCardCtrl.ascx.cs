using System;
using System.Collections;
using System.Globalization;
using System.Web.UI.WebControls;
using DataAccess;
using Entities;

namespace UserCtrl
{
	public partial class UserCtrl_SelectBankCardCtrl : System.Web.UI.UserControl
	{
		public string CardUsage { get; set; }
		public int InitCardId { get; set; }
		public bool HideTitle { get; set; }
        public bool EnableInput { get; set; }

		public int CardId
		{
			get { return Convert.ToInt32(ddlCardName.SelectedItem.Value); }
		}

		public string BankName
		{
			get { return ddlBankName.SelectedItem.Text; }
		}

		public string CardName
		{
			get { return ddlCardName.SelectedItem.Text; }
		}

        public UserCtrl_SelectBankCardCtrl()
        {
            EnableInput = true;
        }

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				var defaultBankCard = LoadDefaultBankCard(CardUsage);
				LoadBankName(CardUsage, defaultBankCard.BankName);
				lbTitle.Text = "选择" + CardUsage + "卡：";
				lbTitle.Visible = !HideTitle;
				ViewState["CardUsage"] = CardUsage;
				LoadCardName(CardUsage, ddlBankName.Text, defaultBankCard.CardName);
                ddlBankName.Enabled = EnableInput;
                ddlCardName.Enabled = EnableInput;
            }
			else
			{
				CardUsage = ViewState["CardUsage"].ToString();
			}
		}

		private void LoadBankName(string cardUsage, string defaultBankName)
		{
			ddlBankName.DataSource = BankCardDal.GetBankByCardUsage(cardUsage);
			ddlBankName.DataBind();

			var defaultItem = ddlBankName.Items.FindByText(defaultBankName);
			if (defaultItem != null)
				defaultItem.Selected = true;
		}

		private void LoadCardName(string cardUsage, string bankName, string defaultCardName)
		{
			ddlCardName.Items.Clear();
			var cards = BankCardDal.GetCardByBankAndUsage(bankName, cardUsage);
			foreach (DictionaryEntry item in cards)
			{
				var bankCard = item.Value as BankCard;
				if (bankCard == null)
					continue;

				if ((CardUsage == "支出" || CardUsage == "转出" || CardUsage == "加油") && bankName != "其他" && bankCard.Account <= 0)
					continue;

				var listItem = new ListItem(bankCard.CardName, bankCard.CardId.ToString(CultureInfo.CurrentCulture));
				ddlCardName.Items.Add(listItem);
			}

			var defaultItem = ddlCardName.Items.FindByText(defaultCardName);
			if (defaultItem != null)
				defaultItem.Selected = true;
		}

		protected void ddlBankName_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadCardName(CardUsage, ddlBankName.Text, string.Empty);
		}

		private BankCard LoadDefaultBankCard(string cardUsage)
		{
			if (InitCardId == 0)
			{
				if (cardUsage == null)
					return new BankCard();

				var list = SettingDal.GetIntValues("默认" + cardUsage + "卡");
				if (list.Count != 0)
					InitCardId = list[0];
			}

			return BankCardDal.GetAllAvailableCards()[InitCardId] as BankCard;
		}
	}
}