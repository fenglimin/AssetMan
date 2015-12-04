using System;
using System.Data.OleDb;
using Entities;

namespace DataAccess
{
	/// <summary>
	/// Summary description for AddOilDal
	/// </summary>
	public static class AddOilDal
	{
		public static void AddOilDetail(AddOilDetail addOilDetail)
		{
			var lastestAddOilDetail = GetLatestAddOilDetail();
			if (lastestAddOilDetail != null)
			{
				lastestAddOilDetail.DrivedMileAge = addOilDetail.CurMileAge - lastestAddOilDetail.CurMileAge;
				double oilUsed = lastestAddOilDetail.OilMeteBeforeAdd + lastestAddOilDetail.OilMeteAdded - addOilDetail.OilMeteBeforeAdd;

				int averageOilUsage = Convert.ToInt32(oilUsed * 10000 / lastestAddOilDetail.DrivedMileAge);
				lastestAddOilDetail.AverageOilUsage = Convert.ToDouble(averageOilUsage) / 100;

				UpdateAddOilDetail(lastestAddOilDetail);
			}

			var strSql = "INSERT INTO AddOil (AddOilDate,CurMileAge,OilFee,OilPrice,OilMeteAdded,OilMeteBeforeAdd,DrivedMileAge,AverageOilUsage,AddOilAddress) VALUES " +
				"(DATEVALUE('" + addOilDetail.AddOilDate + "')," + addOilDetail.CurMileAge + "," + addOilDetail.OilFee + "," + addOilDetail.OilPrice +
				"," + addOilDetail.OilMeteAdded + "," + addOilDetail.OilMeteBeforeAdd + "," + addOilDetail.DrivedMileAge + "," + addOilDetail.AverageOilUsage + ",'" + addOilDetail.AddOilAddress + "')";


			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			comm.ExecuteNonQuery();
		}

		public static AddOilDetail FillAddOilDetail(OleDbDataReader reader)
		{
			var addOilDetail = new AddOilDetail
			{
				AddOilDate = reader.GetDateTime(0).ToString("yyyy-MM-dd"),
				CurMileAge = reader.GetInt32(1),
				OilFee = reader.GetInt32(2),
				OilPrice = reader.GetDouble(3),
				OilMeteAdded = reader.GetDouble(4),
				OilMeteBeforeAdd = reader.GetDouble(5),
				DrivedMileAge = reader.GetInt32(6),
				AverageOilUsage = reader.GetDouble(7),
				AddOilAddress = Common.GetSafeString(reader, 8)
			};

			return addOilDetail;
		}

		public static AddOilDetail GetLatestAddOilDetail()
		{
			const string strSql = "SELECT * from AddOil order by AddOilDate DESC";
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			var reader = comm.ExecuteReader();
			if (reader == null) return null;

			var addOilDetail = reader.Read() ? FillAddOilDetail(reader) : new AddOilDetail();

			reader.Close();
			return addOilDetail;
		}

		public static void UpdateAddOilDetail(AddOilDetail addOilDetail)
		{
			var strSql = "UPDATE AddOil SET " +
				 "CurMileAge = " + addOilDetail.CurMileAge +
				 ", OilFee = " + addOilDetail.OilFee +
				 ", OilPrice = " + addOilDetail.OilPrice +
				 ", OilMeteAdded = " + addOilDetail.OilMeteAdded +
				 ", OilMeteBeforeAdd = " + addOilDetail.OilMeteBeforeAdd +
				 ", DrivedMileAge = " + addOilDetail.DrivedMileAge +
				 ", AverageOilUsage = " + addOilDetail.AverageOilUsage +
				 ", AddOilAddress = '" + addOilDetail.AddOilAddress + "'" +
				 " WHERE AddOilDate = DATEVALUE('" + addOilDetail.AddOilDate + "')";


			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			comm.ExecuteNonQuery();
		}

	}
}