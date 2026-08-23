using MiniCities.Core.Event;
using MiniCities.Systems.City;
using UnityEngine;
using UnityEngine.UIElements;

namespace MiniCities.UI.Controllers
{
    [RequireComponent(typeof(PanelRenderer))]
    public class GameHudController : MonoBehaviour
    {
        private enum Plate
        {
            Stats,
            Title
        }
        
        [SerializeField] private CityManager cityManager;

        private PanelRenderer _panelRenderer;

        private VisualElement _statsPlate;
        private VisualElement _titlePlate;
        
        private Label _titleLabel;

        private Plate _currentPlate;
        private string _title;

        private void Awake()
        {
            _panelRenderer = GetComponent<PanelRenderer>();
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);

            GameEventBus<LevelStartedEvent>.OnEvent += OnLevelStarted;
            GameEventBus<EnterShopEvent>.OnEvent += OnEnterShop;
            GameEventBus<EnterLevelSelectorEvent>.OnEvent += OnEnterLevelSelector;
        }

        private void OnDestroy()
        {
            _panelRenderer.UnregisterUIReloadCallback(OnUIReload);
            
            GameEventBus<LevelStartedEvent>.OnEvent -= OnLevelStarted;
            GameEventBus<EnterShopEvent>.OnEvent -= OnEnterShop;
            GameEventBus<EnterLevelSelectorEvent>.OnEvent -= OnEnterLevelSelector;
        }

        private void OnUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            root.dataSource = cityManager;

            _statsPlate = root.Q<VisualElement>("StatsPlate");
            _titlePlate = root.Q<VisualElement>("TitlePlate");
            _titleLabel = root.Q<Label>("TitleLabel");

            RefreshUI();
        }

        private void OnLevelStarted(LevelStartedEvent evt)
        {
            _currentPlate = Plate.Stats;

            RefreshUI();
        }

        private void OnEnterShop(EnterShopEvent evt)
        {
            _currentPlate = Plate.Title;
            _title = "Shop";

            RefreshUI();
        }

        private void OnEnterLevelSelector(EnterLevelSelectorEvent evt)
        {
            _currentPlate = Plate.Title;
            _title = "Select Level";

            RefreshUI();
        }

        private void RefreshUI()
        {
            if (_statsPlate == null || _titlePlate == null || _titleLabel == null) return;

            if (_title != null) _titleLabel.text = _title;
            
            var showStats = _currentPlate == Plate.Stats;
            
            UpdatePlate(_statsPlate, showStats);
            UpdatePlate(_titlePlate, !showStats);
        }

        private static void UpdatePlate(VisualElement plate, bool display)
        {
            if (display) plate.AddToClassList("showDisplayPlate");
            else plate.RemoveFromClassList("showDisplayPlate");
        }
    }
}
