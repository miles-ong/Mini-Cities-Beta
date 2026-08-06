using MiniCities.Core.Event;
using MiniCities.Core.Grid;
using MiniCities.Systems.Map;
using UnityEngine;

namespace MiniCities.Systems.Tracker
{
    public class TrackerManager : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private MapManager mapManager;
        [SerializeField] private SelectionTracker selectionTracker;
        [SerializeField] private PlacementTracker placementTracker;
        
        private GridRaycaster _raycaster;
        
        private void Start()
        {
            _raycaster = new GridRaycaster(camera);

            selectionTracker.Initialize(_raycaster);
            placementTracker.Initialize(_raycaster, mapManager);

            EventBus<BlueprintSelectedEvent>.OnEvent += OnBlueprintSelected;
            EventBus<BlueprintUsedEvent>.OnEvent += OnBlueprintUsed;
        }

        private void OnBlueprintSelected(BlueprintSelectedEvent e)
        {
            selectionTracker.End();
            placementTracker.Begin(e.Blueprint);
        }

        private void OnBlueprintUsed(BlueprintUsedEvent e)
        {
            selectionTracker.Begin();
            placementTracker.End();
        }
    }
}