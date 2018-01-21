using UnityEngine;
using UnityEditor;

public class CreatGunPartTemplate {
   
    [MenuItem("Assets/Create/Gun Part Template")]
    public static GunPartTemplate Create()
    {
        GunPartTemplate asset = ScriptableObject.CreateInstance<GunPartTemplate>();

        AssetDatabase.CreateAsset(asset, "Assets/GunPartTemplate.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}