using System.Collections.Generic;
using System.Data.OleDb;

namespace DataAccess
{
	/// <summary>
	/// Summary description for SettingDal
	/// </summary>
	public static class SettingDal
	{
		public static IList<string> GetStringValues(string itemType)
		{
			var strSql = string.Format("SELECT ItemValue FROM Setting WHERE ItemType = '{0}'", itemType);
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			var reader = comm.ExecuteReader();
			if (reader == null) return null;

			var setting = new List<string>();
			while (reader.Read())
			{
				setting.Add(Common.GetSafeString(reader, 0));
			}

			reader.Close();
			return setting;
		}

		public static IList<int> GetIntValues(string itemType)
		{
			var strSql = string.Format("SELECT ItemValue FROM Setting WHERE ItemType = '{0}' ORDER BY ID", itemType);
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			var reader = comm.ExecuteReader();
			if (reader == null) return null;

			var setting = new List<int>();
			while (reader.Read())
			{
				setting.Add(Common.SafeConvertToInt(Common.GetSafeString(reader, 0)));
			}

			reader.Close();
			return setting;
		}

        public static bool ExistValue(string itemType, string itemValue)
        {
            var strSql = string.Format("SELECT ItemValue FROM Setting WHERE ItemType = '{0}' AND ItemValue = '{1}'", itemType, itemValue);
            var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
            var reader = comm.ExecuteReader();
            if (reader == null) return false;

            var ret = reader.Read();
            reader.Close();
            return ret;
        }

        public static bool AddSetting(string itemType, string itemValue)
        {
            if (ExistValue(itemType, itemValue))
            {
                return true;

            }

			var strSql = string.Format("INSERT INTO Setting ( ItemType, ItemValue ) VALUES ( '{0}', '{1}' )", itemType, itemValue);
			var comm = new OleDbCommand(strSql, DbManager.OleDbConn);
			return comm.ExecuteNonQuery() == 1;
		}
	}
}