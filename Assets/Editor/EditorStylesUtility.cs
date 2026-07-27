using UnityEditor;
using UnityEngine;

public static class EditorStylesUtility
{
    public const float LargeSpacing = 10f;
    public const float MediumSpacing = 5f;
    public const float SmallSpacing = 3f;
        
    public static readonly GUIStyle Header = new(EditorStyles.boldLabel) { fontSize = 24 };
    public static readonly GUIStyle Subheader = new(EditorStyles.boldLabel) { fontSize = 16 };
}