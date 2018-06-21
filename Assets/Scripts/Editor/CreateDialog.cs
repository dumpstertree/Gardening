using UnityEngine;
using UnityEditor;

public class CreateDialog {
   
    [MenuItem("Assets/Create/Dialog/Dialog")]
	public static Eden.Model.Dialog.Dialog Create() {
		
		Eden.Model.Dialog.Dialog asset = ScriptableObject.CreateInstance<Eden.Model.Dialog.Dialog>();

        AssetDatabase.CreateAsset(asset, "Assets/New Dialog.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}