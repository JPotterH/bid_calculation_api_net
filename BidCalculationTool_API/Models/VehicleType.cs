namespace BidCalculationTool_API.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
    }

    public enum VehicleTypeEnum : int
    {
        Common = 1,
        Luxury
    }
}
