using System.IO;
using UnityEngine;

public class ScriptableObjectAssetCreator
{
    public static T Create<T>(string scriptableObjName, string mapFolder = "", string desFolder = "") where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();
#if UNITY_EDITOR
        if (!Directory.Exists(Path.Combine(desFolder, mapFolder)))
            Directory.CreateDirectory(Path.Combine(desFolder, mapFolder));
        UnityEditor.AssetDatabase.CreateAsset(asset, Path.Combine(desFolder, mapFolder, scriptableObjName + ".asset"));
        UnityEditor.AssetDatabase.SaveAssets();
#endif
        return asset;
    }
    public static T Get<T>(string scriptableObjName, string mapFolder = "", string desFolder = "") where T : ScriptableObject
    {
#if UNITY_EDITOR
        if (Directory.Exists(Path.Combine(desFolder, mapFolder, scriptableObjName + ".asset")))
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(Path.Combine(desFolder, mapFolder, scriptableObjName + ".asset"));
        }
#endif
        return default(T);
    }
}