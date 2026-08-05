using System;
using System.Collections.Generic;
using System.Linq;
using MiniCities.Core;
using MiniCities.Definitions.Blueprint.Stat;
using MiniCities.Runtime.Block;
using MiniCities.Systems.City.Buff;
using MiniCities.Systems.City.Stat;
using UnityEngine;

namespace MiniCities.Systems.City
{
    public class CityManager
    {
        private struct CityContribution
        {
            public int Population;
            public int Culture;
            public int Pollution;
        }
        
        private readonly Dictionary<Vector3Int, BuildingRuntime> _buildings = new();
        private readonly Dictionary<BuildingRuntime, CityContribution> _lastContribution = new();

        private readonly BuildingBuffIndex _buildingBuffIndex = new();
        private readonly CityBuffIndex _cityBuffIndex = new();

        private CityStats _baseStats;
        private CityStats _totalStats;

        public int Population => _totalStats.Population;
        public int Culture => _totalStats.Culture;
        public int Pollution => _totalStats.Pollution;
        public int Money =>  _totalStats.Money;

        public CityManager()
        {
            // reg events
        }

        public void AddBuilding(BuildingRuntime building) => _buildings.Add(building.Location, building);

        public void RemoveBuilding(BuildingRuntime building)
        {
            foreach (var b in _buildingBuffIndex.GetSourceBuffs(building)) MarkAffectedDirty(b);

            var last = _lastContribution.TryGetValue(building, out var c) ? c : default;
            ApplyContributionDelta(last, default);
            _lastContribution.Remove(building);
            
            _buildings.Remove(building.Location);

            _buildingBuffIndex.RemoveSource(building);
            _cityBuffIndex.RemoveSource(building);
            _buildingBuffIndex.RemoveReceiver(building);
        }

        public BuildingRuntime GetBuilding(Vector3Int location)
        {
            _buildings.TryGetValue(location, out var building);

            return building;
        }
        
        public void ModifyBaseStat(CityStat stat, Operation operation, float amount) =>
            _baseStats.ModifyStat(stat, operation, amount);

        public void RegisterBuildingBuff(BuildingBuff buff)
        {
            _buildingBuffIndex.Register(buff);
            MarkAffectedDirty(buff);
        }
        
        public void UnregisterBuildingBuff(BuildingBuff buff)
        {
            _buildingBuffIndex.Unregister(buff);
            MarkAffectedDirty(buff);
        }
        
        public void RegisterCityBuff(CityBuff buff) => _cityBuffIndex.Register(buff);
        public void UnregisterCityBuff(CityBuff buff) => _cityBuffIndex.Unregister(buff);
        
        public void RecalculateCity()
        {
            RecalculateDirtyBuildings();
            _totalStats = _baseStats;
            
            foreach (var cb in _cityBuffIndex.Buffs) _totalStats.ModifyStat(cb.Stat, cb.Operation, cb.Amount);
        }
        
        private void RecalculateDirtyBuildings()
        {
            foreach (var b in _buildings.Values)
            {
                if (b.CachedVersion == b.CurrentVersion) continue;
                
                RecalculateBuilding(b);
            }
        }

        private void RecalculateBuilding(BuildingRuntime building)
        {
            building.TotalStats.CopyFrom(building.BaseStats);
            
            foreach (var b in GetOrderedBuildingBuffs(building)) building.TotalStats.Modify(b.Stat, b.Operation, b.Amount);

            var newC = ComputeCityContribution(building.TotalStats);
            var oldC = _lastContribution.TryGetValue(building, out var prev) ? prev : default;
            ApplyContributionDelta(oldC, newC);
            _lastContribution[building] = newC;
            
            building.MarkClean();
        }
        
        private IEnumerable<BuildingBuff> GetOrderedBuildingBuffs(BuildingRuntime building) =>
            _buildingBuffIndex
                .GetReceiverBuffs(building)
                .Concat(_buildingBuffIndex.GetLocationalBuffs(building.Location))
                .OrderBy(b => b.Stat)
                .ThenBy(b => b.Operation);

        private void MarkAffectedDirty(BuildingBuff buff)
        {
            if (buff.Receiver != null) buff.Receiver.MarkDirty();
            else if (buff.Location != null && _buildings.TryGetValue(buff.Location.Value, out var b)) b.MarkDirty();
        }

        private static CityContribution ComputeCityContribution(BuildingStats stats) => new()
        {
            Population = stats.Get(BuildingStat.Population),
            Culture = stats.Get(BuildingStat.Culture),
            Pollution = stats.Get(BuildingStat.Pollution)
        };

        private void ApplyContributionDelta(CityContribution oldC, CityContribution newC)
        {
            _baseStats.ModifyStat(CityStat.Population, Operation.Add, newC.Population - oldC.Population);
            _baseStats.ModifyStat(CityStat.Culture, Operation.Add, newC.Culture - oldC.Culture);
            _baseStats.ModifyStat(CityStat.Pollution, Operation.Add, newC.Pollution - oldC.Pollution);
        }
    }
}