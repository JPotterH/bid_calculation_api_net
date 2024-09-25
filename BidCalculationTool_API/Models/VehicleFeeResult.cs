namespace BidCalculationTool_API.Models
{
    // More a DTO than a Model itself, kept together with models for simplicity 
    public class VehicleFeeResult
    {
        public decimal BasicBuyerFee { get; set; }
        public decimal SellerSpecialFee { get; set; }
        public decimal AssociationFee { get; set; }
        public decimal StorageFee { get; set; }
        public decimal TotalFee { get; set; }
    }
}
