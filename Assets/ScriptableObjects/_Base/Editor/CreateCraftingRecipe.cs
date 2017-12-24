using UnityEngine;
using UnityEditor;

public class CreateCraftingRecipe {
   
    [MenuItem("Assets/Create/Crafting Recipe")]
    public static Crafting.Recipe Create()
    {
        Crafting.Recipe asset = ScriptableObject.CreateInstance<Crafting.Recipe>();

        AssetDatabase.CreateAsset(asset, "Assets/Recipe.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}