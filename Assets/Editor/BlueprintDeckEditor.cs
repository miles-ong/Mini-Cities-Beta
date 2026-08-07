using System;
using System.Linq;
using MiniCities.Definitions.Blueprint;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlueprintDeckDefinition))]
public class BlueprintDeckEditor : MCEditor<BlueprintDeckDefinition>
{
    protected override void OnEnable()
    {
        base.OnEnable();

        SyncDeck();
    }
    
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Blueprint Deck", EditorStylesUtility.Header);

        EditorGUILayout.Space(EditorStylesUtility.LargeSpacing);

        var prev = Target.pool;
        Target.pool = (BlueprintPool)EditorGUILayout.ObjectField("Pool", Target.pool,  typeof(BlueprintPool), false);

        if (Target.pool == null) return;
        if (Target.pool != null && Target.pool != prev) SyncDeck();

        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        DrawStats();

        foreach (BlueprintCategory c in Enum.GetValues(typeof(BlueprintCategory)))
        {
            EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

            DrawBlueprintCounts(c);
        }
    }

    private void DrawStats()
    {
        EditorGUILayout.LabelField("Stats", EditorStylesUtility.Subheader);

        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

        var size = 0;
        var unique = 0;

        foreach (var bc in Target.blueprintCounts)
        {
            size += bc.count;

            if (bc.count > 0) unique++;
        }
        
        EditorGUILayout.LabelField($"Deck Size: {size}");
        EditorGUILayout.LabelField($"Unique Cards: {unique}");

        EditorGUILayout.Space(EditorStylesUtility.SmallSpacing);

        foreach (BlueprintCategory c in Enum.GetValues(typeof(BlueprintCategory)))
        {
            var cSize = 0;
            var cUnique = 0;
            
            foreach (var bc in Target.blueprintCounts.Where(b => b.blueprint.category == c))
            {
                cSize += bc.count;

                if (bc.count > 0) cUnique++;
            }
                
            EditorGUILayout.LabelField($"{c} Blueprints: {cSize} ({cUnique} Unique)");
        }
    }

    private void DrawBlueprintCounts(BlueprintCategory category)
    {
        EditorGUILayout.LabelField($"{category} Blueprints", EditorStylesUtility.Subheader);

        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

        foreach (var bc in Target.blueprintCounts.Where(b => b.blueprint.category == category))
        {
            var prev = bc.count;

            bc.count = EditorGUILayout.IntField(bc.blueprint.displayName, bc.count);

            if (bc.count != prev) MarkDirty();
        }
    }

    private void SyncDeck()
    {
        var poolBlueprints = Target.pool.blueprints.Items.ToHashSet();
        
        var deckBlueprints = Target.blueprintCounts
            .Select(bc => bc.blueprint)
            .ToHashSet();

        foreach (var b in poolBlueprints.Except(deckBlueprints))
            Target.blueprintCounts.Add(new BlueprintCount { blueprint = b });

        Target.blueprintCounts.RemoveAll(bc => !poolBlueprints.Contains(bc.blueprint));

        MarkDirty();
        Debug.Log("Synced");
    }
}
