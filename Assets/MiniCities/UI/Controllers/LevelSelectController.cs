using MiniCities.Core.Event;
using MiniCities.Systems.Game;
using UnityEngine;
using UnityEngine.UIElements;

namespace MiniCities.UI.Controllers
{
    [RequireComponent(typeof(PanelRenderer))]
    public class LevelSelectController : MonoBehaviour
    {
        private PanelRenderer _panelRenderer;

        private LevelContext _level1Context;
        private LevelContext _level2Context;

        private void Awake()
        {
            _panelRenderer = GetComponent<PanelRenderer>();
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }
        
        private void OnDestroy() => _panelRenderer.UnregisterUIReloadCallback(OnUIReload);

        private void OnUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            root.Q<Button>("Level1Button").clicked += SelectLevel1;
            root.Q<Button>("Level2Button").clicked += SelectLevel2;
        }

        private void SelectLevel1() => GameEventBus<LevelSelectedEvent>.Fire(new LevelSelectedEvent(_level1Context));
        private void SelectLevel2() => GameEventBus<LevelSelectedEvent>.Fire(new LevelSelectedEvent(_level2Context));
    }
}