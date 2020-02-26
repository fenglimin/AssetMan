using System;
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
			var strSql = string.Format("SELECT * from Investment {0} order by InvestEndDate Desc", condition);
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

        public static void PurchaseFund(string fundName, int amount, double netWorth, string operationDate)
        {
            var fundList = LoadFundList("where FundName ='" + fundName + "'");
            if (fundList.Count == 0)
            {
                InsertFund(fundName, amount, netWorth);
                fundList = LoadFundList("where FundName ='" + fundName + "'");
            }

            var share = Math.Round(amount / netWorth, 2);
            var fundDetail = new FundDetail
            {
                FundId = fundList[0].FundId,
                Amount = amount,
                NetWorth = netWorth,
                OperationDate = operationDate,
                TotalShare = share,
                AvailableShare = share
            };

            InsertFundDetail(fundDetail);
            CalculateFund(fundList[0].FundId, netWorth);
        }

        private static void CalculateFund(int fundId, double netWorth)
        {
            var strWhere = string.Format("WHERE FundID = {0} AND Type = '申购'", fundId);
            var fundDetailList = LoadFundDetailList(strWhere);

            var totalAmount = 0;
            var totalShare = 0.00;
            var totalBenefit = 0.00;

            for (var i = 0; i < fundDetailList.Count; i++)
            {
                var detail = fundDetailList[i];
                totalAmount += detail.Amount;
                totalShare += detail.AvailableShare;
                totalBenefit += (netWorth - detail.NetWorth) * detail.AvailableShare;
            }

            var strSql = string.Format( "UPDATE Fund SET TotalAmount ={0}, TotalShare = {1}, CurrentNetWorth = {2}, TotalBenefit = {3} WHERE FundID = {4}",
                totalAmount, totalShare, netWorth, totalBenefit, fundId);
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            comm.ExecuteNonQuery();
        }

        public static IList<FundInfo> LoadFundList(string condition)
        {
            var strSql = string.Format("SELECT * from Fund {0} order by FundId", condition);
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            var reader = comm.ExecuteReader();
            if (reader == null)
                return null;

            var list = new List<FundInfo>();
            while (reader.Read())
            {
                list.Add(FillFund(reader));
            }
            reader.Close();

            return list;
        }

        public static IList<FundDetail> LoadFundDetailList(string condition)
        {
            var strSql = string.Format("SELECT * from FundDetail {0} order by OperationDate", condition);
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            var reader = comm.ExecuteReader();
            if (reader == null)
                return null;

            var list = new List<FundDetail>();
            while (reader.Read())
            {
                list.Add(FillFundDetail(reader));
            }
            reader.Close();

            return list;
        }

        private static FundInfo FillFund(OleDbDataReader reader)
        {
            var fundInfo = new FundInfo
            {
                FundId = reader.GetInt32(0),
                FundName = Common.GetSafeString(reader, 1),
                TotalAmount = reader.GetInt32(2),
                TotalShare = reader.GetDouble(3),
                CurrentNetWorth = reader.GetDouble(4),
                TotalBenefit= reader.GetInt32(5),
                WeightedBenefitRate = Common.GetSafeString(reader, 6)
            };

            return fundInfo;
        }

        private static FundDetail FillFundDetail(OleDbDataReader reader)
        {
            var fundDetail = new FundDetail
            {
                FundId = reader.GetInt32(0),
                OperationDate = Common.GetSafeDateTime(reader, 1),
                Type = Common.GetSafeString(reader, 2),
                Amount = reader.GetInt32(3),
                NetWorth = reader.GetDouble(4),
                TotalShare = reader.GetDouble(5),
                AvailableShare = reader.GetDouble(6)
            };

            return fundDetail;
        }

        public static void InsertFund(string fundName, int amount, double netWorth)
        {
            var strSql = "INSERT INTO Fund ( FundName, TotalAmount, TotalShare, CurrentNetWorth, TotalBenefit, WeightedBenefitRate ) VALUES ( " +
                         "'" + fundName + "', " +
                         amount + ", " +
                         Math.Round(amount/netWorth, 2) + ", " +
                         netWorth + ", 0, '' )";
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            comm.ExecuteNonQuery();
        }

        public static void InsertFundDetail(FundDetail fundDetail)
        {
            var strSql = "INSERT INTO FundDetail ( FundID, OperationDate, Type, Amount, TotalShare, NetWorth, AvailableShare ) VALUES ( " +
                         fundDetail.FundId + ", " +
                         "DATEVALUE('" + fundDetail.OperationDate+ "'), '申购', " +
                         fundDetail.Amount + ", " +
                         fundDetail.TotalShare + ", " +
                         fundDetail.NetWorth + ", " +
                         fundDetail.AvailableShare + " )";
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            comm.ExecuteNonQuery();
        }
    }
}

