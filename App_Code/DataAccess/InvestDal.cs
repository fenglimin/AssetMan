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

			return defaultInvestBankCard.Account - used - GetFundTotalAmount();
		}

        public static int GetFundTotalAmount()
        {
            var fundList = LoadFundList("");
            var totalAmount = fundList.Sum(fund => fund.TotalAmount);
            return (int)Math.Round(totalAmount);
        }

        public static void PurchaseFund(string fundName, double amount, double netWorth, double tradeFeeRate, string operationDate)
        {
            var fundList = LoadFundList("where FundName ='" + fundName + "'");
            if (fundList.Count == 0)
            {
                InsertFund(fundName, amount, netWorth, operationDate);
                fundList = LoadFundList("where FundName ='" + fundName + "'");
            }

            var tradeFee = amount * tradeFeeRate / 100;
            var share = (amount - tradeFee) / netWorth;

            var fundDetail = new FundDetail
            {
                FundId = fundList[0].FundId,
                Amount = amount,
                NetWorth = netWorth,
                OperationDate = operationDate,
                TotalShare = share,
                AvailableShare = share,
                TradeFee = tradeFee,
                Type = "申购"
            };

            InsertFundDetail(fundDetail);
            CalculateFund(fundList[0].FundId, netWorth, operationDate);
        }

        public static bool RedemptionFund(string fundName, double share, double netWorth, string operationDate, out double totalAmount, out double totalBenefit, out double weightedBenefitRate)
        {
            // 赎回份额对应的本金
            totalAmount = 0.00;
            // 赎回份额产生的收益
            totalBenefit = 0.00;
            // 所有赎回本金产生的加权收益率
            weightedBenefitRate = 0.000;

            var totalShare = share;

            var fundList = LoadFundList("where FundName ='" + fundName + "'");
            if (fundList.Count != 1)
            {
                return false;
            }

            var strWhere = string.Format("WHERE FundID = {0} AND Type = '申购' AND AvailableShare <> 0", fundList[0].FundId);
            var fundDetailList = LoadFundDetailList(strWhere);

            var weightedBenefitData = new Dictionary<double, double>();
            DateTime dtNetWorth;
            DateTime.TryParse(operationDate, out dtNetWorth);

            for (var i = 0; i < fundDetailList.Count; i++)
            {
                var detail = fundDetailList[i];

                DateTime dtPurchase;
                DateTime.TryParse(detail.OperationDate, out dtPurchase);
                var days = (dtNetWorth - dtPurchase).Days;
                

                if (detail.AvailableShare > share)
                {
                    detail.AvailableShare -= share; // 赎回后，本次申购的剩余份额
                    if (detail.AvailableShare < 0.001)
                    {
                        detail.AvailableShare = 0;
                    }
                    totalAmount += share * detail.NetWorth;
                    totalBenefit += share * (netWorth - detail.NetWorth);
                    UpdateFundDetailAvailableShare(detail.Id, detail.AvailableShare, detail.AvailableShare * detail.NetWorth);
                    weightedBenefitData[days * share * detail.NetWorth] = detail.BenefitRate;
                    break;
                }

                totalAmount += (int)(detail.AvailableShare * detail.NetWorth);
                totalBenefit += detail.AvailableShare * (netWorth - detail.NetWorth);

                share -= detail.AvailableShare;
                detail.AvailableShare = 0;
                UpdateFundDetailAvailableShare(detail.Id, 0, 0);//本次申购的份额已全部赎回
                weightedBenefitData[days * detail.Amount] = detail.BenefitRate;
            }

            var weightedBase = weightedBenefitData.Sum(data => data.Key);
            foreach (var data in weightedBenefitData)
            {
                weightedBenefitRate += data.Value * data.Key / weightedBase;
            }

            var fundDetailRedemption = new FundDetail
            {
                FundId = fundList[0].FundId,
                OperationDate = operationDate,
                Type = "赎回",
                Amount = (int) (totalAmount + totalBenefit),
                NetWorth = netWorth,
                TotalShare = totalShare,
                AvailableShare = fundList[0].TotalShare - totalShare, //本次赎回后，剩余的总份额
                BenefitRate = weightedBenefitRate
            };

            InsertFundDetail(fundDetailRedemption);
            CalculateFund(fundList[0].FundId, netWorth, operationDate);

            return true;
        }

        private static void UpdateFundDetailAvailableShare(int id, double availableShare, double amount)
        {
            var strSql = string.Format("UPDATE FundDetail SET Amount = {0}, AvailableShare ={1} WHERE Id = {2}",Math.Round(amount, 3), Math.Round(availableShare,3), id);
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            comm.ExecuteNonQuery();
        }

        public static void CalculateFund(int fundId, double netWorth, string date)
        {
            var strWhere = string.Format("WHERE FundID = {0} AND Type = '申购' AND Amount <> 0", fundId);
            var fundDetailList = LoadFundDetailList(strWhere);

            var totalAmount = 0.00;
            var totalShare = 0.00;
            var totalBenefit = 0.00;
            var totalTradeFee = 0.00;

            DateTime dtNetWorth;
            DateTime.TryParse(date, out dtNetWorth);

            var weightedBase = 0.00;
            var weightedBenefitRate = 0.00;

            for (var i = 0; i < fundDetailList.Count; i++)
            {
                var detail = fundDetailList[i];
                totalAmount += detail.Amount;
                totalShare += detail.AvailableShare;
                totalTradeFee += detail.TradeFee;
                var benefit = (netWorth - detail.NetWorth) * detail.AvailableShare;
                totalBenefit += benefit;
                DateTime dtPurchase;
                DateTime.TryParse(detail.OperationDate, out dtPurchase);
                var days = (dtNetWorth - dtPurchase).Days;
                weightedBase += days * detail.Amount;

                var benefitRate = days == 0? 0 : benefit * 365 / days / detail.Amount;
                UpdateFundDetail(detail.Id, benefitRate);
            }

            for (var i = 0; i < fundDetailList.Count; i++)
            {
                var detail = fundDetailList[i];

                DateTime dtPurchase;
                DateTime.TryParse(detail.OperationDate, out dtPurchase);
                var days = (dtNetWorth - dtPurchase).Days;
                if (days != 0 && detail.Amount > 0.01)
                {
                    var benefitRate = (netWorth - detail.NetWorth) * detail.AvailableShare * 365 / days / detail.Amount;
                    weightedBenefitRate += benefitRate * days * detail.Amount / weightedBase;
                }
            }

            var strSql = string.Format("UPDATE Fund SET TotalAmount ={0}, TotalShare = {1}, CurrentNetWorth = {2}, TotalBenefit = {3}, TotalTradeFee = {4}, WeightedBenefitRate = {5}, CurrentDate = DATEVALUE('{6}') WHERE FundID = {7}",
                Math.Round(totalAmount), Math.Round(totalShare,2), netWorth, Math.Round(totalBenefit, 2), Math.Round(totalTradeFee, 2), Math.Round(weightedBenefitRate, 5) * 100, date, fundId);
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            comm.ExecuteNonQuery();
        }

        private static void UpdateFundDetail(int detailId, double benefitRate)
        {
            var strSql = string.Format("UPDATE FundDetail SET BenefitRate ={0} WHERE Id = {1}", Math.Round(benefitRate, 5) * 100, detailId);
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            comm.ExecuteNonQuery();
        }

        public static IList<FundInfo> LoadFundList(string condition)
        {
            var strSql = string.Format("SELECT * from Fund {0} order by TotalAmount DESC", condition);
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
            var strSql = string.Format("SELECT * from FundDetail {0} order by Id", condition);
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
                TotalAmount = reader.GetDouble(2),
                TotalShare = reader.GetDouble(3),
                CurrentNetWorth = reader.GetDouble(4),
                TotalBenefit = reader.GetDouble(5),
                WeightedBenefitRate = reader.GetDouble(6),
                CurrentDate = Common.GetSafeDateTime(reader, 7),
                TotalTradeFee = reader.GetDouble(8)
            };

            return fundInfo;
        }

        private static FundDetail FillFundDetail(OleDbDataReader reader)
        {
            var fundDetail = new FundDetail
            {
                Id = reader.GetInt32(0),
                FundId = reader.GetInt32(1),
                OperationDate = Common.GetSafeDateTime(reader, 2),
                Type = Common.GetSafeString(reader, 3),
                Amount = reader.GetDouble(4),
                NetWorth = reader.GetDouble(5),
                TotalShare = reader.GetDouble(6),
                AvailableShare = reader.GetDouble(7),
                BenefitRate = reader.GetDouble(8),
                TradeFee = reader.GetDouble(9)
            };

            return fundDetail;
        }

        public static void InsertFund(string fundName, double amount, double netWorth, string date)
        {
            var strSql = "INSERT INTO Fund ( FundName, TotalAmount, TotalShare, CurrentNetWorth, TotalBenefit, WeightedBenefitRate, CurrentDate, TotalTradeFee ) VALUES ( " +
                         "'" + fundName + "', " +
                         amount + ", " +
                         Math.Round(amount/netWorth, 2) + ", " +
                         netWorth + ", 0, 0, DATEVALUE('" + date + "'), 0 )";
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            comm.ExecuteNonQuery();
        }

        public static void InsertFundDetail(FundDetail fundDetail)
        {
            var strSql = "INSERT INTO FundDetail ( FundID, OperationDate, Type, Amount, TotalShare, NetWorth, AvailableShare, BenefitRate, TradeFee ) VALUES ( " +
                         fundDetail.FundId + ", " +
                         "DATEVALUE('" + fundDetail.OperationDate+ "'), " +
                         "'" + fundDetail.Type + "', " +
                         Math.Round(fundDetail.Amount, 3) + ", " +
                         Math.Round(fundDetail.TotalShare, 3) + ", " +
                         fundDetail.NetWorth + ", " +
                         Math.Round(fundDetail.AvailableShare, 3) + ", " +
                         Math.Round(fundDetail.BenefitRate, 3) + ", " +
                         Math.Round(fundDetail.TradeFee, 3) + " )";
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            comm.ExecuteNonQuery();
        }
    }
}

