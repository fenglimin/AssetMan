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
		public static DataTable CreateDateTableFromAllInvestments(bool hideEndedInvest, int endDayPeriod)
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

            var investmentList = InvestDal.LoadAllInvestments();
            investmentList = AdjustInvestment(investmentList, hideEndedInvest, endDayPeriod);

            double rateSummary = 0;
            double totalAmount = 0;
            double totalBenefit = 0;
            foreach (var investmentInfo in investmentList)
            {
                totalAmount += investmentInfo.InvestAmount;

                var rate = Convert.ToDouble(investmentInfo.InvestBenifitRate.TrimEnd('%'));
                investmentInfo.InvestBenifitRate = rate.ToString("f2") + "%";
                var period = GetDaySpan(investmentInfo.InvestAvailDate, investmentInfo.InvestStartDate);
                investmentInfo.InvestBenifit = (int)(investmentInfo.InvestAmount * rate / 365 * period / 100);
                
                totalBenefit += investmentInfo.InvestBenifit;
                rateSummary += investmentInfo.InvestAmount * rate;
            }

            if (investmentList.Count > 0)
            {
                GridViewManager.AddRow(dt, CreateStatisticRowDataForInvestment(totalAmount, totalBenefit, rateSummary / totalAmount));
            }
            

            foreach (var investDetail in investmentList)
			{
				GridViewManager.AddRow(dt, CreateRowDataForInvestment(investDetail));
			}

			return dt;
		}

        private static IList<InvestDetail> AdjustInvestment(IList<InvestDetail> investmentList, bool hideEndedInvest, int endDayPeriod)
        {
            var count = investmentList.Count;
            var today = DateTime.Today;
            for (var i = 0; i < count; i++)
            {
                DateTime endDay;
                DateTime.TryParse(investmentList[i].InvestAvailDate, out endDay);
                if ((endDayPeriod > 0 && today.AddDays(endDayPeriod) < endDay) || (hideEndedInvest && endDay < today))
                {
                    investmentList.RemoveAt(i);
                    count = investmentList.Count;
                    i--;
                }
            }

            return investmentList;
        }

        private static Dictionary<string, string> CreateStatisticRowDataForInvestment(double totalAmount, double totalBenefit, double benefitRate)
        {
            var rowData = new Dictionary<string, string>();

            rowData[TableFieldName.InvestAmount] = totalAmount.ToString(CultureInfo.InvariantCulture);
            rowData[TableFieldName.InvestBenifit] = totalBenefit.ToString(CultureInfo.InvariantCulture);
            rowData[TableFieldName.InvestBenifitRate] = benefitRate.ToString("f2") + "%";
            
            return rowData;
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

        public static DataTable CreateDateTableFromAllFunds(string condition)
        {
            var dt = new DataTable();
            dt.Columns.Add(TableFieldName.FundID);
            dt.Columns.Add(TableFieldName.FundName);
            dt.Columns.Add(TableFieldName.FundType);
            dt.Columns.Add(TableFieldName.FundTotalAmount, typeof(double));
            //dt.Columns.Add(TableFieldName.FundTotalShare);
            dt.Columns.Add(TableFieldName.NetWorthDate);
            dt.Columns.Add(TableFieldName.FundNetWorth, typeof(double));
            dt.Columns.Add(TableFieldName.NetWorthDelta, typeof(double));
            dt.Columns.Add(TableFieldName.WeightedBenefitRate);
            dt.Columns.Add(TableFieldName.FundTotalBonus, typeof(double));
            dt.Columns.Add(TableFieldName.FundTotalBenefit, typeof(double));
            dt.Columns.Add(TableFieldName.NextOpenDate);

            double rateSummary = 0;
            double totalAmount = 0;
            double totalBenefit = 0;
            double totalBonus = 0;
            var fundList = InvestDal.LoadFundList(condition);
            foreach (var fundInfo in fundList)
            {
                totalAmount += fundInfo.TotalAmount;
                totalBenefit += fundInfo.TotalBenefit;
                totalBonus += fundInfo.TotalBonus;
                rateSummary += fundInfo.TotalAmount * fundInfo.WeightedBenefitRate;
            }

            GridViewManager.AddRow(dt, CreateStatisticRowDataFoFund(totalAmount, totalBenefit, rateSummary/totalAmount, totalBonus));

            foreach (var fundInfo in fundList)
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
            rowData[TableFieldName.FundType] = fundInfo.FundType;
            rowData[TableFieldName.FundTotalAmount] = fundInfo.TotalAmount.ToString("f0");
            //rowData[TableFieldName.FundTotalShare] = fundInfo.TotalShare.ToString("f2");
            rowData[TableFieldName.NetWorthDate] = fundInfo.CurrentDate;
            rowData[TableFieldName.FundNetWorth] = fundInfo.CurrentNetWorth.ToString("f4");
            rowData[TableFieldName.NetWorthDelta] = fundInfo.NetWorthDelta.ToString("f4");
            rowData[TableFieldName.FundTotalBenefit] = fundInfo.TotalBenefit.ToString("f0");
            rowData[TableFieldName.FundTotalBonus] = fundInfo.TotalBonus.ToString("f0");
            rowData[TableFieldName.WeightedBenefitRate] = fundInfo.WeightedBenefitRate.ToString("f2") + "%";
            rowData[TableFieldName.NextOpenDate] = fundInfo.NextOpenDate;

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
            rowData[TableFieldName.InvestBenifitRate] = fundDetail.BenefitRate.ToString("f3") + "%";

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

        private static Dictionary<string, string> CreateStatisticRowDataFoFund(double totalAmount, double totalBenefit, double balanceRate, double totalBonus)
        {
            var rowData = new Dictionary<string, string>();

            rowData[TableFieldName.FundTotalAmount] = totalAmount.ToString("f0");
            rowData[TableFieldName.FundTotalBenefit] = totalBenefit.ToString("f0");
            rowData[TableFieldName.FundTotalBonus] = totalBonus.ToString("f0");
            rowData[TableFieldName.WeightedBenefitRate] = balanceRate.ToString("f2") + "%";

            return rowData;
        }
    }
}