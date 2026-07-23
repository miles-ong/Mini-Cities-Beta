using System;
using _.Entities.Block;
using _.Entities.Block.Enums;
using _.Entities.Map;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapData))]
public class MapDataEditor : Editor
{
    private MapData _data;
    private BlockRegistry _registry;

    private const float Spacing = 10f;
    private const float ViewPadding = 50f;
    
    [CanBeNull] private BlockData _selectedBlock;
    private int _selectedLayer;
    private float _viewWidth;
    
    private void OnEnable()
    {
        _data = (MapData)target;
        FindRegistry();
    }

    public override void OnInspectorGUI()
    {
        if (_registry is null) return;

        _viewWidth = EditorGUIUtility.currentViewWidth - ViewPadding;

        DrawMap();

        EditorGUILayout.Space(Spacing);

        DrawTools();
    }

    private void DrawMap()
    {
        EditorGUILayout.LabelField("Map", EditorStylesUtility.Header);
        EditorGUILayout.Space(Spacing);
        
        DrawLayer();
        DrawLayerControls();
    }
    
    private void DrawTools()
    {
        EditorGUILayout.LabelField("Tools", EditorStylesUtility.Header);
        EditorGUILayout.Space(Spacing);
        
        DrawBrushes();

        EditorGUILayout.Space(Spacing);
        
        DrawDimensions();
        
        EditorGUILayout.Space(Spacing);
        
        DrawModifiers();

        EditorGUILayout.Space(Spacing);
    }

    private void DrawLayer()
    {
        EditorGUILayout.LabelField($"Layer {_selectedLayer + 1} out of {_data.Height}", EditorStylesUtility.SubHeader);
        EditorGUILayout.Space(Spacing);
        
        for (var z = 0; z < _data.Length; z++)
        {
            EditorGUILayout.BeginHorizontal();

            for (var x = 0; x < _data.Width; x++) DrawTile(new Vector3Int(x, _selectedLayer, z));
            
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawTile(Vector3Int coord)
    {
        var block = _data[coord];
        var label = block is not null ? block.name : "Empty";
        var buttonSize = _viewWidth / _data.Width;
        var rect = GUILayoutUtility.GetRect(buttonSize, buttonSize);

        EditorGUI.DrawRect(rect, block?.group.ToColor() ?? Color.gray);
        GUI.Label(rect, label, EditorStylesUtility.TileLabel);

        HandleTileInput(rect, coord, block);
    }

    private void DrawLayerControls()
    {
        if (_data.Height <= 1) return;

        EditorGUILayout.Space(Spacing);
        
        var buttonWidth = _viewWidth / 2;
        const float buttonHeight = 30f;
        
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Previous Layer", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            _selectedLayer = Math.Max(_selectedLayer - 1, 0);

        if (GUILayout.Button("Next Layer", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            _selectedLayer = Math.Min(_selectedLayer + 1, _data.Height - 1);
            
        EditorGUILayout.EndHorizontal();
    }

    private void DrawBrushes()
    {
        EditorGUILayout.LabelField("Brushes", EditorStylesUtility.SubHeader);
        EditorGUILayout.Space(Spacing);
        
        var blocks = _registry.blocks;
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
        EditorGUILayout.LabelField("Dimensions", EditorStylesUtility.SubHeader);
        EditorGUILayout.Space(Spacing);
        
        var newLength = EditorGUILayout.IntSlider("Length", _data.Length, 1, 10);
        var newWidth = EditorGUILayout.IntSlider("Width", _data.Width, 1, 10);
        var newHeight = EditorGUILayout.IntSlider("Height", _data.Height, 1, 5);
        
        var newDimensions = new Vector3Int(newWidth, newHeight, newLength);

        if (newDimensions == _data.grid.Dimensions) return;
        
        _data.grid.Resize(newDimensions);
        MarkDirty();
    }

    private void DrawModifiers()
    {
        EditorGUILayout.LabelField("Modifiers", EditorStylesUtility.SubHeader);
        EditorGUILayout.Space(Spacing);

        _data.terrainFactor = EditorGUILayout.IntSlider("Terrain Factor", _data.terrainFactor, 1, 5);
    }

    private void HandleTileInput(Rect rect, Vector3Int coord, BlockData block)
    {
        var e = Event.current;

        if (!rect.Contains(e.mousePosition)) return;

        switch (e.type)
        {
            case EventType.MouseDown:
                switch (e.button)
                {
                    case <= 1: // Left Click & Right Click
                        _data[coord] = e.button == 0 ?  _selectedBlock : null;
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
                    _data[coord] = e.button == 0 ? _selectedBlock : null;
                    MarkDirty();
                    e.Use();
                }
                break;
        }
    }

    private void FindRegistry()
    {
        var guids = AssetDatabase.FindAssets("t:BlockRegistry");

        switch (guids.Length)
        {
            case 0: Debug.LogError("No BlockRegistry found"); return;
            case 1: Debug.Log("BlockRegistry found"); break;
            case > 1: Debug.LogWarning("Multiple BlockRegistry found. Using the first one"); break;
        }
        
        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var registry = AssetDatabase.LoadAssetAtPath<BlockRegistry>(path);
        
        if (registry is null) Debug.LogError($"Failed to load BlockRegistry at '{path}'");

        _registry = registry;
    }

    private void MarkDirty() => EditorUtility.SetDirty(_data);
}
