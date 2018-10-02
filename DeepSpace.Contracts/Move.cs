using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSpace.Contracts
{
    public class Move
    {
        public Location From { get; set; }
        public Location To { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
