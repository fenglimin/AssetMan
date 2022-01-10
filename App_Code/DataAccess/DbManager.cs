using System;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace DataAccess
{
	/// <summary>
	/// Summary description for DbConnManager
	/// </summary>
	public static class DbManager
	{
		/// <summary>
		/// the sync lock.
		/// </summary>
		private static readonly object SyncRoot = new Object();

		/// <summary>
		/// Database connection
		/// </summary>
		private static OleDbConnection _oleDbConn;

		/// <summary>
		/// 
		/// </summary>
		private const string DatabaseConnectionStsring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=.\\DatabaseFile\\Asset.mdb";

		/// <summary>
		/// 
		/// </summary>
		public static OleDbConnection OleDbConn
		{
			get
			{
				if (_oleDbConn == null)
				{
					lock (SyncRoot)
					{
						if (_oleDbConn == null)
						{
							var connStringSetting = ConfigurationManager.ConnectionStrings[1];
							_oleDbConn = new OleDbConnection(connStringSetting.ConnectionString);
							_oleDbConn.Open();
						}
					}
				}

				return _oleDbConn;
			}
		}

		public static int UpdateDb(string strSql, bool backDbFile, string desc)
		{
			var comm = new OleDbCommand(strSql, OleDbConn);
			var ret = comm.ExecuteNonQuery();

			if (ret != 0 && backDbFile)
			{
				var today = DateTime.Now;
				var descFile = string.Format("{0}.{1} {2} -- {3}", OleDbConn.DataSource, today.ToString("yyyy-MM-dd"),
					today.ToString("HH-mm-ss"), desc);

				File.Copy(OleDbConn.DataSource, descFile);
			}

			return ret;
		}

        public static string ReplaceIllegalCharacter(string input)
        {
            input = input.Replace('/', '_');
            input = input.Replace('\\', '_');
            input = input.Replace(':', '_');
            input = input.Replace('*', '_');
            input = input.Replace('?', '_');
            input = input.Replace('\"', '_');
            input = input.Replace('<', '_');
            input = input.Replace('>', '_');
            input = input.Replace('|', '_');

            return input;
        }

        public static void BackupDb(string reason)
		{
			if (string.IsNullOrEmpty(OleDbConn.DataSource))
				return;

			var today = DateTime.Now;
            string fileName;
            reason = ReplaceIllegalCharacter(reason);

            if (reason == "当日首次刷新")
			{
				DeleteOldDbFile();

				fileName = string.Format("{0} {1} -- {2}{3}", Path.GetFileNameWithoutExtension(OleDbConn.DataSource),
					today.ToString("yyyy-MM-dd"), reason, Path.GetExtension(OleDbConn.DataSource));

				if (File.Exists(fileName))
					return;
			}
			else
			{
				fileName = string.Format("{0} {1} {2} -- {3}{4}", Path.GetFileNameWithoutExtension(OleDbConn.DataSource),
					today.ToString("yyyy-MM-dd"), today.ToString("HH-mm-ss"), reason, Path.GetExtension(OleDbConn.DataSource));
			}


            var copyFile = Path.Combine(Path.GetDirectoryName(OleDbConn.DataSource), fileName);
            File.Copy(OleDbConn.DataSource, copyFile, true);

            var backupDirList = SettingDal.GetStringValues("备份目录");
            if (backupDirList.Count > 0)
            {
                if (Directory.Exists(backupDirList[0]))
                {
                    var backupFile = Path.Combine(backupDirList[0], fileName);
                    File.Copy(copyFile, backupFile, true);
                }
            }
        }

		public static void DeleteOldDbFile()
		{
			var backupDays = SettingDal.GetIntValues("数据库文件备份时间")[0];

			DateTime dt;
			var today = DateTime.Today;
			foreach (var file in Directory.EnumerateFiles(Path.GetDirectoryName(OleDbConn.DataSource), "*.mdb"))
			{
				if (string.IsNullOrEmpty(file))
					continue;

				var splilt = Path.GetFileNameWithoutExtension(file).Split(' ');
				if (splilt.Count() < 2)
					continue;

				DateTime.TryParse(splilt[1], out dt);
				if (dt.AddDays(backupDays) < today)
				{
					File.Delete(file);
				}
			}
		}
	}
}