using UnityEngine.UIElements;

[UxmlElement]
public partial class BlueprintElement : VisualElement
{
    private readonly string _styleClassName = "blueprintElement";
    private readonly string _styleContainerName = "blueprintElementContainer";
    private readonly string _styleStatContainerName = "blueprintElementStatContainer";
    
    
    private VisualElement _card;
    private Label _nameplate;
    private Image _buildingDisplay;
    private VisualElement _statContainer;
    private VisualElement _populationContainer;
    private VisualElement _cultureContainer;
    private VisualElement _pollutionContainer;
    private Image _populationIcon;
    private Image _cultureIcon;
    private Image _pollutionIcon;
    private Label _populationValue;
    private Label _cultureValue;
    private Label _pollutionValue;
    private VisualElement _infoContainer;
    private Label _description;

    public BlueprintElement()
    {
        
        Add(_card = new VisualElement());
        /*_card.AddToClassList("blueprintElementContainer");*/
        Add(_nameplate = new Label());
    }
}