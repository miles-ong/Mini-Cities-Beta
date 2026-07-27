using UnityEditor;
using UnityEngine;

public abstract class RegistryEditor<T, R> : MCEditor<T> where T : Object where R : Object
{
    protected R Registry;

    protected override void OnEnable()
    {
        base.OnEnable();
        FindRegistry();
    }
    
    private void FindRegistry()
    {
        var rName = typeof(R).Name;
        var guids = AssetDatabase.FindAssets($"t:{rName}");

        switch (guids.Length)
        {
            case 0: Debug.LogError($"No {rName} found"); return; 
            case 1: Debug.Log($"{rName} found"); break;
            case > 1: Debug.LogWarning($"Multiple {rName} found. Using the first one"); break;
        }
        
        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var registry = AssetDatabase.LoadAssetAtPath<R>(path);
        
        if (registry is null) Debug.LogError($"Failed to load {rName} at '{path}'");

        Registry = registry;
    }
}