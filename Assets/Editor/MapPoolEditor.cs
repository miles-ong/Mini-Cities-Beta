using System;
using System.Linq;
using MiniCities.Definitions.Map;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapPool))]
public class MapPoolEditor : RegistryEditor<MapPool, MapRegistry>
{
    public override void OnInspectorGUI()
    {
        if (Registry is null) return;
        
        EditorGUILayout.LabelField("Map Pool", EditorStylesUtility.Header);
        
        EditorGUILayout.Space(EditorStylesUtility.LargeSpacing);

        DrawStats();

        foreach (MapSize s in Enum.GetValues(typeof(MapSize)))
        {
            EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
            
            DrawMapToggles(s);
        }
    }

    private void DrawStats()
    {
        EditorGUILayout.LabelField("Stats", EditorStylesUtility.Subheader);
        
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        EditorGUILayout.LabelField($"Total Enabled Maps: {Target.maps.Items.Count}");
        
        EditorGUILayout.Space(EditorStylesUtility.SmallSpacing);

        foreach (MapSize s in Enum.GetValues(typeof(MapSize)))
        {
            var count = Target.GetBySize(s).Count;
            
            EditorGUILayout.LabelField($"{s} Maps: {count}");
        }
    }

    private void DrawMapToggles(MapSize size)
    {
        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField($"{size} Maps", EditorStylesUtility.Subheader);

        var enableAll = false;
        var disableAll = false;

        if (GUILayout.Button("Enable All")) enableAll = true;
        
        if (GUILayout.Button("Disable All"))
        {
            enableAll = false;
            disableAll = true;
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

        foreach (var m in Registry.maps.Where(m => m.size == size))
        {
            if (enableAll) Target.maps.Add(m);
            if (disableAll) Target.maps.Remove(m);
            
            var contains = Target.maps.Contains(m);

            var toggle = EditorGUILayout.Toggle(m.displayName, contains);

            if (contains == toggle) continue;
            
            if (toggle) Target.maps.Add(m);
            else Target.maps.Remove(m);

            MarkDirty();
        }
    }
}