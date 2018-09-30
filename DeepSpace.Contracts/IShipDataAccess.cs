﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpace.Contracts
{
    public interface IShipDataAccess
    {
        Task<Ship> InsertShipAsync(Ship ship);
    }
}
