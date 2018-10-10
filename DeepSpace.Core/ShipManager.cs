using DeepSpace.Contracts;
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
            ShipDataAccess = shipDataAccess;
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

            await ShipDataAccess.InsertShipAsync(ship);
            
            return ship;
        }

        private Location DetermineStartLocation()
        {
            var random = new Random();
            return new Location
            {
                X = random.Next(100) - 50,
                Y = random.Next(100) - 50,
                Z = random.Next(100) - 50
            };
        }

        public async Task<Ship> GetShipAsync(string commandCode)
        {
            var ship = ShipDataAccess.GetShip(commandCode);
            if (UpdateMovements(ship))
            {
                await ShipDataAccess.UpsertShipAsync(ship);
            }
            return await Task.FromResult(ship);
        }

        public async Task<Move> MoveAsync(string commandCode, decimal x, decimal y, decimal z)
        {
            var ship = await this.GetShipAsync(commandCode);
            if (UpdateMovements(ship))
            {
                await ShipDataAccess.UpsertShipAsync(ship);
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
            await ShipDataAccess.UpsertShipAsync(ship);

            return move;
        }

        public static double GetDistance(Location firstLocation, Location secondLocation)
        {
            // Source: https://stackoverflow.com/questions/8914669
            var deltaX = firstLocation.X - secondLocation.X;
            var deltaY = firstLocation.Y - secondLocation.Y;
            var deltaZ = firstLocation.Z - secondLocation.Z;

            return Math.Sqrt((double) (deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ));
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
            var defendingShip = ShipDataAccess.GetShipByTransponderCode(transponderCode); // and the defending ship
            if (UpdateMovements(defendingShip)) // if the defender's position has changed then update it
            {
                await ShipDataAccess.UpsertShipAsync(defendingShip);
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
                KillShip();
            }
            else
            {
                ShipDataAccess.UpsertShipAsync(ship); // otherwise update them with the new health
            }
        }

        private void KillShip()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Ship>> ScanAsync(string commandCode)
        {
            var ship = ShipDataAccess.GetShip(commandCode);
            if(UpdateMovements(ship))
            {
                await ShipDataAccess.UpsertShipAsync(ship);
            }
            var nearbyShips = ShipDataAccess.ScanForShips(ship.CommandCode, ship.Location, ship.Statistics.ScanRange);
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
