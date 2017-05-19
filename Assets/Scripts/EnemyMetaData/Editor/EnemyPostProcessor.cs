using UnityEngine;
using UnityEditor;
using System.IO;

public class EnemyPostProcessor : AssetPostprocessor
{
    public static string watchPath = "/Resources/Enemys/";

    public static string fileName = "EnemyMetaData.asset";
    public static string filePath = "Assets/GameData/" + fileName;
    

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            if (str.Contains(watchPath) && !str.Contains(fileName)) { OnImport(str); }

        }

        for (int i = 0; i < movedAssets.Length; i++)
        {
            if (movedAssets[i].Contains(watchPath) && !movedAssets[i].Contains(fileName))
            {
                OnImport(movedAssets[i]);
            }

        }

        foreach (string str in deletedAssets)
        {
            if (str.Contains(watchPath) && !str.Contains(fileName)) { OnDeleted(str); }

        }

        for (int i = 0; i < movedFromAssetPaths.Length; i++)
        {
            if (movedFromAssetPaths[i].Contains(watchPath) && !movedFromAssetPaths[i].Contains(fileName))
            {
                OnDeleted(movedFromAssetPaths[i]);
            }

        }

    }
    static void OnImport(string Path)
    {
        EnemyMetaDataSO enemyMetaData = AssetDatabase.LoadAssetAtPath<EnemyMetaDataSO>(filePath);
        if (enemyMetaData == null) {
            enemyMetaData = ScriptableObject.CreateInstance<EnemyMetaDataSO>();
            enemyMetaData.name = "EnemyMetaData";
            AssetDatabase.CreateAsset(enemyMetaData, filePath );
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        if (enemyMetaData.enemyMetaData == null)
        {
            enemyMetaData.enemyMetaData = new System.Collections.Generic.List<EnemyMetaData>();
            enemyMetaData.keys = new System.Collections.Generic.List<string>();
        }

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(Path);
        if (prefab == null) { return; }
        if (!enemyMetaData.keys.Contains(prefab.name))
        {
            EnemyMetaData data = new EnemyMetaData();
            data.name = prefab.name;
            data.firstLvl = 1;
            data.cost = 1;
            data.prefab = prefab;

            enemyMetaData.enemyMetaData.Add(data);
            enemyMetaData.keys.Add(prefab.name);
            //AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(enemyMetaData);
        }

    }
    static void OnDeleted(string Path)
    {
        EnemyMetaDataSO enemyMetaData = AssetDatabase.LoadAssetAtPath<EnemyMetaDataSO>(filePath);
        string name = System.IO.Path.GetFileNameWithoutExtension(Path);
        if (enemyMetaData.keys.Contains(name))
        {
            int index = enemyMetaData.keys.IndexOf(name);

            enemyMetaData.keys.RemoveAt(index);
            enemyMetaData.enemyMetaData.RemoveAt(index);
            //AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(enemyMetaData);
        }

    }
}