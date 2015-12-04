using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using Entities;

namespace DataAccess
{
	/// <summary>
	/// Summary description for AssetDetailDal
	/// </summary>
	public static class AssetDetailDal
	{
		private static AssetDetail FillAssetDetail(OleDbDataReader reader)
		{
			var assetDetail = new AssetDetail
			{
				date = reader.GetDateTime(0).ToString("yyyy-MM-dd"),
				allAsset = reader.GetInt32(1),
				netAsset = reader.GetInt32(2),
				bankAcount = reader.GetInt32(3),
				totalTaoXian = reader.GetInt32(4),
				invest = reader.GetInt32(5),
				cash = reader.GetInt32(6),
				zfb = reader.GetInt32(7),
				transferOnWay = reader.GetInt32(8),
				other = reader.GetInt32(9),
				bankCardDetail = Common.GetSafeString(reader, 10)
			};

			return assetDetail;
		}

		public static IList<AssetDetail> LoadAssetDetailHistoryByStartDate(string startDate)
		{
			var condition = string.Format(" WHERE AssetDate >= DATEVALUE('{0}') ORDER BY AssetDate", startDate);
			return LoadAssetDetailHistory(condition);
		}

		public static IList<AssetDetail> LoadAssetDetailHistory(string condition)
		{
			var strSql = "SELECT * from AssetHistory" + condition;
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			var reader = comm.ExecuteReader();
			if (reader == null) return null;

			var list = new List<AssetDetail>();
			while (reader.Read())
			{
				list.Add(FillAssetDetail(reader));
			}

			reader.Close();
			return list;
		}


		public static IList<AssetDetail> LoadAllAssetDetailHistory()
		{
			return LoadAssetDetailHistory(string.Empty);
		}

		public static AssetDetail LoadAssetDetailByDate(string date, bool nearestOld)
		{
			// 如果数据库中不存在当天的记录，则取离当天最近的那一天
		    string strSql = string.Empty;
            if (nearestOld)
		        strSql = "SELECT * from AssetHistory WHERE AssetDate <= DATEVALUE('" + date + "') ORDER BY AssetDate DESC";
            else
                strSql = "SELECT * from AssetHistory WHERE AssetDate >= DATEVALUE('" + date + "') ORDER BY AssetDate";
   
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			var reader = comm.ExecuteReader();
			if (reader == null) return null;

			AssetDetail assetDetail;
			if (reader.Read())
			{
				assetDetail = FillAssetDetail(reader);
			}
			else
			{
				assetDetail = new AssetDetail { date = date };
			}

			reader.Close();
			return assetDetail;
		}

		public static AssetDetail CreateCurrentAssetDetail()
		{
			var assetDetail = new AssetDetail { date = DateTime.Today.ToString("yyyy-MM-dd") };

			foreach (DictionaryEntry item in BankCardDal.GetAllAvailableCards())
			{
				var bankCard = item.Value as BankCard;
				if (bankCard == null) continue;


				if (bankCard.CardType.Contains("信用卡"))
				{
					var temp = bankCard.Account - bankCard.Credit;
					if (temp != 0)
					{
						assetDetail.totalTaoXian += -temp;
						assetDetail.bankCardDetail += bankCard.CardId + ":" + temp + ";";
					}
				}
				else if (bankCard.CardType == "支付宝")
				{
					assetDetail.zfb += bankCard.Account;
				}
				else if (bankCard.CardType == "转账途中")
				{
					assetDetail.transferOnWay += bankCard.Account;
				}
				else if (bankCard.CardType == "投资")
				{
					assetDetail.invest += bankCard.Account;
				}
				else if (bankCard.CardType == "现金")
				{
					assetDetail.cash += bankCard.Account;
				}
				else if (bankCard.CardType == "其他")
				{
					assetDetail.other = bankCard.Account;
				}
				else
				{
					if (bankCard.Account != 0)
					{
						assetDetail.bankAcount += bankCard.Account;
						assetDetail.bankCardDetail += bankCard.CardId + ":" + bankCard.Account + ";";
					}
				}

			}

			assetDetail.allAsset = assetDetail.bankAcount + assetDetail.cash + assetDetail.invest + assetDetail.zfb;
			assetDetail.netAsset = assetDetail.allAsset - assetDetail.totalTaoXian + assetDetail.transferOnWay + assetDetail.other;

			return assetDetail;
		}

		public static void UpdateTodayAssetDetail(string reason)
		{
			var assetDetail = CreateCurrentAssetDetail();

			var strSql = "DELETE FROM AssetHistory WHERE AssetDate = DATEVALUE('" + assetDetail.date + "')";
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			comm.ExecuteNonQuery();

			InsertAssetDetail(assetDetail);

			DbManager.BackupDb(reason);
		}

		public static void InsertAssetDetail(AssetDetail assetDetail)
		{
			var strSql = "INSERT INTO AssetHistory VALUES(DATEVALUE('" + assetDetail.date + "')," +
				assetDetail.allAsset + "," +
				assetDetail.netAsset + "," +
				assetDetail.bankAcount + "," +
				assetDetail.totalTaoXian + "," +
				assetDetail.invest + "," +
				assetDetail.cash + "," +
				assetDetail.zfb + "," +
				assetDetail.transferOnWay + "," +
				assetDetail.other + ",'" +
				assetDetail.bankCardDetail + "')";

			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			comm.ExecuteNonQuery();
		}

		public static void UpdateAssetDetail(AssetDetail assetDetail)
		{
			var strSql = string.Format("UPDATE AssetHistory SET AllAsset={0}, NetAsset={1}, BankAccount={2}, TotalTaoXian={3}, Cash={4}, ZFB={5}, " +
			                           "TransferOnWay={6}, Other={7}, BankCardDetail='{8}' WHERE AssetDate = DATEVALUE('{9}')",
									   assetDetail.allAsset, assetDetail.netAsset, assetDetail.bankAcount, assetDetail.totalTaoXian, assetDetail.cash,
									   assetDetail.zfb, assetDetail.transferOnWay, assetDetail.other, assetDetail.bankCardDetail, assetDetail.date);
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			comm.ExecuteNonQuery();
		}
	}
}