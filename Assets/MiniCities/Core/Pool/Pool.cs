using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniCities.Core.Pool
{
    [Serializable]
    public class Pool<T>
    {
        [SerializeField] private List<T> items = new();
        
        public IReadOnlyList<T> Items => items;

        public void Add(T item)
        {
            if (!items.Contains(item)) items.Add(item);
        }

        public void Remove(T item)
        {
            if (items.Contains(item)) items.Remove(item);
        }

        public bool Contains(T item) => items.Contains(item);

        public Pool<T> Clone()
        {
            var clonedPool = new Pool<T>();

            foreach (var item in items) clonedPool.Add(item);

            return clonedPool;
        }
    }
}