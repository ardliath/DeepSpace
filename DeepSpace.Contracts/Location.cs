namespace DeepSpace.Contracts
{
    public class Location
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }

        public Location(decimal setX, decimal setY, decimal setZ) {
            X = setX;
            Y = setY;
            Z = setZ;
        }
    }
}