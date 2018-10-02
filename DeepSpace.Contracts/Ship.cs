namespace DeepSpace.Contracts
{
    public class Ship
    {
        public string Name { get; set; }
        public string CommandCode { get; set; }
        public string TransponderCode { get; set; }

        public Location Location { get; set; }
        public Move Move { get; set; }
    }    
}
