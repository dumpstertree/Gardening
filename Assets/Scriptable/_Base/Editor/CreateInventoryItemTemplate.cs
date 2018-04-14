using UnityEngine;
using UnityEditor;

public class CreateInventoryItemTemplate {
   
    [MenuItem("Assets/Create/Inventory Item Template")]
	public static Model.Template.InventoryItemTemplate Create()
    {
		Model.Template.InventoryItemTemplate asset = ScriptableObject.CreateInstance<Model.Template.InventoryItemTemplate>();

        AssetDatabase.CreateAsset(asset, "Assets/Inventory Item Template.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}