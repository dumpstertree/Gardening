using UnityEngine;
using UnityEditor;

public class CreateDialog {
   
    [MenuItem("Assets/Create/Dialog/Dialog")]
	public static Model.Dialog.Dialog Create() {
		
		Model.Dialog.Dialog asset = ScriptableObject.CreateInstance<Model.Dialog.Dialog>();

        AssetDatabase.CreateAsset(asset, "Assets/New Dialog.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}