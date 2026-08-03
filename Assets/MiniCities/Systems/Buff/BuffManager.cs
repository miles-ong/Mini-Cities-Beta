using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniCities.Systems.BuffSys
{
    public class BuffManager
    {
        private readonly HashSet<Buff> _buffs = new();
        private readonly Dictionary<IBuffSource, HashSet<Buff>> _sourceIndex = new();
        private readonly Dictionary<IBuffReceiver, HashSet<Buff>> _receiverIndex = new();
        private readonly Dictionary<Vector3Int, HashSet<Buff>> _locationIndex = new();
        
        public void Register(Buff buff)
        {
            if (!_buffs.Add(buff)) return;

            if (buff.Receiver is not null)
            {
                if (!_receiverIndex.TryGetValue(buff.Receiver, out var rBuffs))
                {
                    rBuffs = new HashSet<Buff>();
                    _receiverIndex.Add(buff.Receiver, rBuffs);
                }

                rBuffs.Add(buff);
            }
            
            if (buff.Location is not null)
            {
                if (!_locationIndex.TryGetValue(buff.Location.Value, out var lBuffs))
                {
                    lBuffs = new HashSet<Buff>();
                    _locationIndex.Add(buff.Location.Value, lBuffs);
                }

                lBuffs.Add(buff);
            }

            if (!_sourceIndex.TryGetValue(buff.Source, out var sBuffs))
            {
                sBuffs = new HashSet<Buff>();
                _sourceIndex.Add(buff.Source, sBuffs);
            }

            sBuffs.Add(buff);
            
            MarkDirty(buff);
        }

        public void Unregister(Buff buff)
        {
            if (!_buffs.Remove(buff)) return;

            if (buff.Receiver is not null && _receiverIndex.TryGetValue(buff.Receiver, out var rBuffs))
            {
                rBuffs.Remove(buff);

                if (rBuffs.Count == 0) _receiverIndex.Remove(buff.Receiver);
            }

            if (buff.Location is not null && _locationIndex.TryGetValue(buff.Location.Value, out var lBuffs))
            {
                lBuffs.Remove(buff);
                
                if (lBuffs.Count == 0) _locationIndex.Remove(buff.Location.Value);
            }

            if (!_sourceIndex.TryGetValue(buff.Source, out var sBuffs)) return;
            
            sBuffs.Remove(buff);

            if (sBuffs.Count == 0) _sourceIndex.Remove(buff.Source);

            MarkDirty(buff);
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

        public IEnumerable<Buff> GetReceiverBuffs(IBuffReceiver receiver) =>
            _receiverIndex.TryGetValue(receiver, out var buffs) ? buffs : Enumerable.Empty<Buff>();

        public IEnumerable<Buff> GetLocationalBuffs(Vector3Int location) =>
            _locationIndex.TryGetValue(location, out var buffs) ? buffs : Enumerable.Empty<Buff>();
        
        public void Clear()
        {
            _buffs.Clear();
            _receiverIndex.Clear();
            _locationIndex.Clear();
            _sourceIndex.Clear();
        }

        private static void MarkDirty(Buff buff)
        {
            buff.Receiver?.MarkDirty();

            if (buff.Location is not null)
            {
                // fix
            }
        }
    }
}