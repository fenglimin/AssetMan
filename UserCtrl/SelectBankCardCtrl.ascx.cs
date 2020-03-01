using System;
using System.Collections;
using System.Collections.Generic;
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
            var allAvailableCards = BankCardDal.GetAllAvailableCards();
            var list = SettingDal.GetIntValues("默认" + cardUsage + "卡");
            if (cardUsage == null || list.Count == 0)
            {
                return InitCardId == 0 ? new BankCard() : allAvailableCards[InitCardId] as BankCard;
            }

            if (InitCardId == 0)
            {
                InitCardId = list[0];
            }

            var autoSelect = false;
            var disableEmptyCard = false;
            if (CardUsage == "支出" || CardUsage == "转出" || CardUsage == "加油")
            {
                disableEmptyCard = true;
                var bankCard = allAvailableCards[InitCardId] as BankCard;
                autoSelect = bankCard.BankName != "其他" && bankCard.Account <= 0;
            }

            //if (list.Count > 1)
            //{
            //    var bankCard1 = allAvailableCards[list[0]] as BankCard;
            //    rbCard1.Text = bankCard1.Alias;
            //    rbCard1.Attributes.Add("value", bankCard1.BankName + "_" + bankCard1.CardName);

            //    var bankCard2 = allAvailableCards[list[1]] as BankCard;
            //    rbCard2.Text = bankCard2.Alias;
            //    rbCard2.Attributes.Add("value", bankCard2.BankName + "_" + bankCard2.CardName);
            //}

            var listCard = new List<RadioButton> {rbCard1, rbCard2, rbCard3, rbCard4};

            var noChecked = true;
            for (var i = 1; i <= list.Count; i++)
            {
                var bankCard = allAvailableCards[list[i - 1]] as BankCard;
                listCard[i - 1].Text = bankCard.Alias;
                listCard[i - 1].Visible = true;
                listCard[i - 1].Enabled = !(disableEmptyCard && bankCard.Account <= 0);

                listCard[i - 1].Checked = listCard[i - 1].Enabled && noChecked && ( autoSelect || bankCard.CardId == InitCardId);
                if (listCard[i - 1].Checked)
                {
                    noChecked = false;
                    InitCardId = list[i - 1];
                }
                
                ViewState["Bank" + i] = bankCard.BankName;
                ViewState["Card" + i] = bankCard.CardName;
            }

            return allAvailableCards[InitCardId] as BankCard;
        }

        protected void rbCard1_CheckedChanged(object sender, EventArgs e)
        {
            LoadSelectBankCard(1);
        }

        protected void rbCard2_CheckedChanged(object sender, EventArgs e)
        {
            LoadSelectBankCard(2);
        }

        protected void rbCard3_CheckedChanged(object sender, EventArgs e)
        {
            LoadSelectBankCard(3);
        }

        protected void rbCard4_CheckedChanged(object sender, EventArgs e)
        {
            LoadSelectBankCard(4);
        }

        private void LoadSelectBankCard(int id)
        {
            LoadBankName(CardUsage, ViewState["Bank" + id].ToString());
            LoadCardName(CardUsage, ViewState["Bank" + id].ToString(), ViewState["Card" + id].ToString());
        }
    }
}