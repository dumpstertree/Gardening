using UnityEngine;
using UnityEditor;

public class CreateInventoryItemTemplate {
   
    [MenuItem("Assets/Create/Inventory Item Template")]
    public static InventoryItemTemplate Create()
    {
        InventoryItemTemplate asset = ScriptableObject.CreateInstance<InventoryItemTemplate>();

        AssetDatabase.CreateAsset(asset, "Assets/Inventory Item Template.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}