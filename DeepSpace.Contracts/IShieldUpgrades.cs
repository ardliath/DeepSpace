namespace DeepSpace.Contracts
{
    public interface IShieldUpgrades
    {
        string Name { get; set; }
        int ShieldValue { get; set; }
        float Weight { get; set; }
        float SellingPrice { get; set; }
    }
}