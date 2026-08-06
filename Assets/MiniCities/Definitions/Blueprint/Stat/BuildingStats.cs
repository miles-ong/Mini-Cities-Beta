using System;
using System.Collections.Generic;
using System.Linq;
using MiniCities.Core.Math;
using UnityEngine;

namespace MiniCities.Definitions.Blueprint.Stat
{
    [Serializable]
    public class BuildingStats
    {
        [SerializeField] private List<StatValue> values = new();
        
        public IEnumerable<BuildingStat> Keys => values.Select(v => v.Stat);
        
        public int Get(BuildingStat stat)
        {
            for (var i = 0; i < values.Count; i++)
                if (values[i].Stat == stat) return values[i].Value;

            return 0;
        }

        public int Modify(BuildingStat stat, Operation operation, float amount)
        {
            for (var i = 0; i < values.Count; i++)
            {
                if (values[i].Stat != stat) continue;

                var next = (int)operation.Apply(values[i].Value, amount);
                values[i] = new StatValue { Stat = stat, Value = next };

                return next;
            }

            var created = (int)operation.Apply(0, amount);
            values.Add(new StatValue { Stat = stat, Value = created });

            return created;
        }

        public bool Has(BuildingStat stat) => values.Any(v => v.Stat == stat);
        public BuildingStats Clone() => new() { values = new List<StatValue>(values) };

        public void CopyFrom(BuildingStats other)
        {
            values.Clear();
            values.AddRange(other.values);
        }
    }
}