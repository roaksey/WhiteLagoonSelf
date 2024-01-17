namespace WhiteLagoon.Web.ViewModels
{
    public class RadialBarChartVM
    {
        public decimal TotalCount { get; set; }
        public decimal IncreaseDecreaseAmount { get; set; }
        public bool HasIncreased { get; set; }
        public int[] Series { get; set; }
    }
}
