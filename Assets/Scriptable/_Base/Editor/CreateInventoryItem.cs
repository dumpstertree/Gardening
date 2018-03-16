using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateInventoryItem {
   
    [MenuItem("Assets/Create/Inventory Item")]
    public static InventoryItem Create()
    {
        InventoryItem asset = ScriptableObject.CreateInstance<InventoryItem>();

        AssetDatabase.CreateAsset(asset, "Assets/InventoryItem.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}