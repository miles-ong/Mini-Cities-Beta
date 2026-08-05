using System;
using MiniCities.Core;
using MiniCities.Definitions.Blueprint.Stat;
using MiniCities.Systems.City.Stat;

namespace MiniCities.Definitions.Ability
{
    public interface IAbilityEffect
    {
        public void Activate(AbilityContext ctx);
        public string Describe();
        public IAbilityEffect Clone();
    }

    [Serializable]
    public class ModifyBuildingBaseStat : IAbilityEffect
    {
        public BuildingStat stat;
        public Operation operation;
        public float amount;

        public void Activate(AbilityContext ctx)
        {
            ctx.Building.BaseStats.Modify(stat, operation, amount);
            ctx.Building.MarkDirty();
        }

        public string Describe() => $"{operation.ToDescriptionText(amount)} {stat}";
        
        public IAbilityEffect Clone() => new ModifyBuildingBaseStat()
        {
            stat = stat,
            operation = operation,
            amount = amount
        };
    }

    [Serializable]
    public class ModifyCityBaseStat : IAbilityEffect
    {
        public CityStat stat = 0;
        public Operation operation = 0;
        public float amount;

        public void Activate(AbilityContext ctx) => ctx.Game.City.ModifyBaseStat(stat, operation, amount);

        public string Describe() => $"{operation.ToDescriptionText(amount)} {stat}";

        public IAbilityEffect Clone() => new ModifyCityBaseStat()
        {
            stat = stat,
            operation = operation,
            amount = amount
        };
    }
}