using UnityEngine;

namespace MiniCities.Definitions.Game
{
    [CreateAssetMenu(fileName = "GAMESETTINGS_", menuName = "Mini Cities/Settings/Game")]
    public class GameSettingsDefinition : ScriptableObject
    {
        [Header("Hand")]
        public int maxHandSize;
        public int initialDrawAmount;
    }
}