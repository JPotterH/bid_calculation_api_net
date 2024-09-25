namespace BidCalculationTool_API.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public decimal BasePrice { get; set; } = 0;
        public string? Description { get; set; } = string.Empty;
        public int Type { get; set; } = (int)VehicleTypeEnum.Common;
    }
}
