using System;
using System.Collections.Generic;

namespace MiniCities.Core.Pool
{
    [Serializable]
    public class Pool<T>
    {
        private List<T> _items = new();
        
        public IReadOnlyList<T> Items => _items;

        public void Add(T item)
        {
            if (!_items.Contains(item)) _items.Add(item);
        }

        public void Remove(T item)
        {
            if (_items.Contains(item)) _items.Remove(item);
        }

        public bool Contains(T item) => _items.Contains(item);
    }
}