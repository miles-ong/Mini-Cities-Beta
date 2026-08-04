using System;
using System.Linq;
using MiniCities.Definitions.Ability;
using MiniCities.Definitions.Blueprint;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlueprintDefinition))]
public class BlueprintEditor : Editor
{
    private SerializedProperty _displayName;
    private SerializedProperty _file;
    private SerializedProperty _category;
    private SerializedProperty _population;
    private SerializedProperty _culture;
    private SerializedProperty _pollution;
    private SerializedProperty _building;
    private SerializedProperty _placementRules;
    private SerializedProperty _abilities;

    private void OnEnable()
    {
        _displayName = serializedObject.FindProperty("displayName");
        _file = serializedObject.FindProperty("file");
        _category = serializedObject.FindProperty("category");
        _population = serializedObject.FindProperty("population");
        _culture = serializedObject.FindProperty("culture");
        _pollution = serializedObject.FindProperty("pollution");
        _building = serializedObject.FindProperty("building");
        _placementRules = serializedObject.FindProperty("placementRules");
        _abilities = serializedObject.FindProperty("abilities");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Blueprint", EditorStylesUtility.Header);
        
        EditorGUILayout.Space(EditorStylesUtility.LargeSpacing);

        EditorGUILayout.PropertyField(_displayName);
        EditorGUILayout.PropertyField(_file);
        EditorGUILayout.PropertyField(_category);
        EditorGUILayout.PropertyField(_population);
        EditorGUILayout.PropertyField(_culture);
        EditorGUILayout.PropertyField(_pollution);

        var category = (BlueprintCategory)_category.enumValueIndex;

        if (category is not (BlueprintCategory.Initiative or BlueprintCategory.Disaster))
            EditorGUILayout.PropertyField(_building);
        
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        DrawPolymorphicList(_placementRules, typeof(IPlacementRule), "Placement Rules");

        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

        DrawAbilities();
        
        serializedObject.ApplyModifiedProperties();
        
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        EditorGUILayout.LabelField("Generated Description", EditorStylesUtility.Subheader);
        
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);

        var bp = (BlueprintDefinition)target;
        
        EditorGUILayout.HelpBox(bp.GenerateDescription(), MessageType.None);
    }

    private void DrawAbilities()
    {
        EditorGUILayout.LabelField("Abilities", EditorStylesUtility.Subheader);
        
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        EditorGUI.indentLevel++;

        var removeIndex = -1;

        for (var i = 0; i < _abilities.arraySize; i++)
        {
            var ability = _abilities.GetArrayElementAtIndex(i);
            var trigger = ability.FindPropertyRelative("trigger");
            var conditions = ability.FindPropertyRelative("conditions");
            var effects = ability.FindPropertyRelative("effects");

            EditorGUILayout.BeginVertical(GUI.skin.box);
            
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField($"Ability {i}", EditorStyles.boldLabel);

            if (GUILayout.Button("Remove", GUILayout.Width(70))) removeIndex = i;
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(trigger);
            
            EditorGUILayout.Space(EditorStylesUtility.SmallSpacing);
            
            DrawPolymorphicList(conditions, typeof(IAbilityCondition), "Conditions");
            
            EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
            
            DrawPolymorphicList(effects, typeof(IAbilityEffect), "Effects");

            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space(EditorStylesUtility.SmallSpacing);
        }
        
        EditorGUI.indentLevel--;
        
        if (GUILayout.Button("Add Ability")) _abilities.InsertArrayElementAtIndex(_abilities.arraySize);
        if (removeIndex >= 0) _abilities.DeleteArrayElementAtIndex(removeIndex);
    }

    private static void DrawPolymorphicList(SerializedProperty prop, Type interfaceType, string label)
    {
        EditorGUILayout.LabelField(label, EditorStylesUtility.Subheader);
        
        EditorGUILayout.Space(EditorStylesUtility.MediumSpacing);
        
        EditorGUI.indentLevel++;

        var removeIndex = -1;

        for (var i = 0; i < prop.arraySize; i++)
        {
            var element = prop.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(element, includeChildren: true);

            if (GUILayout.Button("-", GUILayout.Width(20))) removeIndex = i;
            
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex >= 0)
        {
            prop.GetArrayElementAtIndex(removeIndex).managedReferenceValue = null;
            prop.DeleteArrayElementAtIndex(removeIndex);
        }
        
        if (GUILayout.Button($"Add {label}", GUILayout.Width(120))) ShowAddMenu(prop, interfaceType);

        EditorGUI.indentLevel--;
    }

    private static void ShowAddMenu(SerializedProperty prop, Type interfaceType)
    {
        var sObj = prop.serializedObject;
        var menu = new GenericMenu();

        var types = TypeCache.GetTypesDerivedFrom(interfaceType)
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .OrderBy(t => t.Name)
            .ToList();

        if (types.Count == 0)
        {
            menu.AddDisabledItem(new GUIContent("No types found"));
        }
        else
        {
            foreach (var t in types)
            {
                var captured = t;
                
                menu.AddItem(new GUIContent(captured.Name), false, () =>
                {
                    var index = prop.arraySize;
                    prop.InsertArrayElementAtIndex(index);
                    var element = prop.GetArrayElementAtIndex(index);
                    element.managedReferenceValue = Activator.CreateInstance(captured);
                    sObj.ApplyModifiedProperties();
                });
            }
        }
        
        menu.ShowAsContext();
    }
}