using MiniCities.Core.Math;
using MiniCities.Systems.City.Stat;

namespace MiniCities.Systems.City.Buff
{
    public sealed class CityBuff
    {
        public IBuffSource Source { get; }
        public CityStat Stat { get; }
        public Operation Operation { get; }
        public float Amount { get; }

        public CityBuff(IBuffSource source, CityStat stat, Operation operation, float amount)
        {
            Source = source;
            Stat = stat;
            Operation = operation;
            Amount = amount;
        }
    }
}