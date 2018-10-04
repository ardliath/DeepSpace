namespace DeepSpace.Contracts
{
    public interface IShieldUpgrades
    {
        string Name { get; set; }
        double ShieldValue { get; set; }
        double Weight { get; set; }
        double SellingPrice { get; set; }
    }
}