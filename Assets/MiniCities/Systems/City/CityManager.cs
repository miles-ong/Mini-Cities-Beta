using System.Collections.Generic;
using System.Linq;
using MiniCities.Core.Event;
using MiniCities.Core.Grid;
using MiniCities.Core.Math;
using MiniCities.Definitions.Ability;
using MiniCities.Definitions.Blueprint.Stat;
using MiniCities.Runtime.Block;
using MiniCities.Systems.City.Ability;
using MiniCities.Systems.City.Buff;
using MiniCities.Systems.City.Stat;
using MiniCities.Systems.Game;
using MiniCities.Systems.Map;
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

        private readonly MapManager _map;

        private readonly HashSet<BuildingRuntime> _activeBuildings = new();
        private readonly Dictionary<BuildingRuntime, CityContribution> _lastContribution = new();
        private readonly BuildingBuffIndex _buildingBuffIndex = new();
        private readonly CityBuffIndex _cityBuffIndex = new();
        private readonly AbilityDispatcher _abilityDispatcher = new();

        private CityStats _baseStats;
        private CityStats _totalStats;

        public int Population => _totalStats.Population;
        public int Culture => _totalStats.Culture;
        public int Pollution => _totalStats.Pollution;
        public int Money =>  _totalStats.Money;

        public CityManager(MapManager map)
        {
            _map = map;

            EventBus<BuildingPlacedEvent>.OnEvent += OnBuildingPlaced;
            EventBus<BuildingRemovedEvent>.OnEvent += OnBuildingRemoved;
        }

        public void SetContext(GameContext context) => _abilityDispatcher.SetContext(context);
        
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
        
        private void RecalculateCity()
        {
            RecalculateDirtyBuildings();
            _totalStats = _baseStats;
            
            foreach (var cb in _cityBuffIndex.Buffs) _totalStats.ModifyStat(cb.Stat, cb.Operation, cb.Amount);
            
            Debug.Log(_totalStats);
            EventQueue.OnDrained -= RecalculateCity;
        }
        
        private void OnBuildingPlaced(BuildingPlacedEvent e)
        {
            var building = e.Building;
            _activeBuildings.Add(building);
            building.MarkDirty();
            MarkNeighborsDirty(building.Location);

            _abilityDispatcher.QueueWhenPlaced(building);
            _abilityDispatcher.QueueWhenever(AbilityTriggerType.WheneverBuildingPlaced, building, _activeBuildings);
            
            EventQueue.OnDrained += RecalculateCity;
            EventQueue.Pump();
        }

        private void OnBuildingRemoved(BuildingRemovedEvent e)
        {
            var building = e.Building;

            foreach (var b in _buildingBuffIndex.GetSourceBuffs(building)) MarkAffectedDirty(b);

            var last = _lastContribution.GetValueOrDefault(building);
            ApplyContributionDelta(last, default);
            _lastContribution.Remove(building);

            _buildingBuffIndex.RemoveSource(building);
            _cityBuffIndex.RemoveSource(building);
            _buildingBuffIndex.RemoveReceiver(building);
            _activeBuildings.Remove(building);

            _abilityDispatcher.QueueWhenever(AbilityTriggerType.WheneverBuildingRemoved, building, _activeBuildings);

            MarkNeighborsDirty(building.Location);
        }

        private void MarkNeighborsDirty(Vector3Int location)
        {
            foreach (var n in _map.GetBlocksInScope<BuildingRuntime>(location, GridScope.Nearby, GridLevel.Level))
                n.MarkDirty();
        }
        
        private void RecalculateDirtyBuildings()
        {
            foreach (var b in _activeBuildings)
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
            var oldC = _lastContribution.GetValueOrDefault(building);
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
            if (buff.Receiver != null)
            {
                buff.Receiver.MarkDirty();
            }
            else if (buff.Location != null)
            {
                if (_map.GetBlock(buff.Location.Value) is BuildingRuntime b) b.MarkDirty();
            }
        }
        
        private void ApplyContributionDelta(CityContribution oldC, CityContribution newC)
        {
            _baseStats.ModifyStat(CityStat.Population, Operation.Add, newC.Population - oldC.Population);
            _baseStats.ModifyStat(CityStat.Culture, Operation.Add, newC.Culture - oldC.Culture);
            _baseStats.ModifyStat(CityStat.Pollution, Operation.Add, newC.Pollution - oldC.Pollution);
        }

        private static CityContribution ComputeCityContribution(BuildingStats stats) => new()
        {
            Population = stats.Get(BuildingStat.Population),
            Culture = stats.Get(BuildingStat.Culture),
            Pollution = stats.Get(BuildingStat.Pollution)
        };
    }
}