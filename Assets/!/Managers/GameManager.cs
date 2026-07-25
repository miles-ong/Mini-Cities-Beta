using _.Entities.Map;
using _.Systems.Map;
using UnityEngine;

namespace _.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MapManager mapManager;
        [SerializeField] private MapData testMap;
        
        private void Start()
        {
            mapManager.LoadMap(testMap);
        }
    }
}