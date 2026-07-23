using UnityEditor;
using UnityEngine;

public class EditorStylesUtility
{
    public static GUIStyle Header { get; private set; }
    public static GUIStyle SubHeader { get; private set; }
    public static GUIStyle TileLabel { get; private set; }

    static EditorStylesUtility()
    {
        Header = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 24
        };

        SubHeader = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 16
        };

        TileLabel = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true
        };
    }
}