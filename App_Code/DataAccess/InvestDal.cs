using System.Activities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using Entities;

namespace DataAccess
{
	/// <summary>
	/// Summary description for InvestDal
	/// </summary>
	public static class InvestDal
	{
		public static void InsertInvestDetail(InvestDetail investDetail)
		{
			var strSql = "INSERT INTO Investment ( InvestType, InvestName, InvestAmount, InvestStartDate, InvestEndDate, InvestBenifitRate, InvestAvailDate ) VALUES ( " +
				 "'" + investDetail.InvestType + "', " +
				 "'" + investDetail.InvestName + "', " +
				 investDetail.InvestAmount + ", " +
				 "DATEVALUE('" + investDetail.InvestStartDate + "'), " +
				 "DATEVALUE('" + investDetail.InvestEndDate + "'), " +
				 "'" + investDetail.InvestBenifitRate + "%', " +
				 "DATEVALUE('" + investDetail.InvestAvailDate + "') )";


			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			comm.ExecuteNonQuery();
		}

		public static void UpdateInvestDetail(InvestDetail investDetail)
		{
			var strSql = "UPDATE Investment SET " +
				 "InvestType = '" + investDetail.InvestType + "'" +
				 ", InvestName = '" + investDetail.InvestName + "'" +
				 ", InvestAmount = " + investDetail.InvestAmount +
				 ", InvestStartDate = DATEVALUE('" + investDetail.InvestStartDate + "')" +
				 ", InvestEndDate = DATEVALUE('" + investDetail.InvestEndDate + "')" +
				 ", InvestBenifit = " + investDetail.InvestBenifit +
				 ", InvestBenifitRate = '" + investDetail.InvestBenifitRate + "%'" +
				 ", InvestAvailDate = DATEVALUE('" + investDetail.InvestAvailDate + "')" +
				 " WHERE InvestID = " + investDetail.InvestID;


			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			comm.ExecuteNonQuery();
		}

		public static IList<InvestDetail> LoadAllInvestments()
		{
			return LoadInvestments(string.Empty);
		}

		public static IList<InvestDetail> LoadInvestments(string condition)
		{
			var strSql = string.Format("SELECT * from Investment {0}order by InvestEndDate Desc", condition);
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			var reader = comm.ExecuteReader();
			if (reader == null)
				return null;

			var list = new List<InvestDetail>();
			while (reader.Read())
			{
				list.Add(FillInvestDetail(reader));
			}
			reader.Close();

			return list;
		}

		public static InvestDetail FillInvestDetail(OleDbDataReader reader)
		{
			var investDetail = new InvestDetail
			{
				InvestID = reader.GetInt32(0),
				InvestType = Common.GetSafeString(reader, 1),
				InvestName = Common.GetSafeString(reader, 2),
				InvestAmount = reader.GetInt32(3),
				InvestStartDate = Common.GetSafeDateTime(reader, 4),
				InvestEndDate = Common.GetSafeDateTime(reader, 5),
				InvestBenifit = reader.GetInt32(6),
				InvestBenifitRate = Common.GetSafeString(reader, 7),
				InvestAvailDate = Common.GetSafeDateTime(reader, 8)
			};

			return investDetail;
		}

		public static InvestDetail LoadInvestDetailById(string investId)
		{
			var condition = string.Format("WHERE InvestID = {0} ", investId);
			var list = LoadInvestments(condition);
			return list.Count == 1 ? list[0] : null;
		}

		/// <summary>
		/// 获取投资账户内的可用余额
		/// </summary>
		/// <returns></returns>
		public static int GetInvestBalance()
		{
			var list = LoadInvestments("WHERE InvestBenifit = 0 ");
			var used = list.Sum(investDetail => investDetail.InvestAmount);

			var defaultInvestBankCard = BankCardDal.GetDefaultInvestBankCard();
			if (defaultInvestBankCard == null)
				return 0;

			return defaultInvestBankCard.Account - used;
		}
	}
}