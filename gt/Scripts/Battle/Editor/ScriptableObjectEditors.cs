﻿using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}

public class ShellItemAsset
{
    [MenuItem("HelpTools/CreateScriptableObject/ShellItem")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<ShellItem>();
    }
}

public class VehicleSoundSetAsset
{
    [MenuItem("HelpTools/CreateScriptableObject/VehicleSoundSet")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<VehicleSoundSetItem>();
    }
}

public class BotSettingsAsset
{
    [MenuItem("HelpTools/CreateScriptableObject/BotSettingsAsset")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<BotSettings>();
    }
}