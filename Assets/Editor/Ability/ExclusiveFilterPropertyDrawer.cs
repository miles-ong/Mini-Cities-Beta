using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class ExclusiveFilterPropertyDrawer : PropertyDrawer
{
    protected abstract string[] AlwaysDrawnFields { get; }
    protected abstract (string toggleField, string[] revealFields)[] ExclusiveOptions { get; }

    private float _y;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

        _y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            
            foreach (var name in AlwaysDrawnFields) DrawField(position, property, name);

            var toggles = ExclusiveOptions
                .Select(option => property.FindPropertyRelative(option.toggleField))
                .ToArray();

            for (var i = 0; i < toggles.Length; i++)
            {
                EditorGUI.BeginChangeCheck();
            
                DrawField(position, property, ExclusiveOptions[i].toggleField);

                if (!(EditorGUI.EndChangeCheck() && toggles[i].boolValue)) continue;
            
                for (var j = 0; j < toggles.Length; j++)
                    if (j != i) toggles[j].boolValue = false;
            }

            for (var i = 0; i < toggles.Length; i++)
            {
                if (!toggles[i].boolValue) continue;

                var fields = ExclusiveOptions[i].revealFields;

                if (fields == null) continue;

                foreach (var field in fields) DrawField(position, property, field);

                break;
            }
            
            EditorGUI.indentLevel--;
        }
        
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        if (property.isExpanded)
        {
            foreach (var name in AlwaysDrawnFields) height += LineOf(property, name);
            foreach (var option in ExclusiveOptions) height += LineOf(property, option.toggleField);

            foreach (var option in ExclusiveOptions)
            {
                if (!property.FindPropertyRelative(option.toggleField).boolValue) continue;
                if (option.revealFields == null) continue;

                foreach (var field in option.revealFields) height += LineOf(property, field);

                break;
            }
        }
        
        return height - EditorGUIUtility.standardVerticalSpacing;
    }
    
    private void DrawField(Rect position, SerializedProperty property, string name)
    {
        var prop = property.FindPropertyRelative(name);
        var height = EditorGUI.GetPropertyHeight(prop);

        EditorGUI.PropertyField(new Rect(position.x, _y, position.width, height), prop, true);
        
        _y += height + EditorGUIUtility.standardVerticalSpacing;
    }
    
    private float LineOf(SerializedProperty property, string name) =>
        EditorGUI.GetPropertyHeight(property.FindPropertyRelative(name), true)
        + EditorGUIUtility.standardVerticalSpacing;
}