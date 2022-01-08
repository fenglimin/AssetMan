using System.Collections.Generic;
using Constants;

namespace Translation
{
	/// <summary>
	/// Summary description for TranslationManager
	/// </summary>
	public static class TranslationManager
	{
		private static Dictionary<string, string> TranDic;
 
		static TranslationManager()
		{
			TranDic = new Dictionary<string, string>
			{
				{TableFieldName.Balance, "金额"},
				{TableFieldName.Type, "类别"},
				{TableFieldName.BankName, "银行"},
				{TableFieldName.CardName, "卡名"},
				{TableFieldName.Date, "日期"},
				{TableFieldName.NetWorthDate, "净值日期"},
				{TableFieldName.Income, "收入"},
				{TableFieldName.Outcome, "支出"},
				{TableFieldName.Description, "描述"},				
				{TableFieldName.CreditAll, "总额度"},
				{TableFieldName.CreditAvailable, "可用额度"},
				{TableFieldName.BillPeriod, "账单周期"},
				{TableFieldName.BillAccount, "本期金额"},
				{TableFieldName.BillPayDay, "还款日"},
				{TableFieldName.BillPaid, "已还款"},
				{TableFieldName.BillNotPaid, "需还款"},
				{TableFieldName.InvestType, "类别"},
				{TableFieldName.InvestName, "名称"},
				{TableFieldName.InvestAmount, "金额"},
				{TableFieldName.InvestStartDate, "起始日期"},
				{TableFieldName.InvestEndDate, "终止日期"},
				{TableFieldName.InvestBenifit, "收益"},
				{TableFieldName.InvestBenifitRate, "收益率"},
				{TableFieldName.InvestAvailDate, "到账日期"},
				{TableFieldName.InvestPeriod, "期限"},
				{TableFieldName.DayDetailActionType, "类型"},
				{TableFieldName.Surplus, "结余"},
                {TableFieldName.FundName, "名称"},
                {TableFieldName.FundType, "类型"},
                {TableFieldName.FundTotalAmount, "金额"},
                {TableFieldName.FundTotalShare, "份额"},
                {TableFieldName.FundNetWorth, "净值"},
                {TableFieldName.FundTotalBenefit, "总收益"},
                {TableFieldName.FundTotalBonus, "已分红"},
                {TableFieldName.NextOpenDate, "开放日"},
                {TableFieldName.WeightedBenefitRate, "收益率"},
                {TableFieldName.NetWorth, "净值"},
                {TableFieldName.NetWorthDelta, "变动"},
                {TableFieldName.Share, "份额"},
                {TableFieldName.ShareAvailable, "可用份额"},
                {DropListItemName.ThisWeek, "本周"},
				{DropListItemName.LastWeek, "最近一周"},
				{DropListItemName.PrevWeek, "上一周"},
				{DropListItemName.ThisMonth, "本月"},
				{DropListItemName.LastMonth, "最近一月"},
				{DropListItemName.PrevMonth, "上个月"},
				{DropListItemName.ThisYear, "本年"},
				{DropListItemName.LastYear, "最近一年"},
				{DropListItemName.PrevYear, "去年"},
                {DropListItemName.PrevPrevYear, "前年"},
                {DropListItemName.PrevPrevPrevYear, "大前年"},
                {DropListItemName.FromDay, "从指定日开始"},
				{DropListItemName.ToDay, "到指定日结束"},
				{DropListItemName.FreeSet, "自由设定"},
                {DropListItemName.All, "全部"},
				{DropListItemName.MoneyIn, "收入"},
				{DropListItemName.MoneyOut, "支出"},
				{DropListItemName.TransferIn, "转入"},
				{DropListItemName.TransferOut, "转出"}
			};
		}

		public static string Translate(string key)
		{
			return TranDic.ContainsKey(key) ? TranDic[key] : key;
		}
	}
}