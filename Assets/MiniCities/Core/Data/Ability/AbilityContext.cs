using MiniCities.Core.Event;
using MiniCities.Runtime.Game;

namespace MiniCities.Core.Data.Ability
{
    public readonly struct AbilityContext
    {
        public GameContext Game { get; }
        public IEvent Evt { get; }

        public AbilityContext(GameContext game, IEvent evt)
        {
            Game = game;
            Evt = evt;
        }
    }
}