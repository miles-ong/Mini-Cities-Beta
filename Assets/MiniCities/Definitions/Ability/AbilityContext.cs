using MiniCities.Runtime.Block;
using MiniCities.Systems.City.Stat;
using MiniCities.Systems.Game;

namespace MiniCities.Definitions.Ability
{
    public readonly struct AbilityContext
    {
        public readonly GameContext Game;
        public readonly BuildingRuntime Owner;

        public readonly BuildingRuntime TriggeringBuilding;
        public readonly CityStat? TriggeringStat;
        public readonly float TriggeringStatValue;
        
        public AbilityContext(GameContext game, BuildingRuntime owner)
        {
            Game = game;
            Owner = owner;
            TriggeringBuilding = null;
            TriggeringStat = null;
            TriggeringStatValue = 0;
        }

        private AbilityContext(
            GameContext game,
            BuildingRuntime owner,
            BuildingRuntime triggeringBuilding,
            CityStat? triggeringStat,
            float triggeringStatValue)
        {
            Game = game;
            Owner = owner;
            TriggeringBuilding = triggeringBuilding;
            TriggeringStat = triggeringStat;
            TriggeringStatValue = triggeringStatValue;
        }

        public static AbilityContext ForBuilding(GameContext game, BuildingRuntime owner, BuildingRuntime triggeringBuilding) =>
            new(game, owner, triggeringBuilding, null, 0f);

        public static AbilityContext ForCityStat(GameContext game, BuildingRuntime owner, CityStat stat, float value) =>
            new(game, owner, null, stat, value);
    }
}