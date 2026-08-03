using MiniCities.Definitions.Map;
using MiniCities.Systems.MapSys;
using UnityEngine;

namespace MiniCities.Systems.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MapManager mapManager;
        [SerializeField] private MapDefinition testMap;
        
        private void Start()
        {
            mapManager.LoadMap(testMap);
        }
    }
}