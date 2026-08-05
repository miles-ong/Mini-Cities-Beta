using System.Collections.Generic;
using System.Linq;

namespace MiniCities.Systems.City.Buff
{
    public class CityBuffIndex
    {
        private readonly HashSet<CityBuff> _buffs = new();
        private readonly Dictionary<IBuffSource, HashSet<CityBuff>> _sourceIndex = new();

        public IReadOnlyCollection<CityBuff> Buffs => _buffs;

        public void Register(CityBuff buff)
        {
            if (!_buffs.Add(buff)) return;

            if (!_sourceIndex.TryGetValue(buff.Source, out var buffs))
            {
                buffs = new HashSet<CityBuff>();
                _sourceIndex.Add(buff.Source, buffs);
            }

            buffs.Add(buff);
        }

        public void Unregister(CityBuff buff)
        {
            if (!_buffs.Remove(buff)) return;

            if (!_sourceIndex.TryGetValue(buff.Source, out var buffs)) return;

            buffs.Remove(buff);

            if (buffs.Count == 0) _sourceIndex.Remove(buff.Source);
        }

        public void RemoveSource(IBuffSource source)
        {
            if (!_sourceIndex.TryGetValue(source, out var buffs)) return;
            
            foreach (var b in buffs.ToArray()) Unregister(b);
        }

        public void Clear()
        {
            _buffs.Clear();
            _sourceIndex.Clear();
        }
    }
}