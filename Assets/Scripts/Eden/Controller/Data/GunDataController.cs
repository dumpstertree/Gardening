using System.Collections.Generic;
using UnityEngine;
using System.IO;
// using Model;

public class GunDataController : DataController {


	// *************** PUBLIC **********

	// private const string GUN_SAVE_DATA_PATH = "/Users/zacharycollins/desktop/";

	
	// public void SaveGun ( string id, Gun gun ) {

 //       	var json = JsonUtility.ToJson( gun, true );
 //        File.WriteAllText( Application.persistentDataPath + id, json );
 //        Debug.Log( Application.persistentDataPath);
	// }
	// public Gun LoadGun ( string id ) {

	// 	return CreateBlankGun( id );
	// 		Debug.Log( Application.persistentDataPath);
	// 	var text = LoadFileFromPath(  Application.persistentDataPath + id );
	// 	return (text != "") ? CreatGunFromJson( text ) : CreateBlankGun( id );
	// }

	// // ************** PRIVATE **********
	
	// private Gun CreatGunFromJson ( string json ) {
		
	// 	return JsonUtility.FromJson<Gun>( json );
	// }
	// private Gun CreateBlankGun ( string id ) {
	
	// 	var newGun = new Gun( new List<Gun.Part>() );
	// 	SaveGun( id, newGun );

	// 	Debug.LogWarningFormat( "No Gun data found. Making a new one" );
	
	// 	return newGun;
	// }
}
