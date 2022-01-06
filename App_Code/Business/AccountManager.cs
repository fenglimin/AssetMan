using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using System.Workflow.Activities;
using Constants;
using Entities;
using UI;
using System;
using DataAccess;
using Translation;

namespace Business
{
	/// <summary>
	/// Summary description for AccountManager
	/// </summary>
	public static class AccountManager
	{
		public static string CreateAmountConditionString(int minAmount)
		{
			return string.Format(" AND ( Account >= {0} OR Account <= -{1} )", minAmount, minAmount);
		}

		public static string CreateActionTypeConditionString(IList<string> typeListSelected, IList<string> typeListUnselected)
	    {
			if (typeListSelected != null && typeListSelected.Count != 0)
            {
				var typeCondition = typeListSelected.Aggregate(string.Empty, (current, actionType) => current + ("ActionType = '" + actionType + "' OR "));
                return " AND (" + typeCondition.Substring(0, typeCondition.Length - 4) + ")";
            }

			if (typeListUnselected != null && typeListUnselected.Count != 0)
            {
				var typeCondition = typeListUnselected.Aggregate(string.Empty, (current, actionType) => current + ("ActionType <> '" + actionType + "' AND "));
                return " AND (" + typeCondition.Substring(0, typeCondition.Length - 5) + ")";
            }

	        return string.Empty;
	    }

		public static string CreateDescConditionString(string descriptions)
		{
			if (string.IsNullOrEmpty(descriptions))
				return string.Empty;

			var list = descriptions.Split(' ');
			var descCondition = list.Where(desc => !string.IsNullOrEmpty(desc)).Aggregate(string.Empty, (current, desc) => current + ("Description LIKE '%" + desc + "%' OR "));
			return string.IsNullOrEmpty(descCondition)? string.Empty : " AND (" + descCondition.Substring(0, descCondition.Length - 4) + ")";
		}

		public static DataTable CreateDataTableForAllYearStatistics()
		{
			var dt = new DataTable();
			dt.Columns.Add(TableFieldName.Date);
			dt.Columns.Add(TableFieldName.Income);
			dt.Columns.Add(TableFieldName.Outcome);
			dt.Columns.Add(TableFieldName.Surplus);

			var firstDate = SettingDal.GetStringValues("启用时间")[0];
			firstDate = firstDate.Substring(0, 4) + "-01-01";

			DateTime dtStart;
			DateTime.TryParse(firstDate, out dtStart);

			var historyTotalin = 0;
			var historyTotalOut = 0;

			while (dtStart < DateTime.Now)
			{
				var dtEnd = dtStart.AddYears(1).AddDays(-1);
				var rowData = CreateRowDataForStatistics(dtStart.ToString("yyyy-MM-dd"), dtEnd.ToString("yyyy-MM-dd"));
				rowData[TableFieldName.Date] = dtStart.ToString("yyyy");
				dtStart = dtEnd.AddDays(1);

				historyTotalin += Convert.ToInt32(rowData[TableFieldName.Income]);
				historyTotalOut += Convert.ToInt32(rowData[TableFieldName.Outcome]);

				GridViewManager.AddRow(dt, rowData);
			}

			var rowDataAll = new Dictionary<string, string>();
			rowDataAll[TableFieldName.Date] = "汇总";
			rowDataAll[TableFieldName.Income] = historyTotalin.ToString(CultureInfo.InvariantCulture);
			rowDataAll[TableFieldName.Outcome] = historyTotalOut.ToString(CultureInfo.InvariantCulture);
			rowDataAll[TableFieldName.Surplus] = (historyTotalin + historyTotalOut).ToString(CultureInfo.InvariantCulture);

			GridViewManager.AddRowToTop(dt, rowDataAll);

			return dt;
		}

		public static DataTable CreateDataTableForOneYearStatistics(string year)
		{
			var dt = new DataTable();
			dt.Columns.Add(TableFieldName.Date);
			dt.Columns.Add(TableFieldName.Income);
			dt.Columns.Add(TableFieldName.Outcome);
			dt.Columns.Add(TableFieldName.Surplus);

			DateTime dtStart;
			DateTime.TryParse(year + "-01-01", out dtStart);
			var yearTotalIn = 0;
			var yearTotalOut = 0;
			for (var i = 0; i < 12; i++)
			{
				var dtEnd = dtStart.AddMonths(1).AddDays(-1);
				var rowData = CreateRowDataForStatistics(dtStart.ToString("yyyy-MM-dd"), dtEnd.ToString("yyyy-MM-dd"));
				rowData[TableFieldName.Date] = dtStart.ToString("yyyy-MM");
				dtStart = dtEnd.AddDays(1);

				yearTotalIn += Convert.ToInt32(rowData[TableFieldName.Income]);
				yearTotalOut += Convert.ToInt32(rowData[TableFieldName.Outcome]);

				GridViewManager.AddRow(dt, rowData);
			}

			var rowDataAll = new Dictionary<string, string>();
			rowDataAll[TableFieldName.Date] = "汇总";
			rowDataAll[TableFieldName.Income] = yearTotalIn.ToString(CultureInfo.InvariantCulture);
			rowDataAll[TableFieldName.Outcome] = yearTotalOut.ToString(CultureInfo.InvariantCulture);
			rowDataAll[TableFieldName.Surplus] = (yearTotalIn + yearTotalOut).ToString(CultureInfo.InvariantCulture);

			GridViewManager.AddRowToTop(dt, rowDataAll);

			return dt;
		}

		private static Dictionary<string, string> CreateRowDataForStatistics(string startDate, string endDate)
		{
			int totalIn, totalOut;
			var condition = string.Format(" AvailDate >= DATEVALUE('{0}') AND AvailDate <= DATEVALUE('{1}')", startDate, endDate);
			condition += CreateActionTypeConditionString(new List<string>{"收入", "支出"}, null);
			DayDetailDal.QueryDayDetail(condition, out totalIn, out totalOut);

			var rowData = new Dictionary<string, string>();

			rowData[TableFieldName.Income] = totalIn.ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.Outcome] = totalOut.ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.Surplus] = (totalIn+totalOut).ToString(CultureInfo.InvariantCulture);

			return rowData;
		}

		public static DataTable CreateDateTableFromMoneyInOutDetailQuery(string startDate, string endDate, 
			IList<string> typeListSelected, IList<string> typeListUnselected, string keyword, int minAmount)
        {
            var dt = new DataTable();
            dt.Columns.Add(TableFieldName.Date);
            dt.Columns.Add(TableFieldName.BankName);
            dt.Columns.Add(TableFieldName.CardName);
            dt.Columns.Add(TableFieldName.Income);
            dt.Columns.Add(TableFieldName.Outcome);
            dt.Columns.Add(TableFieldName.Description);


            int totalIn, totalOut;
            var condition = string.Format(
                    " AvailDate >= DATEVALUE('{0}') AND AvailDate <= DATEVALUE('{1}')", startDate, endDate);

			condition += CreateActionTypeConditionString(typeListSelected, typeListUnselected);
			condition += CreateDescConditionString(keyword);
			condition += CreateAmountConditionString(minAmount);
            var dayDetails = DayDetailDal.QueryDayDetail(condition, out totalIn, out totalOut);

            var queryStatistic = CreateStatisticRowDataForMoneyInOutDetail(dayDetails.Count, totalIn, totalOut);
            GridViewManager.AddRow(dt, queryStatistic);

            foreach (var dayDetail in dayDetails)
            {
                GridViewManager.AddRow(dt, CreateRowDataForMoneyInOutDetail(dayDetail));
            }

            return dt;
        }

		public static DataTable CreateDateTableFromBankCardDetailQuery(string bankName, string cardName, string startDate, string endDate,
			IList<string> typeListSelected, IList<string> typeListUnselected, string keyword, int minAmount)
		{
			var dt = new DataTable();
			dt.Columns.Add(TableFieldName.Date);
			dt.Columns.Add(TableFieldName.Type);
			dt.Columns.Add(TableFieldName.Balance);
			dt.Columns.Add(TableFieldName.Description);


			int totalIn, totalOut;
			var condition = string.Format(
					" BankName='{0}' AND CardName='{1}' AND AvailDate >= DATEVALUE('{2}') AND AvailDate <= DATEVALUE('{3}')", 
					bankName, cardName, startDate, endDate);

	        condition += CreateActionTypeConditionString(typeListSelected, typeListUnselected);
			condition += CreateDescConditionString(keyword);
			condition += CreateAmountConditionString(minAmount);
			var dayDetails = DayDetailDal.QueryDayDetail(condition, out totalIn, out totalOut);
			var queryStatistic = CreateStatisticRowDataForBankCardDetail(dayDetails.Count, totalIn, totalOut);
			GridViewManager.AddRow(dt, queryStatistic);

			foreach (var dayDetail in dayDetails)
			{
				GridViewManager.AddRow(dt, CreateRowDataForBankCardDetail(dayDetail));
			}

			return dt;
		}

		public static DataTable CreateDateTableForDayOperations(string condition)
		{
			var dt = new DataTable();
			dt.Columns.Add(TableFieldName.Id);
			dt.Columns.Add(TableFieldName.Date);
			dt.Columns.Add(TableFieldName.BankName);
			dt.Columns.Add(TableFieldName.CardName);
			dt.Columns.Add(TableFieldName.Type);
			dt.Columns.Add(TableFieldName.Balance);
			dt.Columns.Add(TableFieldName.Description);

			int totalIncome, totalOutcome;
			var dayDetails = DayDetailDal.QueryDayDetail(condition, out totalIncome, out totalOutcome);

			foreach (var dayDetail in dayDetails)
			{
				GridViewManager.AddRow(dt, CreateRowDataForDayOperations(dayDetail));
			}

			return dt;
		}

		public static DataTable CreateDateTableFromMonthDetailQuery(DateTime month, bool createStatistic)
		{
			var dt = new DataTable();
			dt.Columns.Add(TableFieldName.Date);
            dt.Columns.Add(TableFieldName.BankName);
            dt.Columns.Add(TableFieldName.CardName);
			dt.Columns.Add(TableFieldName.Income);
			dt.Columns.Add(TableFieldName.Outcome);
			dt.Columns.Add(TableFieldName.Description);

            month = month.AddDays(1 - month.Day);
            var fromDate = month.ToString("yyyy-MM-dd");
            month = month.AddMonths(1).AddDays(-1);
            var toDate = month.ToString("yyyy-MM-dd");

			int totalIncome, totalOutcome;
            var dayDetails = DayDetailDal.QueryDayDetail(fromDate, toDate, out totalIncome, out totalOutcome);

			if (createStatistic)
			{
				var monthStatistic = CreateStatisticRowDataForMoneyInOutDetail(dayDetails.Count, totalIncome, totalOutcome);
				GridViewManager.AddRow(dt, monthStatistic);
			}

            foreach(var dayDetail in dayDetails)
            {
                GridViewManager.AddRow(dt, CreateRowDataForMoneyInOutDetail(dayDetail));
            }

			return dt;
		}

		private static Dictionary<string, string> CreateRowDataForDayOperations(DayDetail dayDetail)
		{
			var rowData = new Dictionary<string, string>();

			rowData[TableFieldName.Id] = dayDetail.Id.ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.Date] = dayDetail.AvailDate;
			rowData[TableFieldName.BankName] = dayDetail.BankName;
			rowData[TableFieldName.CardName] = dayDetail.CardName;
			rowData[TableFieldName.Type] = dayDetail.ActionType;
			rowData[TableFieldName.Balance] = dayDetail.Account;
			rowData[TableFieldName.Description] = dayDetail.Desc;


			return rowData;
		}

		private static Dictionary<string, string> CreateRowDataForMoneyInOutDetail(DayDetail dayDetail)
		{
			var rowData = new Dictionary<string, string>();

			rowData[TableFieldName.Date] = dayDetail.AvailDate;
            rowData[TableFieldName.BankName] = dayDetail.BankName;
            rowData[TableFieldName.CardName] = dayDetail.CardName;
            if (dayDetail.ActionType == TranslationManager.Translate(TableFieldName.Income))
                rowData[TableFieldName.Income] = dayDetail.Account;
            if (dayDetail.ActionType == TranslationManager.Translate(TableFieldName.Outcome))
                rowData[TableFieldName.Outcome] = dayDetail.Account;
			rowData[TableFieldName.Description] = dayDetail.Desc;


			return rowData;
		}

		private static Dictionary<string, string> CreateRowDataForBankCardDetail(DayDetail dayDetail)
		{
			var rowData = new Dictionary<string, string>();

			rowData[TableFieldName.Date] = dayDetail.AvailDate;
			rowData[TableFieldName.Type] = dayDetail.ActionType;
			rowData[TableFieldName.Balance] = dayDetail.Account;
			rowData[TableFieldName.Description] = dayDetail.Desc;

			return rowData;
		}

		private static Dictionary<string, string> CreateStatisticRowDataForMoneyInOutDetail(int rowCount, int totalIncome, int totalOutcome)
		{
			var rowData = new Dictionary<string, string>();

			rowData[TableFieldName.Date] = rowCount + " 条记录";
			//rowData[TableFieldName.BankName] = string.Empty;
			//rowData[TableFieldName.CardName] = string.Empty;
			rowData[TableFieldName.BankName] = totalIncome.ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.CardName] = totalOutcome.ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.Income] = string.Format("结余： {0}", totalIncome + totalOutcome);

			rowData[TableFieldName.Description] = (totalIncome + totalOutcome)>0 ? "1" : "0";

			return rowData;
		}

		private static Dictionary<string, string> CreateStatisticRowDataForBankCardDetail(int rowCount, int totalIncome, int totalOutcome)
		{
			var rowData = new Dictionary<string, string>();

			rowData[TableFieldName.Date] = rowCount + " 条记录";
            rowData[TableFieldName.Type] = string.Empty;
			rowData[TableFieldName.Balance] = string.Format("{0}", totalIncome + totalOutcome);
			rowData[TableFieldName.Description] = string.Format("总入账：{0}, 总出账：{1}", totalIncome, totalOutcome);
			return rowData;
		}
	}
}