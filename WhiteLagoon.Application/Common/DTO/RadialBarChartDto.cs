﻿namespace WhiteLagoon.Web.ViewModels
{
    public class RadialBarChartDto
    {
        public decimal TotalCount { get; set; }
        public decimal CountInCurrentMonth { get; set; }
        public bool HasIncreased { get; set; }
        public int[] Series { get; set; }
    }
}