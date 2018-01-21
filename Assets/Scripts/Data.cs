using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Model;

public static class DataController {	
	
	// *********** INVENTORY *************

	private const string INVENTORY_SAVE_PATH = "/Users/zacharycollins/desktop/";
	private const string INVENTORY_SAVE_FILE_NAME = "Inventory";
	private const int NUMBER_OF_INVENTORY_SLOTS = 15;


	public static void SaveInventory ( Inventory inventory ) {
      
       	var json = JsonUtility.ToJson( inventory.Serialize(), true );
        File.WriteAllText( INVENTORY_SAVE_PATH + INVENTORY_SAVE_FILE_NAME, json );
	}
	public static Inventory LoadInventory () {

		var text = LoadFileFromPath( INVENTORY_SAVE_PATH + INVENTORY_SAVE_FILE_NAME );
		return (text != "") ? CreateInventoryFromJson( text ) : CreateBlankInventory();
	}
	private static Inventory CreateInventoryFromJson ( string json ) {

		var model = JsonUtility.FromJson<Inventory.Serialized>( json );
		return Inventory.Deserialize( model );
	}
	private static Inventory CreateBlankInventory () {

		var newInventory = new Inventory( NUMBER_OF_INVENTORY_SLOTS );
		SaveInventory( newInventory );

		Debug.LogWarningFormat( "No Inventory data found. Making a new one" );

		return newInventory;
	}

	
	// ************ QUICKSLOT ************

	private const string QUICKSLOT_PATH = "/Users/zacharycollins/desktop/";
	private const string QUICKSLOT_FILE_NAME = "QuickSlot";
	private const int NUMBER_OF_QUICKSLOT_SLOTS = 5;

	
	public static void SaveQuickSlotInventory ( QuickSlotInventory quickslot ) {

       	var json = JsonUtility.ToJson( quickslot.Serialize(), true );
        File.WriteAllText( QUICKSLOT_PATH + QUICKSLOT_FILE_NAME, json );
	}
	public static QuickSlotInventory LoadQuickSlotInventory () {

		var text = LoadFileFromPath( QUICKSLOT_PATH + QUICKSLOT_FILE_NAME );
		return (text != "") ? CreateQuickSlotFromJson( text ) : CreateBlankQuickSlot();
	}
	private static QuickSlotInventory CreateQuickSlotFromJson ( string json ) {
	
		var model = JsonUtility.FromJson<QuickSlotInventory.Serialized>( json );
		return QuickSlotInventory.Deserialize( model );
	}
	private static QuickSlotInventory CreateBlankQuickSlot () {
	
		var newQuickSlot = new QuickSlotInventory( NUMBER_OF_QUICKSLOT_SLOTS );
		SaveQuickSlotInventory( newQuickSlot );

		Debug.LogWarningFormat( "No Quickslot data found. Making a new one" );
		
		return newQuickSlot;
	}

	
	// ************** GUN **********

	private const string GUN_SAVE_DATA_PATH = "/Users/zacharycollins/desktop/";

	
	public static void SaveGun ( string id, Gun gun ) {

       	var json = JsonUtility.ToJson( gun, true );
        File.WriteAllText( GUN_SAVE_DATA_PATH + id, json );
	}
	public static Gun LoadGun ( string id ) {

		var text = LoadFileFromPath( GUN_SAVE_DATA_PATH + id );
		return (text != "") ? CreatGunFromJson( text ) : CreateBlankGun( id );
	}
	private static Gun CreatGunFromJson ( string json ) {
		
		return JsonUtility.FromJson<Gun>( json );
	}
	private static Gun CreateBlankGun ( string id ) {
	
		var newGun = new Gun( new List<Gun.Part>() );
		SaveGun( id, newGun );
		Debug.Log( id );
		Debug.LogWarningFormat( "No Gun data found. Making a new one" );
	
		return newGun;
	}

	
	// ************************

	
	private static string LoadFileFromPath ( string path ) {

		if ( File.Exists( path ) ) {
			return File.ReadAllText( path );
		}

		return "";
	}
 }
