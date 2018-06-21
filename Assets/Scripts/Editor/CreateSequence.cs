using UnityEngine;
using UnityEditor;

public class CreateSequence {
   
    [MenuItem("Assets/Create/Dialog/Sequence")]
	public static Eden.Model.Dialog.Sequence Create() {
		
		Eden.Model.Dialog.Sequence asset = ScriptableObject.CreateInstance<Eden.Model.Dialog.Sequence>();

        AssetDatabase.CreateAsset(asset, "Assets/New Sequence.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}