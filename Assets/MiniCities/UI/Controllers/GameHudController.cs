using MiniCities.Core.Event;
using MiniCities.Systems.City;
using MiniCities.UI.Controllers.Hand;
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
        
        [Header("References")]
        [SerializeField] private CityManager cityManager;
        [SerializeField] private VisualTreeAsset modernBlueprintTemplate;

        [Header("Settings")] 
        [SerializeField] private BlueprintHandSettings blueprintHandSettings;

        private PanelRenderer _panelRenderer;

        private VisualElement _statsPlate;
        private Label _infoPlate;
        private VisualElement _blueprintHand;
        
        private Plate _currentPlate;
        private string _info = "";

        private BlueprintHandUI _blueprintHandUI;

        private void Awake()
        {
            _panelRenderer = GetComponent<PanelRenderer>();
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);

            // Level events
            GameEventBus<LevelStartedEvent>.OnEvent += OnLevelStarted;
            GameEventBus<EnterShopEvent>.OnEvent += OnEnterShop;
            GameEventBus<EnterLevelSelectorEvent>.OnEvent += OnEnterLevelSelector;

            // Blueprint events
            GameEventBus<BlueprintDrawnEvent>.OnEvent += OnBlueprintDrawn;
            GameEventBus<BlueprintUsedEvent>.OnEvent += OnBlueprintUsed;
            
            // Camera events
            GameEventBus<CameraRotationStartedEvent>.OnEvent += OnCameraRotationStarted;
            GameEventBus<CameraRotationEndedEvent>.OnEvent += OnCameraRotationEnded;
        }

        private void OnDestroy()
        {
            _panelRenderer.UnregisterUIReloadCallback(OnUIReload);
            
            // Level events
            GameEventBus<LevelStartedEvent>.OnEvent -= OnLevelStarted;
            GameEventBus<EnterShopEvent>.OnEvent -= OnEnterShop;
            GameEventBus<EnterLevelSelectorEvent>.OnEvent -= OnEnterLevelSelector;
            
            // Blueprint events
            GameEventBus<BlueprintDrawnEvent>.OnEvent -= OnBlueprintDrawn;
            GameEventBus<BlueprintUsedEvent>.OnEvent -= OnBlueprintUsed;
            
            // Camera events
            GameEventBus<CameraRotationStartedEvent>.OnEvent -= OnCameraRotationStarted;
            GameEventBus<CameraRotationEndedEvent>.OnEvent -= OnCameraRotationEnded;
        }

        private void OnUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            root.dataSource = cityManager;

            _statsPlate = root.Q<VisualElement>("StatsPlate");
            _infoPlate = root.Q<Label>("InfoPlate");
            _blueprintHand = root.Q<VisualElement>("BlueprintHand");

            _blueprintHandUI = new BlueprintHandUI(
                _blueprintHand,
                blueprintHandSettings,
                modernBlueprintTemplate);

            RefreshUI();
        }
        
        #region Level Events

        private void OnLevelStarted(LevelStartedEvent evt)
        {
            _currentPlate = Plate.Stats;

            RefreshUI();
        }

        private void OnEnterShop(EnterShopEvent evt)
        {
            _currentPlate = Plate.Title;
            _info = "Shop";

            RefreshUI();
        }

        private void OnEnterLevelSelector(EnterLevelSelectorEvent evt)
        {
            _currentPlate = Plate.Title;
            _info = "Select Level";

            RefreshUI();
        }
        
        #endregion
        
        #region Blueprint Events

        private void OnBlueprintDrawn(BlueprintDrawnEvent evt) => _blueprintHandUI?.Add(evt.Blueprint);
        private void OnBlueprintUsed(BlueprintUsedEvent evt) => _blueprintHandUI?.Remove(evt.Blueprint);
        
        #endregion
        
        #region Camera Events

        private void OnCameraRotationStarted(CameraRotationStartedEvent evt)
        {
            _blueprintHandUI?.SetHandLowered(true);
        }

        private void OnCameraRotationEnded(CameraRotationEndedEvent evt)
        {
            _blueprintHandUI?.SetHandLowered(false);
        }
        
        #endregion
        
        private void RefreshUI()
        {
            if (_statsPlate == null || _infoPlate == null) return;

            _infoPlate.text = _info;
            
            var showStats = _currentPlate == Plate.Stats;
            
            UpdatePlate(_statsPlate, showStats);
            UpdatePlate(_infoPlate, !showStats);
        }

        private static void UpdatePlate(VisualElement plate, bool display)
        {
            if (display) plate.AddToClassList("showDisplayPlate");
            else plate.RemoveFromClassList("showDisplayPlate");
        }
    }
}
