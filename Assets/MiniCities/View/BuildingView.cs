using MiniCities.Definitions.Blueprint.Stat;
using MiniCities.Systems.City.Stat;
using MiniCities.View.Popup;
using UnityEngine;

namespace MiniCities.View
{
    public class BuildingView : BlockView
    {
        [Header("Popup")]
        [SerializeField] protected PopupViewStack popupStack;
        [SerializeField] protected TextPopupView textPopup;

        public void TelegraphStatChange(BuildingStat stat, int amount)
        {
            var text = $"{(amount >= 0 ? "+" : string.Empty)}{amount} {stat}";
            popupStack.Push(textPopup, p => p.Show(text, stat.ToCityStat().ToColor()));
        }
    }
}