using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using Constants;
using Entities;
using Translation;

namespace DataAccess
{
	/// <summary>
	/// Summary description for DayDetailDal
	/// </summary>
	public static class DayDetailDal
	{
		public static bool AddDayDetail(DayDetail dayDetail)
		{
			var strSql = string.Format(
					"INSERT INTO DayDetail (OperationDate, BankName, CardName, ActionType, Account, AvailDate, Avail, Description) " +
					"VALUES (DATEVALUE('{0}'), '{1}', '{2}', '{3}', {4}, DATEVALUE('{5}'), {6}, '{7}')",
					dayDetail.OperationDate, dayDetail.BankName, dayDetail.CardName, dayDetail.ActionType, dayDetail.Account,
					dayDetail.AvailDate, dayDetail.Avail, dayDetail.Desc);
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			return comm.ExecuteNonQuery() == 1;
		}

		public static bool DeleteDayDetail(string id)
		{
			var strSql = string.Format("DELETE FROM DayDetail WHERE ID={0}", id);
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			return comm.ExecuteNonQuery() == 1;
		}

		public static IList<DayDetail> QueryDayDetail(string fromDate, string toDate, out int totalIncome, out int totalOutcome)
		{
			totalIncome = 0;
			totalOutcome = 0;

			var strSql = string.Format("SELECT * from DayDetail where AvailDate >= DATEVALUE('{0}') and AvailDate <= DATEVALUE('{1}') and " +
				              "(ActionType = '收入' or ActionType = '支出') and Avail = Yes ORDER BY AvailDate DESC, ID DESC", fromDate, toDate);
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			var reader = comm.ExecuteReader();
			if (reader == null) return null;

			var list = new List<DayDetail>();
			while (reader.Read())
			{
				var account = reader.GetInt32(5);
				var actionType = Common.GetSafeString(reader, 4);
				if (actionType == TranslationManager.Translate(TableFieldName.Income))
					totalIncome += account;
				else
					totalOutcome += account;

				var dayDetail = new DayDetail
				{
					BankName = Common.GetSafeString(reader, 2),
					CardName = Common.GetSafeString(reader, 3),
					ActionType = actionType,
					Account = account.ToString(CultureInfo.CurrentCulture),
					AvailDate = reader.GetDateTime(6).ToString("yyyy-MM-dd"),
					Desc = Common.GetSafeString(reader, 8)
				};

				list.Add(dayDetail);
			}

            reader.Close();
			return list;
		}

		public static IList<DayDetail> QueryDayDetail(string condition, out int totalIn, out int totalOut)
		{
			totalIn = 0;
			totalOut = 0;

			var strSql = string.Format("SELECT * from DayDetail WHERE {0} ORDER BY AvailDate, ID", condition);
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			var reader = comm.ExecuteReader();
			if (reader == null) return null;

			var list = new List<DayDetail>();
			while (reader.Read())
			{
				var account = reader.GetInt32(5);
				if (account > 0)
					totalIn += account;
				else
					totalOut += account;

				var dayDetail = new DayDetail
				{
					Id = reader.GetInt32(0),
					BankName = Common.GetSafeString(reader, 2),
					CardName = Common.GetSafeString(reader, 3),
					ActionType = Common.GetSafeString(reader, 4),
					Account = account.ToString(CultureInfo.CurrentCulture),
					AvailDate = reader.GetDateTime(6).ToString("yyyy-MM-dd"),
					Desc = Common.GetSafeString(reader, 8)
				};

				list.Add(dayDetail);
			}

            reader.Close();
			return list;
		}

		public static IList<DayDetail> QueryDayDetailByDate(string date)
		{
			var strSql = string.Format("SELECT BankName, CardName, ActionType, Account from DayDetail where AvailDate = DATEVALUE('{0}') ORDER BY ID DESC", date);
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			var reader = comm.ExecuteReader();
			if (reader == null) return null;

			var list = new List<DayDetail>();
			while (reader.Read())
			{
				var dayDetail = new DayDetail
				{
					BankName = Common.GetSafeString(reader, 0),
					CardName = Common.GetSafeString(reader, 1),
					ActionType = Common.GetSafeString(reader, 2),
					Account = reader.GetInt32(3).ToString(CultureInfo.CurrentCulture)
				};

				list.Add(dayDetail);
			}

            reader.Close();
			return list;
		}
	}
}