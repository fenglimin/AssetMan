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
        public string FundName, WeightedBenefitRate;
        public int FundId, TotalAmount;
        public double TotalShare, CurrentNetWorth, TotalBenefit;
    }

    public class FundDetail
    {
        public string Type, OperationDate;
        public int FundId, Amount;
        public double TotalShare, NetWorth, AvailableShare;
    }
}