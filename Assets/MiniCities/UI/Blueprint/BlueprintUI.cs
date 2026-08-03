using System;
using MiniCities.Definitions.Blueprint;
using MiniCities.Runtime.Blueprint;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiniCities.UI.Blueprint
{
    public class BlueprintUI : 
        MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        [SerializeField] private Image background;
        [SerializeField] private Image nameplate;
        [SerializeField] private TMP_Text displayName;
        [SerializeField] private Image populationIcon;
        [SerializeField] private Image pollutionIcon;
        [SerializeField] private Image cultureIcon;
        [SerializeField] private TMP_Text populationNumber;
        [SerializeField] private TMP_Text pollutionNumber;
        [SerializeField] private TMP_Text cultureNumber;
        [SerializeField] private TMP_Text description;

        public BlueprintRuntime Blueprint { get; private set; }

        public event Action<BlueprintUI> Hovered;
        public event Action<BlueprintUI> Unhovered;
        public event Action<BlueprintUI> Clicked;
        
        public void Set(BlueprintRuntime blueprint)
        {
            Blueprint = blueprint;

            background.color = blueprint.Category.ToColor();
            nameplate.color = blueprint.Category.ToColor();

            Debug.Log(blueprint.Category.ToColor());
            Debug.Log(background.color);
            
            displayName.text = blueprint.DisplayName;
            HandleIcon(populationIcon, populationNumber, blueprint.Stats.Population);
            HandleIcon(pollutionIcon, pollutionNumber, blueprint.Stats.Pollution);
            HandleIcon(cultureIcon, cultureNumber, blueprint.Stats.Culture);
            description.text = blueprint.Description; // make description builder soon
        }

        public void Refresh() => Set(Blueprint);

        public void OnPointerEnter(PointerEventData e) => Hovered?.Invoke(this);
        public void OnPointerExit(PointerEventData e) => Unhovered?.Invoke(this);
        public void OnPointerClick(PointerEventData e) => Clicked?.Invoke(this);

        private static void HandleIcon(Image icon, TMP_Text tmp, int number)
        {
            if (number == 0)
            {
                icon.enabled = false;
                tmp.enabled = false;
                return;
            }

            icon.enabled = true;
            tmp.enabled = true;
            tmp.text = number.ToString();
        }
    }
}