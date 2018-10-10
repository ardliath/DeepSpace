namespace DeepSpace.Contracts
{
    public class Location
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }

        public Location(decimal setX = 0, decimal setY = 0, decimal setZ = 0) {
            X = setX;
            Y = setY;
            Z = setZ;
        }
    }
}