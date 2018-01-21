using UnityEngine;
using System.IO;
using Model;

public class PlayerDataController : DataController {

	// *********** INVENTORY *************

	private const string INVENTORY_SAVE_PATH = "/Users/zacharycollins/desktop/";
	private const string INVENTORY_SAVE_FILE_NAME = "Inventory";
	private const int NUMBER_OF_INVENTORY_SLOTS = 15;


	public void SaveInventory ( Inventory inventory ) {
      
       	var json = JsonUtility.ToJson( inventory.Serialize(), true );
        File.WriteAllText( INVENTORY_SAVE_PATH + INVENTORY_SAVE_FILE_NAME, json );
	}
	public Inventory LoadInventory () {

		var text = LoadFileFromPath( INVENTORY_SAVE_PATH + INVENTORY_SAVE_FILE_NAME );
		return (text != "") ? CreateInventoryFromJson( text ) : CreateBlankInventory();
	}
	private Inventory CreateInventoryFromJson ( string json ) {

		var model = JsonUtility.FromJson<Inventory.Serialized>( json );
		return Inventory.Deserialize( model );
	}
	private Inventory CreateBlankInventory () {

		var newInventory = new Inventory( NUMBER_OF_INVENTORY_SLOTS );
		SaveInventory( newInventory );

		Debug.LogWarningFormat( "No Inventory data found. Making a new one" );

		return newInventory;
	}

	
	// ************ QUICKSLOT ************

	private const string QUICKSLOT_PATH = "/Users/zacharycollins/desktop/";
	private const string QUICKSLOT_FILE_NAME = "QuickSlot";
	private const int NUMBER_OF_QUICKSLOT_SLOTS = 5;

	
	public void SaveQuickSlotInventory ( QuickSlotInventory quickslot ) {

       	var json = JsonUtility.ToJson( quickslot.Serialize(), true );
        File.WriteAllText( QUICKSLOT_PATH + QUICKSLOT_FILE_NAME, json );
	}
	public QuickSlotInventory LoadQuickSlotInventory () {

		var text = LoadFileFromPath( QUICKSLOT_PATH + QUICKSLOT_FILE_NAME );
		return (text != "") ? CreateQuickSlotFromJson( text ) : CreateBlankQuickSlot();
	}
	private QuickSlotInventory CreateQuickSlotFromJson ( string json ) {
	
		var model = JsonUtility.FromJson<QuickSlotInventory.Serialized>( json );
		return QuickSlotInventory.Deserialize( model );
	}
	private QuickSlotInventory CreateBlankQuickSlot () {
	
		var newQuickSlot = new QuickSlotInventory( NUMBER_OF_QUICKSLOT_SLOTS );
		SaveQuickSlotInventory( newQuickSlot );

		Debug.LogWarningFormat( "No Quickslot data found. Making a new one" );
		
		return newQuickSlot;
	}

	// ************ PARTS ************

	private const string PARTS_PATH = "/Users/zacharycollins/desktop/";
	private const string PARTS_FILE_NAME = "PartInventory";
	private const int NUMBER_OF_PARTS_SLOTS = 99;

	
	public void SavePartInventory ( PartInventory partInventory ) {

       	var json = JsonUtility.ToJson( partInventory, true );
        File.WriteAllText( PARTS_PATH + PARTS_FILE_NAME, json );
	}
	public PartInventory LoadPartInventory () {

		var text = LoadFileFromPath( PARTS_PATH + PARTS_FILE_NAME );
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
