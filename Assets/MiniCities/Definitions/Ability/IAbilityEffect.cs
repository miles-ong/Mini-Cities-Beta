using System;
using MiniCities.Core;
using MiniCities.Definitions.Blueprint;

namespace MiniCities.Definitions.Ability
{
    public interface IAbilityEffect
    {
        public void Activate(AbilityContext ctx);
        public IAbilityEffect Clone();
    }

    [Serializable]
    public class ModifyStatEffect : IAbilityEffect
    {
        public BuildingStat stat;
        public Operation operation;
        public float amount;

        public void Activate(AbilityContext ctx)
        {
            ctx.Building.BaseStats.ModifyStat(stat, operation, amount);
            ctx.Building.MarkDirty();
        }

        public IAbilityEffect Clone() => new ModifyStatEffect()
        {
            stat = stat,
            operation = operation,
            amount = amount
        };
    }
}