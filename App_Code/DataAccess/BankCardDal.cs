using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using Entities;

namespace DataAccess
{
	/// <summary>
	/// Summary description for BankCardManager
	/// </summary>
	public static class BankCardDal
	{
        /// <summary>
        /// 所有非停用的卡
        /// </summary>
		public static Hashtable GetAllAvailableCards()
        {
			var allCards = new Hashtable();
			const string strSql = "SELECT * from BankCard where CardUsage is null or CardUsage <> '停用'";
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			var reader = comm.ExecuteReader();
			if (reader == null) return null;

			allCards.Clear();
			while (reader.Read())
			{
				var bankCard = new BankCard
				{
					CardId = reader.GetInt32(0),
					BankName = Common.GetSafeString(reader, 1),
					CardName = Common.GetSafeString(reader, 2),
					CardType = Common.GetSafeString(reader, 3),
					CardUsage = Common.GetSafeString(reader, 4),
					Account = reader.GetInt32(5),
					Credit = reader.GetInt32(6),
					MaxPerDay = reader.GetInt32(7),
					BillDay = Common.GetSafeString(reader, 8),
					PayDay = Common.GetSafeString(reader, 9),
                    Alias = Common.GetSafeString(reader, 10)
                };

				allCards[bankCard.CardId] = bankCard;
			}

			reader.Close();
			return allCards;
		}

        /// <summary>
        /// 信用卡有额度或储蓄卡有余额
        /// </summary>
        public static IList<string> AllBankWithAvailableCard
        {
            get
            {
                var allBankWithAvailableCard = new List<string>();
				foreach (DictionaryEntry de in GetAllAvailableCards())
                {
                    var card = de.Value as BankCard;
                    if (card == null) continue;

                    if (!allBankWithAvailableCard.Contains(card.BankName))
                    {
                        allBankWithAvailableCard.Add(card.BankName);
                    }
                }

                return allBankWithAvailableCard;
            }
        }

        public static IList<string> GetBankByCardUsage(string cardUsage)
        {
			var strSql = string.Format("SELECT DISTINCT BankName FROM BankCard WHERE CardUsage LIKE '%{0}%' AND CardUsage <> '停用'", cardUsage);
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            var reader = comm.ExecuteReader();
            if (reader == null) return null;

            var banks = new List<string>();
            while (reader.Read())
            {
                banks.Add(Common.GetSafeString(reader, 0));
            }

            reader.Close();

			banks.Sort();
            return banks;
        }

		public static Hashtable GetCardByBankAndUsage(string bankName, string cardUsage)
        {
			var cards = new Hashtable();
			foreach (DictionaryEntry item in GetAllAvailableCards())
			{
				var bankCard = item.Value as BankCard;
				if (bankCard == null)
					continue;

				if (bankCard.BankName != bankName)
					continue;

				if (!string.IsNullOrEmpty(cardUsage))
					if (!bankCard.CardUsage.Contains(cardUsage))
						continue;

				cards[bankCard.CardId] = bankCard;
			}

			return cards;
        }

		public static BankCard GetCardByBankNameAndCardName(string bankName, string cardName)
		{
			foreach (DictionaryEntry item in GetAllAvailableCards())
			{
				var bankCard = item.Value as BankCard;
				if (bankCard == null)
					continue;

				if (bankCard.BankName == bankName && bankCard.CardName == cardName)
					return bankCard;
			}

			return null;			
		}

		public static bool ProcessDayDetail(int cardId, int accountChanged)
		{
			var bankCard = GetAllAvailableCards()[cardId] as BankCard;
			if (bankCard == null)
				return false;

			var newAccount = bankCard.Account + accountChanged;

			var strSql = string.Format("UPDATE BankCard SET Account = {0} WHERE CardID = {1}", newAccount, bankCard.CardId);
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			return comm.ExecuteNonQuery() == 1;
		}

		public static BankCard GetDefaultInvestBankCard()
		{
			var list = SettingDal.GetStringValues("默认投资账户");
			if (list.Count != 1)
				return null;

			var cardId = Common.SafeConvertToInt(list[0]);
			return GetAllAvailableCards()[cardId] as BankCard;
		}
	}
}