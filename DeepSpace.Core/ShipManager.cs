using DeepSpace.Contracts;
using DeepSpace.Data;
using System;
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
            var random = new Random();

            var ship = new Ship
            {
                Name = name,
                CommandCode = Guid.NewGuid().ToString(),
                TransponderCode = Guid.NewGuid().ToString(),
                Location = new Location
                {
                    X = random.Next(),
                    Y = random.Next(),
                    Z = random.Next()
                },
                Statistics = new Statistics
                {
                    Speed = 1
                }
            };

            await this.ShipDataAccess.InsertShipAsync(ship);
            return ship;
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

            // FYI: https://math.stackexchange.com/a/42643

            var distanceX = Math.Abs(ship.Location.X - destination.X);
            var distanceY = Math.Abs(ship.Location.Y - destination.Y);
            var distanceZ = Math.Abs(ship.Location.Z - destination.Z);

            var deltaX = Math.Pow((double)distanceX, (double)distanceX);
            var deltaY = Math.Pow((double)distanceY, (double)distanceY);
            var deltaZ = Math.Pow((double)distanceZ, (double)distanceZ);

            var overallMovement = Math.Sqrt(deltaX + deltaY + deltaZ);

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
