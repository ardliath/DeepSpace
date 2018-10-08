namespace DeepSpace.Contracts
{
    public interface IShieldUpgrades
    {
        string Name { get; set; }
        int ShieldValue { get; set; }
        int Weight { get; set; }
        double SellingPrice { get; set; }
    }
}