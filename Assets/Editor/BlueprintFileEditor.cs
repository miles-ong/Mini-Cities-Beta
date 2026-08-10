using System;
using System.Linq;
using MiniCities.Definitions.Blueprint;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlueprintFileDefinition))]
public class BlueprintFileEditor : MCEditor<BlueprintFileDefinition>
{
    protected override void OnEnable()
    {
        base.OnEnable();

        if (Target.poolDefinition != null) SyncFile();
    }
    
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Blueprint File", EditorStylesUtility.Header);

        EditorGUILayout.Space(EditorStylesUtility.LargeSpacing);

        var previousDisplayName = Target.displayName;
        Target.displayName = EditorGUILayout.TextField("Name", Target.displayName);

        if (!Target.displayName.Equals(previousDisplayName)) MarkDirty();
        
        var previousBlueprintPool = Target.poolDefinition;
        Target.poolDefinition = (BlueprintPoolDefinition)EditorGUILayout.ObjectField("Pool", Target.poolDefinition,  typeof(BlueprintPoolDefinition), false);

        if (Target.poolDefinition == null) return;
        if (Target.poolDefinition != null && Target.poolDefinition != previousBlueprintPool) SyncFile();

        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        DrawStats();

        foreach (BlueprintCategory category in Enum.GetValues(typeof(BlueprintCategory)))
        {
            EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

            DrawBlueprintCounts(category);
        }
    }

    private void DrawStats()
    {
        EditorGUILayout.LabelField("Stats", EditorStylesUtility.Subheader);

        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

        var fileSize = 0;
        var uniqueBlueprints = 0;

        foreach (var blueprintCount in Target.blueprintCounts)
        {
            fileSize += blueprintCount.count;

            if (blueprintCount.count > 0) uniqueBlueprints++;
        }
        
        EditorGUILayout.LabelField($"File Size: {fileSize}");
        EditorGUILayout.LabelField($"Unique Blueprints: {uniqueBlueprints}");

        EditorGUILayout.Space(EditorStylesUtility.SmallSpacing);

        foreach (BlueprintCategory category in Enum.GetValues(typeof(BlueprintCategory)))
        {
            var categorySize = 0;
            var uniqueCategoryBlueprints = 0;
            
            foreach (var blueprintCount in Target.blueprintCounts.Where(bc => bc.blueprint.category == category))
            {
                categorySize += blueprintCount.count;

                if (blueprintCount.count > 0) uniqueCategoryBlueprints++;
            }
                
            EditorGUILayout.LabelField($"{category} Blueprints: {categorySize} ({uniqueCategoryBlueprints} Unique)");
        }
    }

    private void DrawBlueprintCounts(BlueprintCategory category)
    {
        EditorGUILayout.LabelField($"{category} Blueprints", EditorStylesUtility.Subheader);

        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

        foreach (var blueprintCount in Target.blueprintCounts.Where(b => b.blueprint.category == category))
        {
            var previousCount = blueprintCount.count;

            blueprintCount.count = EditorGUILayout.IntField(blueprintCount.blueprint.displayName, blueprintCount.count);

            if (blueprintCount.count != previousCount) MarkDirty();
        }
    }

    private void SyncFile()
    {
        var poolBlueprints = Target.poolDefinition.blueprints.Items.ToHashSet();
        
        var fileBlueprints = Target.blueprintCounts
            .Select(bc => bc.blueprint)
            .ToHashSet();

        foreach (var blueprintDefinition in poolBlueprints.Except(fileBlueprints))
            Target.blueprintCounts.Add(new BlueprintCount { blueprint = blueprintDefinition });

        Target.blueprintCounts.RemoveAll(bc => !poolBlueprints.Contains(bc.blueprint));

        MarkDirty();
        Debug.Log("Synced");
    }
}
