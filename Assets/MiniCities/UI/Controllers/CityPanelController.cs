using MiniCities.Systems.City;
using UnityEngine;
using UnityEngine.UIElements;

namespace MiniCities.UI.Controllers
{
    [RequireComponent(typeof(PanelRenderer))]
    public class CityPanelController : MonoBehaviour
    {
        [SerializeField] private CityManager cityManager;

        private PanelRenderer _panelRenderer;

        private void Awake()
        {
            _panelRenderer = GetComponent<PanelRenderer>();
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }

        private void OnDestroy() => _panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        private void OnUIReload(PanelRenderer panelRenderer, VisualElement root, int version) => root.dataSource = cityManager;
    }
}
