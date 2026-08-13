
using MiniCities.Definitions.Ability;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BuffBuildingStatInRange))]
public class BuffBuildingStatInRangeDrawer : PropertyDrawer
{
    private static readonly string[] AlwaysDrawn =
    {
        "stat", "operation", "amount", "scope", "level", "includeSelf"
    };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var y = position.y;

        void DrawField(string name)
        {
            var prop = property.FindPropertyRelative(name);
            var height = EditorGUI.GetPropertyHeight(prop);
            
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, height), prop, true);
            y += height + EditorGUIUtility.standardVerticalSpacing;
            
        }

        foreach (var name in AlwaysDrawn) DrawField(name);
        
        var buffByCategory = property.FindPropertyRelative("buffByCategory");
        var buffByBuilding = property.FindPropertyRelative("buffByBuilding");
        
        EditorGUI.BeginChangeCheck();
        
        DrawField("buffByCategory");

        if (EditorGUI.EndChangeCheck() && buffByCategory.boolValue) buffByBuilding.boolValue = false;
        
        EditorGUI.BeginChangeCheck();
        
        DrawField("buffByBuilding");
        
        if (EditorGUI.EndChangeCheck() && buffByBuilding.boolValue) buffByCategory.boolValue = false;
        
        if (buffByCategory.boolValue) DrawField("targetCategory");
        else if (buffByBuilding.boolValue) DrawField("targetBuilding");

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = 0f;

        float LineOf(string name) => 
            EditorGUI.GetPropertyHeight(property.FindPropertyRelative(name), true)
            + EditorGUIUtility.standardVerticalSpacing;

        foreach (var name in AlwaysDrawn) height += LineOf(name);

        height += LineOf("buffByCategory");
        height += LineOf("buffByBuilding");

        if (property.FindPropertyRelative("buffByCategory").boolValue) height += LineOf("targetCategory");
        else if (property.FindPropertyRelative("buffByBuilding").boolValue) height += LineOf("targetBuilding");
        
        return height - EditorGUIUtility.standardVerticalSpacing;
    }
}
