using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniCities.Systems.City.Buff
{
    public class BuildingBuffIndex
    {
        private readonly HashSet<BuildingBuff> _buffs = new();
        
        private readonly Dictionary<IBuffSource, HashSet<BuildingBuff>> _sourceIndex = new();
        private readonly Dictionary<IBuffReceiver, HashSet<BuildingBuff>> _receiverIndex = new();
        private readonly Dictionary<Vector3Int, HashSet<BuildingBuff>> _locationIndex = new();
        
        public void Register(BuildingBuff buff)
        {
            if (!_buffs.Add(buff)) return;

            if (!_sourceIndex.TryGetValue(buff.Source, out var sBuffs))
            {
                sBuffs = new HashSet<BuildingBuff>();
                _sourceIndex.Add(buff.Source, sBuffs);
            }

            sBuffs.Add(buff);

            if (buff.Receiver != null)
            {
                if (!_receiverIndex.TryGetValue(buff.Receiver, out var rBuffs))
                {
                    rBuffs = new HashSet<BuildingBuff>();
                    _receiverIndex.Add(buff.Receiver, rBuffs);
                }

                rBuffs.Add(buff);
            }

            if (buff.Location == null) return;
            
            if (!_locationIndex.TryGetValue(buff.Location.Value, out var lBuffs))
            {
                lBuffs = new HashSet<BuildingBuff>();
                _locationIndex.Add(buff.Location.Value, lBuffs);
                Debug.Log($"Registering new buff-list at location ({buff.Location.Value})");
            }

            lBuffs.Add(buff);
        }

        public void Unregister(BuildingBuff buff)
        {
            if (!_buffs.Remove(buff)) return;

            if (_sourceIndex.TryGetValue(buff.Source, out var sBuffs))
            {
                sBuffs.Remove(buff);
                
                if (sBuffs.Count == 0) _sourceIndex.Remove(buff.Source);
            }

            if (buff.Receiver != null && _receiverIndex.TryGetValue(buff.Receiver, out var rBuffs))
            {
                rBuffs.Remove(buff);
                
                if (rBuffs.Count == 0) _receiverIndex.Remove(buff.Receiver);
            }

            if (buff.Location == null || !_locationIndex.TryGetValue(buff.Location.Value, out var lBuffs)) return;
            
            lBuffs.Remove(buff);
                
            if (lBuffs.Count == 0) _locationIndex.Remove(buff.Location.Value);
        }

        public void RemoveSource(IBuffSource source)
        {
            if (!_sourceIndex.TryGetValue(source, out var buffs)) return;
            
            foreach (var b in buffs.ToArray()) Unregister(b);
        }
        
        public void RemoveReceiver(IBuffReceiver receiver)
        {
            if (!_receiverIndex.TryGetValue(receiver, out var buffs)) return;
            
            foreach (var b in buffs.ToArray()) Unregister(b);
        }
        
        public IEnumerable<BuildingBuff> GetSourceBuffs(IBuffSource source) =>
            _sourceIndex.TryGetValue(source, out var buffs) ? buffs : Enumerable.Empty<BuildingBuff>();

        public IEnumerable<BuildingBuff> GetReceiverBuffs(IBuffReceiver receiver) =>
            _receiverIndex.TryGetValue(receiver, out var buffs) ? buffs : Enumerable.Empty<BuildingBuff>();

        public IEnumerable<BuildingBuff> GetLocationalBuffs(Vector3Int location) =>
            _locationIndex.TryGetValue(location, out var buffs) ? buffs : Enumerable.Empty<BuildingBuff>();
        
        public void Clear()
        {
            _buffs.Clear();
            _receiverIndex.Clear();
            _locationIndex.Clear();
            _sourceIndex.Clear();
        }
    }
}