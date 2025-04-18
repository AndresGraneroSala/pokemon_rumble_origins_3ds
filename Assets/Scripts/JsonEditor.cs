using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Reflection;

[ExecuteInEditMode]
public class JsonEditor : MonoBehaviour
{
    #if UNITY_EDITOR
    public UnityEngine.Object targetScript;
    [TextArea(10, 20)]
    public string jsonText = "";

    [ContextMenu("Generate Json")]
    public void GenerateJson()
    {
        if (targetScript != null)
        {
            jsonText = JsonUtility.ToJson(targetScript, true);
        }
    }
    [ContextMenu("Apply Json")]
    public void ApplyJson()
    {
        if (targetScript != null && !string.IsNullOrEmpty(jsonText))
        {
            JsonUtility.FromJsonOverwrite(jsonText, targetScript);
            EditorUtility.SetDirty(targetScript); // Marca el objeto como modificado
        }
    }
    #endif
}