using System;
using MiniCities.Core;
using MiniCities.Definitions.Blueprint;

namespace MiniCities.Definitions.Ability
{
    public interface IAbilityEffect
    {
        public void Activate(AbilityContext ctx);
        public string Describe();
        public IAbilityEffect Clone();
    }

    [Serializable]
    public class ModifyBaseStatEffect : IAbilityEffect
    {
        public BuildingStat stat;
        public Operation operation;
        public float amount;

        public void Activate(AbilityContext ctx)
        {
            ctx.Building.BaseStats.ModifyStat(stat, operation, amount);
            ctx.Building.MarkDirty();
        }

        public string Describe()
        {
            var valueText = operation switch
            {
                Operation.Add => $"+{amount}",
                Operation.Subtract => $"-{amount}",
                Operation.Multiply => $"x{amount}",
                Operation.Divide => $"/{amount}",
                _ => amount.ToString("0.##")
            };

            return $"{valueText} {stat}";
        }

        public IAbilityEffect Clone() => new ModifyBaseStatEffect()
        {
            stat = stat,
            operation = operation,
            amount = amount
        };
    }
}