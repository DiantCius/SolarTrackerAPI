﻿using Backend.Models;

namespace Backend.DTO.Requests
{
    public record AddEnergyProductionsRequest(string SerialNumber, List<EnergyProduction> EnergyProductions);
}