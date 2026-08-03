using System.Collections.Generic;
using MiniCities.Runtime.Blueprint;
using UnityEngine;

namespace MiniCities.UI.Blueprint
{
    public class BlueprintHandUI : MonoBehaviour
    {
        [SerializeField] private BlueprintUI blueprintPrefab;
        [SerializeField] private RectTransform container;

        private readonly List<BlueprintUI> _blueprints = new();

        public void Add(BlueprintRuntime blueprint)
        {
            var bpUI = Instantiate(blueprintPrefab, container);

            bpUI.Set(blueprint);

            _blueprints.Add(bpUI);
        }

        public void Remove(BlueprintRuntime blueprint)
        {
            var bpUI = _blueprints.Find(b => b.Blueprint == blueprint);

            if (bpUI is null) return;

            _blueprints.Remove(bpUI);
            Destroy(bpUI.gameObject);
        }
    }
}