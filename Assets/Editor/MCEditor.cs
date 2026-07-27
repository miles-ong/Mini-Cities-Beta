using UnityEngine;
using UnityEditor;

public abstract class MCEditor<T> : Editor where T : Object
{
    protected T Target;
    
    protected virtual void OnEnable() => Target = (T)target;

    protected void MarkDirty() => EditorUtility.SetDirty(Target);
}