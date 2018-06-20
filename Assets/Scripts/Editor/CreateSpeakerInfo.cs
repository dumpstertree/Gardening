using UnityEngine;
using UnityEditor;

public class CreateSpeakerInfo {
   
    [MenuItem("Assets/Create/Dialog/Speaker")]
	public static Model.Dialog.SpeakerInfo Create() {
		
		Model.Dialog.SpeakerInfo asset = ScriptableObject.CreateInstance<Model.Dialog.SpeakerInfo>();

        AssetDatabase.CreateAsset(asset, "Assets/New Speaker.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}