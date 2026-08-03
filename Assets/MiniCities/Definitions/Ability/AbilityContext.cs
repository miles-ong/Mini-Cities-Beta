using MiniCities.Core.Event;
using MiniCities.Runtime.Block;
using MiniCities.Systems.GameSys;

namespace MiniCities.Definitions.Ability
{
    public readonly struct AbilityContext
    {
        public GameContext Game { get; }
        public IEvent Evt { get; }
        public BuildingRuntime Building { get; }

        public AbilityContext(GameContext game, IEvent evt, BuildingRuntime building)
        {
            Game = game;
            Evt = evt;
            Building = building;
        }
    }
}