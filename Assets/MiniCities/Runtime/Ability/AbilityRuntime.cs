using System.Collections.Generic;
using System.Linq;
using MiniCities.Definitions.Ability;

namespace MiniCities.Runtime.Ability
{
    public class AbilityRuntime
    {
        public AbilityTrigger Trigger;
        public List<IAbilityCondition> Conditions;
        public List<IAbilityEffect> Effects;
        
        public AbilityRuntime(AbilityDefinition definition)
        {
            Trigger = definition.trigger;
            Conditions = definition.conditions.Select(e => e.Clone()).ToList();
            Effects = definition.effects.Select(e => e.Clone()).ToList();
        }
    }
}