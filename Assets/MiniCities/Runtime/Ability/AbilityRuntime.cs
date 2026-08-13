using System.Collections.Generic;
using System.Linq;
using MiniCities.Definitions.Ability;
using MiniCities.Definitions.Ability.Trigger;

namespace MiniCities.Runtime.Ability
{
    public class AbilityRuntime
    {
        public readonly AbilityTrigger Trigger;
        public readonly List<IAbilityCondition> Conditions;
        public readonly List<IAbilityEffect> Effects;
        
        public AbilityRuntime(AbilityDefinition definition)
        {
            Trigger = definition.trigger.Clone();
            Conditions = definition.conditions.Select(e => e.Clone()).ToList();
            Effects = definition.effects.Select(e => e.Clone()).ToList();
        }

        public void Activate(AbilityContext context)
        {
            foreach (var condition in Conditions)
                if (!condition.Evaluate(context))
                    return;

            foreach (var effect in Effects)
            {
                effect.Activate(context);
                context.Activator.TelegraphAbilityUsage();
            }
        }
    }
}