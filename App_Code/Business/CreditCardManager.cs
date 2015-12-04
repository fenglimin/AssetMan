using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Constants;
using DataAccess;
using Entities;
using Translation;
using UI;

namespace Business
{
	/// <summary>
	/// Summary description for CreditCardManager
	/// </summary>
	public static class CreditCardManager
	{
		//private static DateTime GetBillDayOfThisMonth(Hashtable allCards, int cardId)
		//{
		//	var bankCard = (BankCard)allCards[cardId];

		//	var billDayNumber = Convert.ToInt32(bankCard.BillDay);
		//	var today = DateTime.Today;
		//	return today.AddDays(billDayNumber - today.Day);
		//}

		//private static DateTime GetPayDayOfThisMonth(Hashtable allCards, int cardId)
		//{
		//	var bankCard = (BankCard)allCards[cardId];

		//	DateTime payDay;
		//	if (bankCard.PayDay.StartsWith("+"))
		//	{
		//		var period = Convert.ToInt32(bankCard.PayDay.Trim(new char[] {'+'}));
		//		payDay = GetBillDayOfThisMonth(allCards, cardId).AddDays(period);
		//		payDay = payDay.AddMonths(DateTime.Today.Month - payDay.Month);
		//	}
		//	else
		//	{
		//		var payDayNumber = Convert.ToInt32(bankCard.PayDay);
		//		var today = DateTime.Today;
		//		payDay = today.AddDays(payDayNumber - today.Day);
		//	}

		//	return payDay;
		//}

		//public static void GetCreditCardStatus(int cardId)
		//{
		//	var allCards = BankCardDal.AllCards;
		//	var billDayThisMonth = GetBillDayOfThisMonth(allCards, cardId);
		//	var payDayThisMonth = GetPayDayOfThisMonth(allCards, cardId);

		//	var today = DateTime.Today;
		//	if (billDayThisMonth.Day < payDayThisMonth.Day)
		//	{
		//		// 账单日 < 还款日

		//		if (today.Day < billDayThisMonth.Day)
		//		{
		//			// 	
		//		}
		//	}


		//}

		//private static void CheckLatestBill(int cardId, DateTime billDayThisMonth, int monthShift)
		//{
		//	var billDayChecked = billDayThisMonth.AddMonths(monthShift).ToString("yyyy-MM-dd");

		//	// 最近一个账单日的欠款，就是最近已出账单的欠款，为负数
		//	var account = AssetDetailManager.GetCardAccountByDate(cardId, billDayChecked);

		//	// 最近一个账单日到今日的还款总额
		//	int totalIn, totalOut;
		//	var condition = string.Format( "(ActionType = '转入' OR ActionType = '收入') AND (AvailDate > DATEVALUE('{0}') AND AvailDate <= DATEVALUE('{1}'))",
		//			billDayChecked, DateTime.Today.ToString("yyyy-MM-dd"));
		//	DayDetailDal.QueryDayDetail(condition, out totalIn, out totalOut);

		//	if (account+totalIn < 0)
		//}

		private static DateTime GetBillDayOfThisMonth(BankCard bankCard)
		{
			var billDayNumber = Convert.ToInt32(bankCard.BillDay);
			var today = DateTime.Today;
			return today.AddDays(billDayNumber - today.Day);
		}

		private static DateTime GetPayDayOfTheBill(BankCard bankCard, DateTime billDay)
		{
			DateTime payDay;
			if (bankCard.PayDay.StartsWith("+"))
			{
				var period = Convert.ToInt32(bankCard.PayDay.Trim(new char[] { '+' }));
				payDay = billDay.AddDays(period);
			}
			else
			{
				var payDayNumber = Convert.ToInt32(bankCard.PayDay);
				payDay = billDay.AddDays(payDayNumber - billDay.Day);
				if (payDay < billDay)
					payDay = payDay.AddMonths(1);
			}

			return payDay;
		}

		public static Dictionary<string, string> GetCreditCardStatus_BillPublish(BankCard bankCard, bool ignorePaidBill)
		{
			var today = DateTime.Today;
			var latestBillDay = GetBillDayOfThisMonth(bankCard);
			if (today < latestBillDay)
				latestBillDay = latestBillDay.AddMonths(-1);
			
			// 最近一个账单日的可用额度
			var account = AssetDetailManager.GetCardAccountByDate(bankCard.CardId, latestBillDay.ToString("yyyy-MM-dd"));
			if (account == 0) return null;

			var rowData = new Dictionary<string, string>();
			rowData[TableFieldName.Id] = bankCard.CardId.ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.BankName] = bankCard.BankName;
			rowData[TableFieldName.CardName] = bankCard.CardName;
			rowData[TableFieldName.BillPeriod] = latestBillDay.AddMonths(-1).AddDays(1).ToString("yyyy-MM-dd") + " ~ " +
			                                     latestBillDay.ToString("yyyy-MM-dd");

			// 最近一个账单日到今日的还款总额
			int totalIn, totalOut;
			var condition = string.Format("BankName='{0}' AND CardName='{1}' AND (ActionType = '转入' OR ActionType = '收入') AND " +
			                              "(AvailDate > DATEVALUE('{2}') AND AvailDate <= DATEVALUE('{3}'))",
					bankCard.BankName, bankCard.CardName, latestBillDay.ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd"));
			DayDetailDal.QueryDayDetail(condition, out totalIn, out totalOut);

			var needPay = account + totalIn;
			var needPayText = string.Format("{0}", -needPay);
			if (needPay >= 0)
			{
				// 账单已还清
				if (ignorePaidBill) return null;
				needPayText = "已还清";
			}

			var payDay = GetPayDayOfTheBill(bankCard, latestBillDay);

			rowData[TableFieldName.BillAccount] = string.Format("{0}", - account); 
			rowData[TableFieldName.BillPayDay] = payDay.ToString("yyyy-MM-dd");
			rowData[TableFieldName.BillPaid] = string.Format("{0}", totalIn);
			rowData[TableFieldName.BillNotPaid] = needPayText;

			return rowData;
		}

		public static Dictionary<string, string> GetCreditCardStatus_BillNotPublish(BankCard bankCard)
		{
			var today = DateTime.Today;
			var latestBillDay = GetBillDayOfThisMonth(bankCard);
			if (today < latestBillDay)
				latestBillDay = latestBillDay.AddMonths(-1);

			var payDay = GetPayDayOfTheBill(bankCard, latestBillDay.AddMonths(1));

			var rowData = new Dictionary<string, string>();
			rowData[TableFieldName.Id] = bankCard.CardId.ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.BankName] = bankCard.BankName;
			rowData[TableFieldName.CardName] = bankCard.CardName;
			rowData[TableFieldName.CreditAll] = bankCard.Credit.ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.BillPeriod] = latestBillDay.AddDays(1).ToString("yyyy-MM-dd") + " ~ 今天";


			// 最近一个账单日到今日的欠款总额
			int totalIn, totalOut;
			var condition = string.Format("BankName='{0}' AND CardName='{1}' AND (ActionType = '转出' OR ActionType = '支出') AND " +
										  "(AvailDate > DATEVALUE('{2}') AND AvailDate <= DATEVALUE('{3}'))",
					bankCard.BankName, bankCard.CardName, latestBillDay.ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd"));
			DayDetailDal.QueryDayDetail(condition, out totalIn, out totalOut);


			rowData[TableFieldName.BillAccount] = string.Format("{0}", totalOut);
			rowData[TableFieldName.BillPayDay] = payDay.ToString("yyyy-MM-dd");
			rowData[TableFieldName.CreditAvailable] = string.Format("{0}", bankCard.Account);

			return rowData;
		}

		public static DataTable CreateDateTable_BillPublish(bool ignorePaidBill)
		{
			var dt = new DataTable();
			dt.Columns.Add(TableFieldName.Id);
			dt.Columns.Add(TableFieldName.BankName);
			dt.Columns.Add(TableFieldName.CardName);
			dt.Columns.Add(TableFieldName.BillPeriod);
			dt.Columns.Add(TableFieldName.BillAccount);
			dt.Columns.Add(TableFieldName.BillPayDay);
			dt.Columns.Add(TableFieldName.BillPaid);
			dt.Columns.Add(TableFieldName.BillNotPaid);

			var allCards = BankCardDal.GetAllAvailableCards();
			foreach (DictionaryEntry item in allCards)
			{
				var bankCard = item.Value as BankCard;
				if (bankCard == null) continue;
				if (bankCard.Credit == 0) continue;

				// todo： 需考虑账单日的活动属于上一账单或本账单
				GridViewManager.AddRow(dt, GetCreditCardStatus_BillPublish(bankCard, ignorePaidBill));
			}

			dt.DefaultView.Sort = TableFieldName.BillPayDay;
			return dt.DefaultView.ToTable();
		}

		public static DataTable CreateDateTable_BillNotPublish()
		{
			var dt = new DataTable();
			dt.Columns.Add(TableFieldName.Id);
			dt.Columns.Add(TableFieldName.BankName);
			dt.Columns.Add(TableFieldName.CardName);
			dt.Columns.Add(TableFieldName.CreditAll);
			dt.Columns.Add(TableFieldName.BillPeriod);
			dt.Columns.Add(TableFieldName.BillAccount);
			dt.Columns.Add(TableFieldName.BillPayDay);
			dt.Columns.Add(TableFieldName.CreditAvailable);

			var allCards = BankCardDal.GetAllAvailableCards();
			foreach (DictionaryEntry item in allCards)
			{
				var bankCard = item.Value as BankCard;
				if (bankCard == null) continue;
				if (bankCard.Credit == 0) continue;

				// todo： 需考虑账单日的活动属于上一账单或本账单
				GridViewManager.AddRow(dt, GetCreditCardStatus_BillNotPublish(bankCard));
			}

			dt.DefaultView.Sort = TableFieldName.BillPayDay;
			return dt.DefaultView.ToTable();
		}
	}
}