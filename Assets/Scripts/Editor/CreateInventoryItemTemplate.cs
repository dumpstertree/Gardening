using UnityEngine;
using UnityEditor;

public class CreateInventoryItemTemplate {
   
    [MenuItem("Assets/Create/Inventory Item Template")]
	public static Eden.Model.Template.InventoryItemTemplate Create()
    {
		Eden.Model.Template.InventoryItemTemplate asset = ScriptableObject.CreateInstance<Eden.Model.Template.InventoryItemTemplate>();

        AssetDatabase.CreateAsset(asset, "Assets/Inventory Item Template.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}