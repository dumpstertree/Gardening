using UnityEngine;
using UnityEditor;

public static class CreateItem {
   
    [MenuItem("Assets/Create/Items/Item")]
	public static Eden.Templates.Item Create() {
		
		Eden.Templates.Item asset = ScriptableObject.CreateInstance<Eden.Templates.Item>();

        AssetDatabase.CreateAsset(asset, "Assets/Item.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}

public static class CreateShootableItem {
   
    [MenuItem("Assets/Create/Items/Shootable")]
	public static Eden.Templates.ShootableItem Create() {
		
		Eden.Templates.ShootableItem asset = ScriptableObject.CreateInstance<Eden.Templates.ShootableItem>();

        AssetDatabase.CreateAsset(asset, "Assets/New Shootable Item.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}

public static class CreateActionableItem {
   
    [MenuItem("Assets/Create/Items/Actionable")]
    public static Eden.Templates.ActionableItem Create() {
        
        Eden.Templates.ActionableItem asset = ScriptableObject.CreateInstance<Eden.Templates.ActionableItem>();

        AssetDatabase.CreateAsset(asset, "Assets/New Actionable Item.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}