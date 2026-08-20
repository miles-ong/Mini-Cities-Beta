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
using Unity.Properties;
using UnityEngine;

namespace MiniCities.Systems.City
{
    public class CityManager : MonoBehaviour
    {
        private struct CityContribution
        {
            public int Population;
            public int Culture;
            public int Pollution;
        }

        [SerializeField] private MapManager mapManager;

        private readonly HashSet<BuildingRuntime> _activeBuildings = new();
        private readonly Dictionary<BuildingRuntime, CityContribution> _lastContribution = new();
        private readonly BuildingBuffIndex _buildingBuffIndex = new();
        private readonly CityBuffIndex _cityBuffIndex = new();
        private readonly AbilityDispatcher _abilityDispatcher = new();

        private CityStats _baseStats;
        private CityStats _totalStats;
        private CityStats _targetStats;

        private static int _placementIndex;

        [CreateProperty] public int Population => _totalStats.Population;
        [CreateProperty] public int Culture => _totalStats.Culture;
        [CreateProperty] public int Pollution => _totalStats.Pollution;
        [CreateProperty] public int Money =>  _totalStats.Money;

        [CreateProperty] public int TargetPopulation => _targetStats.Population;
        [CreateProperty] public int TargetCulture => _targetStats.Culture;
        [CreateProperty] public int TargetPollution => _targetStats.Pollution;

        private void Awake()
        {
            GameEventBus<BuildingPlacedEvent>.OnEvent += OnBuildingPlaced;
            GameEventBus<BuildingRemovedEvent>.OnEvent += OnBuildingRemoved;
            GameEventQueue.OnDispatch += RecalculateCity;
        }

        public void PassContext(GameSessionContext context) => _abilityDispatcher.SetContext(context);
        
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
        }
        
        private void OnBuildingPlaced(BuildingPlacedEvent e)
        {
            var building = e.Building;
            building.SetPlacementOrder(_placementIndex++);
            _activeBuildings.Add(building);
            building.MarkDirty();
            MarkNeighborsDirty(building.Location);

            _abilityDispatcher.QueueWhenPlaced(building);
            _abilityDispatcher.QueueWheneverBuilding(AbilityTriggerType.WheneverBuildingPlaced, building, _activeBuildings);
            
            StartCoroutine(GameEventQueue.Pump(0.3f, 0.05f));
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

            _abilityDispatcher.QueueWheneverBuilding(AbilityTriggerType.WheneverBuildingRemoved, building, _activeBuildings);

            MarkNeighborsDirty(building.Location);
        }

        private void MarkNeighborsDirty(Vector3Int location)
        {
            var neighbors = mapManager.GetBlocksInScope<BuildingRuntime>(location, GridScope.Nearby, GridLevel.Level);

            foreach (var neighbor in neighbors) neighbor.MarkDirty();
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
            
            foreach (var b in GetOrderedBuildingBuffs(building)) 
                building.TotalStats.Modify(b.Stat, b.Operation, b.Amount);
            
            var newC = ComputeCityContribution(building.TotalStats);
            var oldC = _lastContribution.GetValueOrDefault(building);
            
            ShowContributionChanges(building, oldC, newC);
            
            ApplyContributionDelta(oldC, newC);
            _lastContribution[building] = newC;
            
            building.MarkClean();
        }
        
        private IEnumerable<BuildingBuff> GetOrderedBuildingBuffs(BuildingRuntime building) =>
            _buildingBuffIndex
                .GetReceiverBuffs(building)
                .Concat(_buildingBuffIndex.GetLocationalBuffs(building.Location))
                .Where(b => b.Condition == null || b.Condition(building))
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
                if (mapManager.GetBlock(buff.Location.Value) is BuildingRuntime b) b.MarkDirty();
            }
        }
        
        private void ApplyContributionDelta(CityContribution oldC, CityContribution newC)
        {
            _baseStats.ModifyStat(CityStat.Population, Operation.Add, newC.Population - oldC.Population);
            _baseStats.ModifyStat(CityStat.Culture, Operation.Add, newC.Culture - oldC.Culture);
            _baseStats.ModifyStat(CityStat.Pollution, Operation.Add, newC.Pollution - oldC.Pollution);
        }
        
        private static void ShowContributionChanges(
            BuildingRuntime building,
            CityContribution oldC,
            CityContribution newC)
        {
            var populationDelta = newC.Population - oldC.Population;
            var cultureDelta = newC.Culture - oldC.Culture;
            var pollutionDelta = newC.Pollution - oldC.Pollution;

            if (populationDelta != 0) building.ShowStatChange(BuildingStat.Population, populationDelta);
            if (cultureDelta != 0) building.ShowStatChange(BuildingStat.Culture, cultureDelta);
            if (pollutionDelta != 0) building.ShowStatChange(BuildingStat.Pollution, pollutionDelta);
        }

        private static CityContribution ComputeCityContribution(BuildingStats stats) => new()
        {
            Population = stats.Get(BuildingStat.Population),
            Culture = stats.Get(BuildingStat.Culture),
            Pollution = stats.Get(BuildingStat.Pollution)
        };
    }
}