using UnityEngine;
using UnityEditor;

public static class CreateAnimation {
   
    [MenuItem("Assets/Create/Animation/Animation")]
    public static Dumpster.Animation.Templates.Animation Create() {
        
        Dumpster.Animation.Templates.Animation asset = ScriptableObject.CreateInstance<Dumpster.Animation.Templates.Animation>();

        AssetDatabase.CreateAsset(asset, "Assets/New Animation.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}
