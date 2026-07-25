using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniCities.Runtime.Game.Buff
{
    public class BuffManager
    {
        private readonly HashSet<Buff> _buffs = new();
        private readonly Dictionary<IBuffSource, HashSet<Buff>> _sourceIndex = new();
        private readonly Dictionary<IBuffReceiver, HashSet<Buff>> _receiverIndex = new();
        private readonly Dictionary<Vector3Int, HashSet<Buff>> _locationIndex = new();
        
        public void Register(Buff buff)
        {
            _buffs.Add(buff);

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
        }

        public void Unregister(Buff buff)
        {
            _buffs.Remove(buff);

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

        public void Clear()
        {
            _buffs.Clear();
            _receiverIndex.Clear();
            _locationIndex.Clear();
            _sourceIndex.Clear();
        }
    }
}