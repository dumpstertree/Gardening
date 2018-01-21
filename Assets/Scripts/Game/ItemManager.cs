using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {


	// ************* PUBLIC **************

	
	public InventoryItem RequestItem( string itemName, string assignID = "" ) {
		
		if ( _items.ContainsKey( itemName ) ){
			return LoadWithName( itemName, assignID );
		} 
			
		return LoadInvalid( itemName, assignID );
	}
	public void Init () {
		
		var items = Resources.LoadAll<InventoryItem>( "" );
		
		_items = new Dictionary<string,InventoryItem>();
		foreach( InventoryItem item in items ) {
			
			if ( _items.ContainsKey( item.name ) ){
				Debug.LogWarning( "Conflicting names : " + item.name, item );
				continue;
			}

			_items.Add( item.name, item );
		}
	}


	// ************* PRIVATE **************


	private const string INVALID_ITEM_NAME = "InvalidItem";
	
	private Dictionary<string,InventoryItem> _items;


	private InventoryItem LoadWithName( string itemName, string assignID ){

		var inst = Instantiate( _items[ itemName ] );
		inst.ID = assignID != "" ? assignID : System.Guid.NewGuid().ToString();
		inst.name = itemName;
		return inst;
	}
	private InventoryItem LoadInvalid( string itemName, string assignID ){
		
		Debug.LogWarning( "Item with name " + itemName + " does not exist. Returning an invalid object!"  );

		var inst = Instantiate( _items[ INVALID_ITEM_NAME ] );
		inst.ID = assignID != "" ? assignID : System.Guid.NewGuid().ToString();
		inst.name = itemName;
		return inst;
	}
}
