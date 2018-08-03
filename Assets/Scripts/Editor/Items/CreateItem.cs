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

public static class CreateFixedRangedWeapon {
   
    [MenuItem("Assets/Create/Items/Ranged/Fixed")]
	public static Eden.Templates.FixedRangedWeapon Create() {
		
		Eden.Templates.FixedRangedWeapon asset = ScriptableObject.CreateInstance<Eden.Templates.FixedRangedWeapon>();

        AssetDatabase.CreateAsset(asset, "Assets/New Shootable Item.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}

public static class CreateDynamicRangedWeapon{
   
    [MenuItem("Assets/Create/Items/Ranged/Dynamic")]
    public static Eden.Templates.DynamicRangedWeapon Create() {
        
        Eden.Templates.DynamicRangedWeapon asset = ScriptableObject.CreateInstance<Eden.Templates.DynamicRangedWeapon>();

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

public static class CreateBuildableGunItem {
   
    [MenuItem("Assets/Create/Items/Buildable/Gun")]
    public static Eden.Templates.GunBuildableItem Create() {
        
        Eden.Templates.GunBuildableItem asset = ScriptableObject.CreateInstance<Eden.Templates.GunBuildableItem>();

        AssetDatabase.CreateAsset(asset, "Assets/New Buildable Gun Item.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}
