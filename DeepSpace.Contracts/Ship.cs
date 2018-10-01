using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepSpace.Contracts
{
    public class Ship
    {
        public string Name { get; set; }
        public string CommandCode { get; set; }
        public string TransponderCode { get; set; }

        public double BaseHealth { get; set; } = 100; // Could be based on ship type for example.
        private double Shield => ShieldUpgrades.Sum(su => su.ShieldValue);
        private List<IShieldUpgrades> ShieldUpgrades { get; set; }

        public double CurrentHealth { get; set; }

        public Location Location { get; set; }
        public Move Move { get; set; }
        public void AddShieldUpgrade(IShieldUpgrades upgrade)
        {
            Console.WriteLine($"{Name} ship bought {upgrade.Name} adding {upgrade.ShieldValue} shield points");
            ShieldUpgrades.Add(upgrade);
            UpdateHealth(upgrade.ShieldValue);
        }

        public void UpdateHealth(double healthChange)
        {
            Console.WriteLine($"{Name} Health {healthChange}");
            CurrentHealth += healthChange;
        }

        public void Restore()
        {
            CurrentHealth = BaseHealth + Shield;
        }
    }    
}
