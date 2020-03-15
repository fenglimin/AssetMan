using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Constants;
using DataAccess;
using Entities;
using System;
using UI;

namespace Business
{
	/// <summary>
	/// Summary description for TodayManager
	/// </summary>
	public static class AssetDetailManager
	{
		public static int GetCardAccountByDate(int cardId, string date)
		{
			var assetDetail = AssetDetailDal.LoadAssetDetailByDate(date, true);
            if (assetDetail.bankCardDetail != null)
            {
                var cards = assetDetail.bankCardDetail.Split(';');
                var allCards = BankCardDal.GetAllAvailableCards();

                foreach (var card in cards)
                {
                    if (string.IsNullOrEmpty(card)) continue;

                    var items = card.Split(':');
                    if (Common.SafeConvertToInt(items[0]) == cardId)
                        return Common.SafeConvertToInt(items[1]);
                }
            }
			

			return 0;
		}

        public static DataSet CreateAssetDetailDataSet(string date, bool nearestOld, out string loadedDate)
		{
			var dataSet = new DataSet();
			var assetDetail = string.IsNullOrEmpty(date) ? AssetDetailDal.CreateCurrentAssetDetail() : AssetDetailDal.LoadAssetDetailByDate(date, nearestOld);
		    loadedDate = assetDetail.date;

			dataSet.Tables.Add(CreateDateTableFromAssetDetail(assetDetail));
			dataSet.Tables.Add(CreateDateTableFromAssetDetailBankList(assetDetail));
			dataSet.Tables.Add(CreateDateTableFromAssetDetailDayAction(assetDetail.date));
			
			return dataSet;
		}

		public static DataTable CreateDateTableFromAssetDetailDayAction(string date)
		{
			var dt = new DataTable();
			dt.Columns.Add(TableFieldName.BankName);
			dt.Columns.Add(TableFieldName.CardName);
			dt.Columns.Add(TableFieldName.DayDetailActionType);
			dt.Columns.Add(TableFieldName.Balance);

			var list = DayDetailDal.QueryDayDetailByDate(date);
			foreach (var dayDetail in list)
			{
				GridViewManager.AddRow(dt, CreateRowDataForAssetDetailDayAction(dayDetail));
			}

			return dt;
		}

		private static Dictionary<string, string> CreateRowDataForAssetDetailDayAction(DayDetail dayDetail)
		{
			var rowData = new Dictionary<string, string>();
			rowData[TableFieldName.BankName] = dayDetail.BankName;
			rowData[TableFieldName.CardName] = dayDetail.CardName;
			rowData[TableFieldName.DayDetailActionType] = dayDetail.ActionType;
			rowData[TableFieldName.Balance] = dayDetail.Account;

			return rowData;
		}

		public static DataTable CreateDateTableFromAssetDetail(AssetDetail assetDetail)
		{
			var dt = new DataTable();
			dt.Columns.Add(TableFieldName.Type);
			dt.Columns.Add(TableFieldName.Balance);

			GridViewManager.AddRow(dt, CreateRowDataForAssetDetail("净资产", assetDetail.netAsset));
			GridViewManager.AddRow(dt, CreateRowDataForAssetDetail("可支配资产", assetDetail.allAsset));
			GridViewManager.AddRow(dt, CreateRowDataForAssetDetail("信用卡总欠款", assetDetail.totalTaoXian));
			GridViewManager.AddRow(dt, CreateRowDataForAssetDetail("储蓄卡总余额", assetDetail.bankAcount));
			GridViewManager.AddRow(dt, CreateRowDataForAssetDetail("现金", assetDetail.cash));
			GridViewManager.AddRow(dt, CreateRowDataForAssetDetail("投资", assetDetail.invest));
			GridViewManager.AddRow(dt, CreateRowDataForAssetDetail("支付宝", assetDetail.zfb));
			GridViewManager.AddRow(dt, CreateRowDataForAssetDetail("转账途中", assetDetail.transferOnWay));
			GridViewManager.AddRow(dt, CreateRowDataForAssetDetail("其他", assetDetail.other));

			return dt;
		}

		private static Dictionary<string, string> CreateRowDataForAssetDetail(string type, int balance)
		{
			var rowData = new Dictionary<string, string>();
			rowData[TableFieldName.Type] = type;
			rowData[TableFieldName.Balance] = balance.ToString(CultureInfo.InvariantCulture);

			return rowData;
		}

		public static DataTable CreateDateTableFromAssetDetailBankList(AssetDetail assetDetail)
		{
			if (string.IsNullOrEmpty(assetDetail.bankCardDetail))
				return new DataTable();

			var dt = new DataTable();
			dt.Columns.Add(TableFieldName.BankName);
			dt.Columns.Add(TableFieldName.CardName);
			dt.Columns.Add(TableFieldName.Balance).DataType = typeof(int);

			var cards = assetDetail.bankCardDetail.Split(';');
			var allCards = BankCardDal.GetAllAvailableCards();

			foreach (var card in cards)
			{
				if (string.IsNullOrEmpty(card)) continue;

				var items = card.Split(':');
				var bankId = items[0];
				var account = Common.SafeConvertToInt(items[1]);

				var bankCard = allCards[Convert.ToInt32(bankId)] as BankCard;
				if (bankCard == null)
					continue;

				if (account != 0)
					GridViewManager.AddRow(dt, CreateRowDataForAssetDetailBankList(bankCard.BankName, bankCard.CardName, account));
			}

			var dtCopy = dt.Copy();
			dtCopy.DefaultView.Sort = TableFieldName.Balance + " DESC";

			return dtCopy.DefaultView.ToTable();

			//return dt;
		}

		private static Dictionary<string, string> CreateRowDataForAssetDetailBankList(string bankName, string cardName, int balance)
		{
			var rowData = new Dictionary<string, string>();
			rowData[TableFieldName.BankName] = bankName;
			rowData[TableFieldName.CardName] = cardName;
			rowData[TableFieldName.Balance] = balance.ToString(CultureInfo.InvariantCulture);

			return rowData;
		}

		private static AssetDetail ApplyAccountChange(AssetDetail assetDetail, int cardId, string cardType, int accountChange)
		{
			if (cardType == "支付宝")
			{
				assetDetail.zfb += accountChange;
			}
			else if (cardType == "转账途中")
			{
				assetDetail.transferOnWay += accountChange;
			}
			else if (cardType == "投资")
			{
				assetDetail.invest += accountChange;
			}
			else if (cardType == "现金")
			{
				assetDetail.cash += accountChange;
			}
			else if (cardType == "其他")
			{
				assetDetail.other += accountChange;
			}
			else
			{
				assetDetail.bankAcount += accountChange;

				var cards = assetDetail.bankCardDetail.Split(';');
				var newCards = new List<string>();

				var cardFound = false;
				foreach (var card in cards)
				{
					if (string.IsNullOrEmpty(card)) continue;

					if (cardFound)
					{
						newCards.Add(card);
						continue;
					}

					var items = card.Split(':');
					var checkCardId = Common.SafeConvertToInt(items[0]);
					var checkCardaccount = Common.SafeConvertToInt(items[1]);

					var newCard = card;
					if (checkCardId == cardId)
					{
						newCard = string.Format("{0}:{1}", checkCardId, checkCardaccount + accountChange);
						cardFound = true;
					}

					newCards.Add(newCard);
				}

				if (!cardFound)
				{
					newCards.Add(string.Format("{0}:{1}", cardId, accountChange));
				}
				assetDetail.bankCardDetail = string.Join(";", newCards);
			}

			assetDetail.allAsset = assetDetail.bankAcount + assetDetail.cash + assetDetail.invest + assetDetail.zfb;
			assetDetail.netAsset = assetDetail.allAsset - assetDetail.totalTaoXian + assetDetail.transferOnWay + assetDetail.other;

			return assetDetail;
		}

		public static void AddDayDetail(int cardId, DayDetail dayDetail)
		{
			DayDetailDal.AddDayDetail(dayDetail);

			var accountChanged = Common.SafeConvertToInt(dayDetail.Account);
			FixHistoryData(cardId, accountChanged,dayDetail.AvailDate);
		}

		public static void DeleteDayDetail(string id, int cardId, int accountChanged, string dateChanged)
		{
			DayDetailDal.DeleteDayDetail(id);
			FixHistoryData(cardId, accountChanged, dateChanged);
		}

		private static void FixHistoryData(int cardId, int accountChanged, string dateChanged)
		{
			BankCardDal.ProcessDayDetail(cardId, accountChanged);

			var affectedAssetDetailHistory = AssetDetailDal.LoadAssetDetailHistoryByStartDate(dateChanged);
			if (affectedAssetDetailHistory.Count == 0)
				return;

			var allCards = BankCardDal.GetAllAvailableCards();
			var bankCard = allCards[cardId] as BankCard;
			if (bankCard == null)
				return;


			if (affectedAssetDetailHistory[0].date != dateChanged)
			{
				// 当日无记录
				var assetDetail = AssetDetailDal.LoadAssetDetailByDate(dateChanged, true);
				assetDetail = ApplyAccountChange(assetDetail, cardId, bankCard.CardType, accountChanged);
				assetDetail.date = dateChanged;
				AssetDetailDal.InsertAssetDetail(assetDetail);
			}

			foreach (var assetDetail in affectedAssetDetailHistory)
			{
				var changedAssetDetail = ApplyAccountChange(assetDetail, cardId, bankCard.CardType, accountChanged);
				AssetDetailDal.UpdateAssetDetail(changedAssetDetail);
			}
		}

		public static void Test()
		{
			var list = AssetDetailDal.LoadAllAssetDetailHistory();
			var allCards = BankCardDal.GetAllAvailableCards();

			foreach (var assetDetail in list)
			{
				var cards = assetDetail.bankCardDetail.Split(';');
				var newCards = new List<string>();

				foreach (var card in cards)
				{
					if (string.IsNullOrEmpty(card)) continue;

					var items = card.Split(':');
					var bankId = items[0];
					var account = Common.SafeConvertToInt(items[1]);

					var bankCard = allCards[Convert.ToInt32(bankId)] as BankCard;
					if (bankCard == null)
						continue;

					var newCard = card;
					if (bankCard.Credit != 0)
					{
						newCard = string.Format("{0}:{1}", bankId, account - bankCard.Credit);
					}

					newCards.Add(newCard);
				}

				assetDetail.bankCardDetail = string.Join(";", newCards);
				AssetDetailDal.UpdateAssetDetail(assetDetail);
			}
		}
	}
}