using System.Linq;
using MiniCities.Core.Definitions.Ability;

namespace MiniCities.Runtime.Game.AbilitySystem
{
    public class AbilityRuntime
    {
        public AbilityTrigger Trigger;
        public IAbilityCondition[] Conditions;
        public IAbilityEffect[] Effects;
        
        public AbilityRuntime(AbilityDefinition definition)
        {
            Trigger = definition.trigger;
            Conditions = definition.conditions.Select(e => e.Clone()).ToArray();
            Effects = definition.effects.Select(e => e.Clone()).ToArray();
        }
    }
}