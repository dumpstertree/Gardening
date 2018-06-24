using UnityEngine;
using System.IO;
using Model;

public class PlayerDataController : DataController {

	// *********** INVENTORY *************

	private const string INVENTORY_SAVE_PATH = "/Users/zacharycollins/desktop/";
	private const string INVENTORY_SAVE_FILE_NAME = "Inventory";
	private const int NUMBER_OF_INVENTORY_SLOTS = 15;


	public void SaveInventory ( Inventory inventory ) {

		Debug.Log( "Save!" );
      
       	var json = JsonUtility.ToJson( inventory.Serialize(), true );
        File.WriteAllText( Application.persistentDataPath + INVENTORY_SAVE_FILE_NAME, json );
	}
	public Inventory LoadInventory () {

		var text = LoadFileFromPath( Application.persistentDataPath + INVENTORY_SAVE_FILE_NAME );
		return (text != "") ? CreateInventoryFromJson( text ) : CreateBlankInventory();
	}
	private Inventory CreateInventoryFromJson ( string json ) {
		
		var inventory = JsonUtility.FromJson<Inventory.Serialized>( json );
		return new Inventory( inventory );
	}
	private Inventory CreateBlankInventory () {

		var newInventory = new Inventory( NUMBER_OF_INVENTORY_SLOTS );
		SaveInventory( newInventory );

		Debug.LogWarningFormat( "No Inventory data found. Making a new one" );

		return newInventory;
	}


	// ************ PARTS ************

	private const string PARTS_PATH = "/Users/zacharycollins/desktop/";
	private const string PARTS_FILE_NAME = "PartInventory";
	private const int NUMBER_OF_PARTS_SLOTS = 99;

	
	public void SavePartInventory ( PartInventory partInventory ) {

       	var json = JsonUtility.ToJson( partInventory, true );
        File.WriteAllText( Application.persistentDataPath + PARTS_FILE_NAME, json );
	}
	public PartInventory LoadPartInventory () {

		var text = LoadFileFromPath( Application.persistentDataPath + PARTS_FILE_NAME );
		return (text != "") ? CreatePartInventoryFromJson( text ) : CreateBlankPartInventory();
	}
	private PartInventory CreatePartInventoryFromJson ( string json ) {
		
		var model = JsonUtility.FromJson<PartInventory>( json );
		return model;
	}
	private PartInventory CreateBlankPartInventory () {
	
		var newPartInventory = new PartInventory( NUMBER_OF_PARTS_SLOTS );
		SavePartInventory( newPartInventory );

		Debug.LogWarningFormat( "No PartInventory data found. Making a new one" );
		
		return newPartInventory;
	}

}
