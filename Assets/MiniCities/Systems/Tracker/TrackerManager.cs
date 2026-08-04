using MiniCities.Core.Event;
using MiniCities.Core.Grid;
using UnityEngine;

namespace MiniCities.Systems.Tracker
{
    public class TrackerManager : MonoBehaviour
    {
        [SerializeField] private Camera camera;

        [SerializeField] private SelectionTracker selectionTracker;
        [SerializeField] private PlacementTracker placementTracker;

        private GridRaycaster _raycaster;

        private void Start()
        {
            _raycaster = new GridRaycaster(camera);

            placementTracker.SetRaycaster(_raycaster);

            EventBus<BlueprintSelectedEvent>.OnEvent += OnBlueprintSelected;
        }

        private void OnBlueprintSelected(BlueprintSelectedEvent evt)
        {
            selectionTracker.enabled = false;
            placementTracker.Begin(evt.Blueprint);
        }
    }
}