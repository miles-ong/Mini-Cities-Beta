using System;
using System.Linq;
using MiniCities.Core.Pool;
using MiniCities.Definitions.Blueprint;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlueprintPoolDefinition))]
public class BlueprintPoolEditor : RegistryEditor<BlueprintPoolDefinition, BlueprintRegistry>
{
    protected override void OnEnable()
    {
        base.OnEnable();

        Target.blueprints ??= new Pool<BlueprintDefinition>();
    }

    public override void OnInspectorGUI()
    {
        if (Registry is null) return;
        
        EditorGUILayout.LabelField("Blueprint Pool", EditorStylesUtility.Header);
        
        EditorGUILayout.Space(EditorStylesUtility.LargeSpacing);

        DrawStats();

        foreach (BlueprintCategory c in Enum.GetValues(typeof(BlueprintCategory)))
        {
            EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

            DrawBlueprintToggles(c);
        }
    }

    private void DrawStats()
    {
        EditorGUILayout.LabelField("Stats", EditorStylesUtility.Subheader);
        
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        EditorGUILayout.LabelField($"Total Enabled Blueprints: {Target.blueprints.Items.Count}");
        
        EditorGUILayout.Space(EditorStylesUtility.SmallSpacing);

        foreach (BlueprintCategory c in Enum.GetValues(typeof(BlueprintCategory)))
        {
            var count = Target.GetByCategory(c).Count;
            
            EditorGUILayout.LabelField($"{c} Blueprints: {count}");
        }
    }

    private void DrawBlueprintToggles(BlueprintCategory category)
    {
        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField($"{category} Blueprints", EditorStylesUtility.Subheader);

        var enableAll = false;
        var disableAll = false;

        if (GUILayout.Button("EnableAll")) enableAll = true;

        if (GUILayout.Button("DisableAll"))
        {
            enableAll = false;
            disableAll = true;
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

        foreach (var b in Registry.blueprints.Where(b => b.category == category))
        {
            if (enableAll) Target.blueprints.Add(b);
            if (disableAll) Target.blueprints.Remove(b);

            var contains = Target.blueprints.Contains(b);
            
            var toggle = EditorGUILayout.Toggle(b.displayName, contains);

            if (contains == toggle) continue;

            if (toggle) Target.blueprints.Add(b);
            else Target.blueprints.Remove(b);

            MarkDirty();
        }

        if (enableAll || disableAll) MarkDirty();
    }
}
