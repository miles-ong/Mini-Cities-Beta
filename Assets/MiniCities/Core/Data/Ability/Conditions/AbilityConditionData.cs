using MiniCities.Core.Event;
using MiniCities.Runtime.Game;

namespace _.Core.Data.Ability.Conditions
{
    public abstract class AbilityConditionData
    {
        public abstract bool Evaluate(GameContext ctx, IEvent evt);
    }
}