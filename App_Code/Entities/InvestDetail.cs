namespace Entities
{
	/// <summary>
	/// Summary description for InvestDetail
	/// </summary>
	public class InvestDetail
	{
		public string InvestType, InvestName, InvestStartDate, InvestEndDate, InvestBenifitRate, InvestAvailDate;
		public int InvestID, InvestAmount, InvestBenifit;
	}

    public class FundInfo
    {
        public string FundName, CurrentDate, FundCode, FundType, NextOpenDate;
        public int FundId;
        public double TotalAmount, TotalShare, CurrentNetWorth, TotalBenefit, WeightedBenefitRate, TotalBonus, NetWorthDelta;
    }

    public class FundDetail
    {
        public string Type, OperationDate;
        public int Id, FundId;
        public double Amount, TotalShare, NetWorth, AvailableShare, BenefitRate;
    }
}