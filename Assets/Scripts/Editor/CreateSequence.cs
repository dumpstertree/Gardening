using UnityEngine;
using UnityEditor;

public class CreateSequence {
   
    [MenuItem("Assets/Create/Dialog/Sequence")]
	public static Model.Dialog.Sequence Create() {
		
		Model.Dialog.Sequence asset = ScriptableObject.CreateInstance<Model.Dialog.Sequence>();

        AssetDatabase.CreateAsset(asset, "Assets/New Sequence.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}