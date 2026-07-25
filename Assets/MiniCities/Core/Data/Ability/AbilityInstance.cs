using System.Linq;
using MiniCities.Core.Data.Ability.Conditions;
using MiniCities.Core.Data.Ability.Effects;
using MiniCities.Core.Data.Ability.Enums;

namespace MiniCities.Core.Data.Ability
{
    public class AbilityInstance
    {
        public AbilityTrigger Trigger;
        public IAbilityCondition[] Conditions;
        public IAbilityEffect[] Effects;
        
        public AbilityInstance(AbilityDefinition definition)
        {
            Trigger = definition.trigger;
            Conditions = definition.conditions.Select(e => e.Clone()).ToArray();
            Effects = definition.effects.Select(e => e.Clone()).ToArray();
        }
    }
}