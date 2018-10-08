using DeepSpace.Contracts;
using DeepSpace.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeepSpace.Core
{
    /// <summary>
    /// All the business logic for creating/managing ships
    /// </summary>
    public class ShipManager : IShipManager
    {
        public ShipManager(IShipDataAccess shipDataAccess)
        {
            this.ShipDataAccess = shipDataAccess;
        }

        public IShipDataAccess ShipDataAccess { get; }

        public async Task<Ship> CreateShipAsync(string name)
        {
            var startLocation = DetermineStartLocation();
            var ship = new Ship
            {
                Name = name,
                CommandCode = Guid.NewGuid().ToString(),
                TransponderCode = Guid.NewGuid().ToString(),
                Location = startLocation,
                ShieldUpgrades = new List<IShieldUpgrades>(),
                Statistics = new Statistics
                {
                    Speed = 1,
                    ScanRange = DeepSpaceConstants.BASE_SCANRANGE,
                    BaseHealth = DeepSpaceConstants.BASE_HEALTH,
                    CurrentHealth = DeepSpaceConstants.BASE_HEALTH,
                    FirePower = DeepSpaceConstants.BASE_FIREPOWER
                }
            };

            await this.ShipDataAccess.InsertShipAsync(ship);
            return ship;
        }

        private Location DetermineStartLocation()
        {
            var random = new Random();
            var x = random.Next(100) - 50;
            var y = random.Next(100) - 50;
            var z = random.Next(100) - 50;
            return new Location(x, y, z);
        }

        public async Task<Ship> GetShipAsync(string commandCode)
        {
            var ship = this.ShipDataAccess.GetShip(commandCode);
            if (this.UpdateMovements(ship))
            {
                await this.ShipDataAccess.UpsertShipAsync(ship);
            }
            return await Task.FromResult(ship);
        }

        public async Task<Move> MoveAsync(string commandCode, decimal x, decimal y, decimal z)
        {
            var ship = await this.GetShipAsync(commandCode);
            if (this.UpdateMovements(ship))
            {
                await this.ShipDataAccess.UpsertShipAsync(ship);
            }

            var destination = new Location { X = x, Y = y, Z = z };

            var speed = ship.Statistics.Speed;            
            double overallMovement = GetDistance(ship.Location, destination);

            // Then simple Time = Distance / Speed calc. We're going to round because you're using TimeSpan.
            var timeToMove = Convert.ToInt32(Math.Round((overallMovement / speed), 0, MidpointRounding.AwayFromZero));

            var time = new TimeSpan(0, timeToMove, 0);
            var now = DateTime.UtcNow;

            var move = new Move
            {
                StartTime = now,
                ArrivalTime = now.Add(time),
                Duration = time,
                From = ship.Location,
                To = destination
            };
            ship.Location = null;
            ship.Move = move;
            await this.ShipDataAccess.UpsertShipAsync(ship);

            return move;
        }

        public  double GetDistance(Location firstLocation, Location secondLocation)
        {
            // FYI: https://math.stackexchange.com/a/42643
            var distanceX = Math.Abs(firstLocation.X - secondLocation.X);
            var distanceY = Math.Abs(firstLocation.Y - secondLocation.Y);
            var distanceZ = Math.Abs(firstLocation.Z - secondLocation.Z);

            var deltaX = Math.Pow((double)distanceX, (double)distanceX);
            var deltaY = Math.Pow((double)distanceY, (double)distanceY);
            var deltaZ = Math.Pow((double)distanceZ, (double)distanceZ);

            var overallMovement = Math.Sqrt(deltaX + deltaY + deltaZ);
            return overallMovement;
        }

        public async Task AddShieldUpgradeAsync(string commandCode, IShieldUpgrades upgrade)
        {
            var ship = await GetShipAsync(commandCode);

            Console.WriteLine($"{ship.Name} ship bought {upgrade.Name} adding {upgrade.ShieldValue} shield points");
            ship.ShieldUpgrades.Add(upgrade);
            UpdateHealth(ship, DeepSpaceConstants.BASE_SHIELD_HEALTH);
        }

        public async Task AttackShipAsync(string commandCode, string transponderCode)
        {
            var attackingShip =  await GetShipAsync(commandCode); // get the attacking ship
            var defendingShip = this.ShipDataAccess.GetShipByTransponderCode(transponderCode); // and the defending ship
            if (this.UpdateMovements(defendingShip)) // if the defender's position has changed then update it
            {
                await this.ShipDataAccess.UpsertShipAsync(defendingShip);
            }

            UpdateHealth(defendingShip, -attackingShip.Statistics.FirePower); // reduce the defending ship's health by the firepower of the main ship
        }

        public async Task RepairAsync(string commandCode)
        {
            var ship = await GetShipAsync(commandCode);
            UpdateHealth(ship, DeepSpaceConstants.BASE_AMOUNT_HEALTH_PER_REPAIR);
        }

        public async Task RestoreAsync(string commandCode)
        {
            var ship = await GetShipAsync(commandCode);
            UpdateHealth(ship, ship.Statistics.BaseHealth + ship.Shield);
        }

        private void UpdateHealth(Ship ship, int healthChange)
        {
            ship.Statistics.CurrentHealth += healthChange;
            Console.WriteLine($"{ship.Name} Health {healthChange}");
            if(ship.Statistics.CurrentHealth <= 0) // if it's dead
            {
                this.KillShip(); // then kill it
            }
            else
            {
                this.ShipDataAccess.UpsertShipAsync(ship); // otherwise update them with the new health
            }
        }

        private void KillShip()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Ship>> ScanAsync(string commandCode)
        {
            var ship = this.ShipDataAccess.GetShip(commandCode);
            if(this.UpdateMovements(ship))
            {
                await this.ShipDataAccess.UpsertShipAsync(ship);
            }
            var nearbyShips = this.ShipDataAccess.ScanForShips(ship.CommandCode, ship.Location, ship.Statistics.ScanRange);
            return nearbyShips;
        }

        private bool UpdateMovements(Ship ship)
        {
            var now = DateTime.UtcNow;
            if (ship.Move != null && ship.Move.ArrivalTime < now)
            {
                ship.Location = ship.Move.To;
                ship.Move = null;

                return true;
            }
            return false;
        }
    }
}
