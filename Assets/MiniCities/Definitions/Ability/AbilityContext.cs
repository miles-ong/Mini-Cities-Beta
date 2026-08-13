using MiniCities.Runtime.Block;
using MiniCities.Systems.City.Stat;
using MiniCities.Systems.Game;

namespace MiniCities.Definitions.Ability
{
    public readonly struct AbilityContext
    {
        public readonly GameContext Game;
        public readonly IAbilityActivator Activator;
        public readonly BuildingRuntime TriggeringBuilding;
        public readonly CityStat? TriggeringStat;
        public readonly float TriggeringStatValue;
        
        public AbilityContext(GameContext game, IAbilityActivator activator)
        {
            Game = game;
            Activator = activator;
            TriggeringBuilding = null;
            TriggeringStat = null;
            TriggeringStatValue = 0;
        }

        private AbilityContext(
            GameContext game,
            IAbilityActivator activator,
            BuildingRuntime triggeringBuilding,
            CityStat? triggeringStat,
            float triggeringStatValue)
        {
            Game = game;
            Activator = activator;
            TriggeringBuilding = triggeringBuilding;
            TriggeringStat = triggeringStat;
            TriggeringStatValue = triggeringStatValue;
        }

        public static AbilityContext TriggeredByBuilding(
            GameContext game, 
            IAbilityActivator activator, 
            BuildingRuntime triggeringBuilding) =>
            new(game, activator, triggeringBuilding, null, 0f);

        public static AbilityContext TriggeredByCityStat(
            GameContext game, 
            IAbilityActivator activator, 
            CityStat stat, 
            float value) =>
            new(game, activator, null, stat, value);
    }
}