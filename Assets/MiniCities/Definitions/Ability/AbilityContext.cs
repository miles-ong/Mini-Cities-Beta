using MiniCities.Runtime.Block;
using MiniCities.Systems.Game;

namespace MiniCities.Definitions.Ability
{
    public readonly struct AbilityContext
    {
        public readonly GameContext Game;
        public readonly BuildingRuntime Owner;
        
        public AbilityContext(GameContext game, BuildingRuntime owner)
        {
            Game = game;
            Owner = owner;
        }
    }
}