using UnityEngine;
using UnityEditor;

public class CreateSpeakerInfo {
   
    [MenuItem("Assets/Create/Dialog/Speaker")]
	public static Eden.Model.Dialog.SpeakerInfo Create() {
		
		Eden.Model.Dialog.SpeakerInfo asset = ScriptableObject.CreateInstance<Eden.Model.Dialog.SpeakerInfo>();

        AssetDatabase.CreateAsset(asset, "Assets/New Speaker.asset");
        AssetDatabase.SaveAssets();
        
        return asset;
    }
}