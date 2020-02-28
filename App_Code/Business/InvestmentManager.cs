using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Constants;
using DataAccess;
using Entities;
using Translation;
using UI;

namespace Business
{
	/// <summary>
	/// Summary description for InvestmentManager
	/// </summary>
	public static class InvestmentManager
	{
		public static DataTable CreateDateTableFromAllInvestments()
		{
			var dt = new DataTable();
			dt.Columns.Add(TableFieldName.Id);
			dt.Columns.Add(TableFieldName.InvestType);
			dt.Columns.Add(TableFieldName.InvestName);
			dt.Columns.Add(TableFieldName.InvestAmount);
			dt.Columns.Add(TableFieldName.InvestStartDate);
			dt.Columns.Add(TableFieldName.InvestPeriod);
			dt.Columns.Add(TableFieldName.InvestBenifit);
			dt.Columns.Add(TableFieldName.InvestBenifitRate);
			dt.Columns.Add(TableFieldName.InvestAvailDate);


			foreach (var investDetail in InvestDal.LoadAllInvestments())
			{
				GridViewManager.AddRow(dt, CreateRowDataForInvestment(investDetail));
			}

			return dt;
		}

		private static Dictionary<string, string> CreateRowDataForInvestment(InvestDetail investDetail)
		{
			var rowData = new Dictionary<string, string>();

			rowData[TableFieldName.Id] = investDetail.InvestID.ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.InvestType] = investDetail.InvestType;
			rowData[TableFieldName.InvestName] = investDetail.InvestName;
			rowData[TableFieldName.InvestAmount] = investDetail.InvestAmount.ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.InvestStartDate] = investDetail.InvestStartDate;
			rowData[TableFieldName.InvestPeriod] = GetDaySpan(investDetail.InvestAvailDate, investDetail.InvestStartDate).ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.InvestBenifit] = investDetail.InvestBenifit.ToString(CultureInfo.InvariantCulture);
			rowData[TableFieldName.InvestBenifitRate] = investDetail.InvestBenifitRate;
			rowData[TableFieldName.InvestAvailDate] = investDetail.InvestAvailDate;

			return rowData;
		}

        public static DataTable CreateDateTableFromAllFunds()
        {
            var dt = new DataTable();
            dt.Columns.Add(TableFieldName.FundID);
            dt.Columns.Add(TableFieldName.FundName);
            dt.Columns.Add(TableFieldName.FundTotalAmount);
            dt.Columns.Add(TableFieldName.FundTotalShare);
            dt.Columns.Add(TableFieldName.Date);
            dt.Columns.Add(TableFieldName.FundNetWorth);
            dt.Columns.Add(TableFieldName.FundTotalBenefit);
            dt.Columns.Add(TableFieldName.WeightedBenefitRate);

            foreach (var fundInfo in InvestDal.LoadFundList(""))
            {
                GridViewManager.AddRow(dt, CreateRowDataForFund(fundInfo));
            }

            return dt;
        }

        public static DataTable CreateDateTableFromFund(string condition)
        {
            var dt = new DataTable();
            dt.Columns.Add(TableFieldName.Date);
            dt.Columns.Add(TableFieldName.Type);
            dt.Columns.Add(TableFieldName.Balance);
            dt.Columns.Add(TableFieldName.NetWorth);
            dt.Columns.Add(TableFieldName.Share);
            dt.Columns.Add(TableFieldName.ShareAvailable);
            dt.Columns.Add(TableFieldName.InvestBenifitRate);

            foreach (var fundDetail in InvestDal.LoadFundDetailList(condition))
            {
                GridViewManager.AddRow(dt, CreateRowDataForFundDetail(fundDetail));
            }

            return dt;
        }

        private static Dictionary<string, string> CreateRowDataForFund(FundInfo fundInfo)
        {
            var rowData = new Dictionary<string, string>();

            rowData[TableFieldName.FundID] = fundInfo.FundId.ToString(CultureInfo.InvariantCulture);
            rowData[TableFieldName.FundName] = fundInfo.FundName;
            rowData[TableFieldName.FundTotalAmount] = fundInfo.TotalAmount.ToString("f0");
            rowData[TableFieldName.FundTotalShare] = fundInfo.TotalShare.ToString("f2");
            rowData[TableFieldName.Date] = fundInfo.CurrentDate;
            rowData[TableFieldName.FundNetWorth] = fundInfo.CurrentNetWorth.ToString("f4");
            rowData[TableFieldName.FundTotalBenefit] = fundInfo.TotalBenefit.ToString("f0");
            rowData[TableFieldName.WeightedBenefitRate] = fundInfo.WeightedBenefitRate.ToString("f3");

            return rowData;
        }

        private static Dictionary<string, string> CreateRowDataForFundDetail(FundDetail fundDetail)
        {
            var rowData = new Dictionary<string, string>();

            rowData[TableFieldName.Date] = fundDetail.OperationDate;
            rowData[TableFieldName.Type] = fundDetail.Type;
            rowData[TableFieldName.Balance] = fundDetail.Amount.ToString("f0");
            rowData[TableFieldName.NetWorth] = fundDetail.NetWorth.ToString("f4");
            rowData[TableFieldName.Share] = fundDetail.TotalShare.ToString("f3");
            rowData[TableFieldName.ShareAvailable] = fundDetail.AvailableShare.ToString("f3");
            rowData[TableFieldName.InvestBenifitRate] = fundDetail.BenefitRate.ToString("f3");

            return rowData;
        }

        public static int GetDaySpan(string startDate, string endDate)
		{
			DateTime dtStart, dtEnd;
			DateTime.TryParse(startDate, out dtStart);
			DateTime.TryParse(endDate, out dtEnd);

			var daySpan = dtStart - dtEnd;
			return daySpan.Days;
		}


		public static string GetDelayString(string dateExpected, string dateActual)
		{
			var daySpan = GetDaySpan(dateExpected, dateActual);
			return GetDelayString(-daySpan);
		}

		public static string GetDelayString(int daySpan)
		{
			var ret = daySpan == 0 ? "当天到账" : string.Format("延期 {0} 天到账", daySpan);
			return ret;
		}

		public static string GetActualDay(string dateExpected, string delay)
		{
			if (delay == "当天到账") return dateExpected;

			var items = delay.Split(' ');
			if (items.Count() != 3) return dateExpected;

			DateTime dtActual;
			DateTime.TryParse(dateExpected, out dtActual);

			var daySpan = Convert.ToInt32(items[1]);
			return dtActual.AddDays(daySpan).ToString("yyyy-MM-dd");
		}

		public static string CalculateBenifitRate(InvestDetail investDetail)
		{
			var daySpan = -GetDaySpan(investDetail.InvestStartDate, investDetail.InvestAvailDate);
			var benifitRate = investDetail.InvestBenifit*365000/daySpan/investDetail.InvestAmount;
			return string.Format("{0}.{1}", benifitRate/10, benifitRate%10);
		}
	}
}