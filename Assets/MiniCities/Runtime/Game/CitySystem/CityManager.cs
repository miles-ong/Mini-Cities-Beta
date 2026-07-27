using System.Collections.Generic;
using System.Linq;
using MiniCities.Runtime.Game.BuffSystem;
using MiniCities.Runtime.Game.MapSystem.Block.Building;

namespace MiniCities.Runtime.Game.CitySystem
{
    public class CityManager
    {
        public int TotalPopulation { get; private set; }
        public int TotalCulture { get; private set; }
        public int TotalPollution { get; private set; }
        
        private readonly BuffManager _buffManager;

        public CityManager(BuffManager buffManager)
        {
            _buffManager = buffManager;
        }

        public BuildingStats GetStats(BuildingRuntime building)
        {
            if (building.CachedVersion == building.CurrentVersion) return building.TotalStats;

            Recalculate(building);
            building.MarkClean();

            return building.TotalStats;
        }

        private void Recalculate(BuildingRuntime building)
        {
            building.TotalStats = building.BaseStats;

            foreach (var b in GetOrderedBuffs(building)) building.TotalStats.ModifyStat(b.Stat, b.Operation, b.Amount);
        }

        private List<Buff> GetOrderedBuffs(BuildingRuntime building)
        {
            return _buffManager
                .GetReceiverBuffs(building)
                .Concat(_buffManager.GetLocationalBuffs(building.Location))
                .OrderBy(b => b.Stat)
                .ThenBy(b => b.Operation)
                .ToList();
        }
    }
}