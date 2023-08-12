namespace Infrastructure.DataBaseWithNoRelation.CustomEntities
{
    public class IntrumentLastTrade
    {
        public string Name { get; set; } = string.Empty;
        public DateTime DateEn { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }
}
