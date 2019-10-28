using System;

namespace Models
{
    public class Rate
    {
        public int Id { get; set; }
        public double Ask { get; set; }
        public double Bid { get; set; }
        public string Pair { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
