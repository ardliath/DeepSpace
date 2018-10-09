using DeepSpace.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeepSpace.Core.Tests
{
    [TestClass]
    public class ShipManagerTests
    {
        [TestMethod]
        public void Test_GetDistance() {
            var location1 = new Location() {
                X = 0,
                Y = 0,
                Z = 0
            };
            var location2 = new Location() {
                X = 2,
                Y = 0,
                Z = 0
            };
            Assert.IsTrue(ShipManager.GetDistance(location1, location2) == 2);
            
            location1 = new Location() {
                X = 0,
                Y = 0,
                Z = -2
            };
            location2 = new Location() {
                X = 0,
                Y = 0,
                Z = -1
            };
            Assert.IsTrue(ShipManager.GetDistance(location1, location2) == 1);
            
            location1 = new Location() {
                X = 0,
                Y = 5,
                Z = 0
            };
            location2 = new Location() {
                X = 0,
                Y = -2,
                Z = 0
            };
            Assert.IsTrue(ShipManager.GetDistance(location1, location2) == 7);
        }
    }
}
