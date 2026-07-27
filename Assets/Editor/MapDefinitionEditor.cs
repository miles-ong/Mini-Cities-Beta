using System;
using JetBrains.Annotations;
using MiniCities.Core.Definitions.Block;
using MiniCities.Core.Definitions.Map;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapDefinition))]
public class MapDefinitionEditor : RegistryEditor<MapDefinition, BlockRegistry>
{
    private const float ViewPadding = 50f;

    private GUIStyle _tileLabel;
    
    [CanBeNull] private BlockDefinition _selectedBlock;
    private int _selectedLayer;
    private float _viewWidth;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        _tileLabel = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true
        };
    }

    public override void OnInspectorGUI()
    {
        if (Registry is null) return;

        _viewWidth = EditorGUIUtility.currentViewWidth - ViewPadding;

        DrawMap();

        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

        DrawTools();
    }

    private void DrawMap()
    {
        EditorGUILayout.LabelField("Map", EditorStylesUtility.Header);
        EditorGUILayout.Space(EditorStylesUtility.LargeSpacing);

        Target.displayName = EditorGUILayout.TextField("Name", Target.displayName);

        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        DrawLayer();
        DrawLayerControls();
    }
    
    private void DrawTools()
    {
        EditorGUILayout.LabelField("Tools", EditorStylesUtility.Header);
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        DrawBrushes();

        EditorGUILayout.Space(EditorStylesUtility.SmallSpacing);
        
        DrawDimensions();
        
        EditorGUILayout.Space(EditorStylesUtility.SmallSpacing);
        
        DrawModifiers();

        EditorGUILayout.Space(EditorStylesUtility.SmallSpacing);
    }

    private void DrawLayer()
    {
        EditorGUILayout.LabelField($"Layer {_selectedLayer + 1} out of {Target.Height}", EditorStylesUtility.Subheader);
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        for (var z = 0; z < Target.Length; z++)
        {
            EditorGUILayout.BeginHorizontal();

            for (var x = 0; x < Target.Width; x++) DrawTile(new Vector3Int(x, _selectedLayer, z));
            
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawTile(Vector3Int coord)
    {
        var block = Target[coord];
        var label = block is not null ? block.name : "Empty";
        var buttonSize = _viewWidth / Target.Width;
        var rect = GUILayoutUtility.GetRect(buttonSize, buttonSize);

        EditorGUI.DrawRect(rect, block?.group.ToColor() ?? Color.gray);
        
        if (_tileLabel is not null) GUI.Label(rect, label, _tileLabel);

        HandleTileInput(rect, coord, block);
    }

    private void DrawLayerControls()
    {
        if (Target.Height <= 1) return;

        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        var buttonWidth = _viewWidth / 2;
        const float buttonHeight = 30f;
        
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Previous Layer", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            _selectedLayer = Math.Max(_selectedLayer - 1, 0);

        if (GUILayout.Button("Next Layer", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            _selectedLayer = Math.Min(_selectedLayer + 1, Target.Height - 1);
            
        EditorGUILayout.EndHorizontal();
    }

    private void DrawBrushes()
    {
        EditorGUILayout.LabelField("Brushes", EditorStylesUtility.Subheader);
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        var blocks = Registry.blocks;
        const int cols = 5;
        var buttonSize = _viewWidth / cols;
        var rowCount = 0;
        
        EditorGUILayout.BeginHorizontal();

        foreach (var b in blocks)
        {
            GUI.backgroundColor = b == _selectedBlock ? Color.cyan : b.group.ToColor();

            if (GUILayout.Button(b.name, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                _selectedBlock = b;
            
            GUI.backgroundColor = Color.white;

            if (++rowCount % cols != 0) continue;
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
        }
        
        EditorGUILayout.EndHorizontal();
    }

    private void DrawDimensions()
    {
        EditorGUILayout.LabelField("Dimensions", EditorStylesUtility.Subheader);
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

        Target.size = (MapSize)EditorGUILayout.EnumPopup("Size", Target.size);
        
        EditorGUILayout.Space(EditorStylesUtility.SmallSpacing);
        
        var newLength = EditorGUILayout.IntSlider("Length", Target.Length, 1, 10);
        var newWidth = EditorGUILayout.IntSlider("Width", Target.Width, 1, 10);
        var newHeight = EditorGUILayout.IntSlider("Height", Target.Height, 1, 5);
        
        var newDimensions = new Vector3Int(newWidth, newHeight, newLength);

        if (newDimensions == Target.grid.Dimensions) return;
        
        Target.grid.Resize(newDimensions);
        MarkDirty();
    }

    private void DrawModifiers()
    {
        EditorGUILayout.LabelField("Modifiers", EditorStylesUtility.Subheader);
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

        Target.terrainFactor = EditorGUILayout.IntSlider("Terrain Factor", Target.terrainFactor, 1, 5);
    }

    private void HandleTileInput(Rect rect, Vector3Int coord, BlockDefinition block)
    {
        var e = Event.current;

        if (!rect.Contains(e.mousePosition)) return;

        switch (e.type)
        {
            case EventType.MouseDown:
                switch (e.button)
                {
                    case <= 1: // Left Click & Right Click
                        Target[coord] = e.button == 0 ?  _selectedBlock : null;
                        MarkDirty();
                        e.Use();
                        break;
                    
                    case 2: // Middle Click
                        if (block is not null) _selectedBlock = block;

                        e.Use();
                        break;
                }

                break;
            
            case EventType.MouseDrag:
                if (e.button <= 1)
                {
                    Target[coord] = e.button == 0 ? _selectedBlock : null;
                    MarkDirty();
                    e.Use();
                }
                break;
        }
    }
}
