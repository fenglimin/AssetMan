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
		private static OleDbConnection oleDbConn;

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
				if (oleDbConn == null)
				{
					lock (SyncRoot)
					{
						if (oleDbConn == null)
						{
							var connStringSetting = ConfigurationManager.ConnectionStrings[1];
							oleDbConn = new OleDbConnection(connStringSetting.ConnectionString);
							oleDbConn.Open();
						}
					}
				}

				return oleDbConn;
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

		public static void BackupDb(string reason)
		{
			if (string.IsNullOrEmpty(OleDbConn.DataSource))
				return;
			var today = DateTime.Now;

			string fileName;
			if (reason == "当日首次刷新")
			{
				DeleteOldDbFile();

				fileName = string.Format("{0}\\{1} {2} -- {3}{4}", Path.GetDirectoryName(OleDbConn.DataSource), Path.GetFileNameWithoutExtension(OleDbConn.DataSource),
					today.ToString("yyyy-MM-dd"), reason, Path.GetExtension(OleDbConn.DataSource));

				if (File.Exists(fileName))
					return;
			}
			else
			{
				fileName = string.Format("{0}\\{1} {2} {3} -- {4}{5}", Path.GetDirectoryName(OleDbConn.DataSource), Path.GetFileNameWithoutExtension(OleDbConn.DataSource),
					today.ToString("yyyy-MM-dd"), today.ToString("HH-mm-ss"), reason, Path.GetExtension(OleDbConn.DataSource));
			}
			
			File.Copy(OleDbConn.DataSource, fileName);
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