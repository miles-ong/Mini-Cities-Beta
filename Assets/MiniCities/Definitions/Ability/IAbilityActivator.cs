using System.Collections.Generic;
using MiniCities.Runtime.Ability;

namespace MiniCities.Definitions.Ability
{
    public interface IAbilityActivator
    {
        public IReadOnlyCollection<(int index, AbilityRuntime ability)> GetAbilities(AbilityTriggerType? type = null);
        public int GetPriorityValue();
        public void TelegraphAbilityUsage();
    }
}