using UnityEngine;
using Eden.UI;
using Eden.Model;

public class Inventory {

	// ***************** PUBLIC  *******************

	public Inventory ( int inventoryCount ) {
		
		_inventoryCount = inventoryCount;
		_inventoryItems = new Item[ _inventoryCount ];
	}
	// public Inventory ( Serialized serializedData ) {
		
	// 	_inventoryCount = serializedData.InventoryCount;
	// 	_inventoryItems = new Item[ _inventoryCount ];

	// 	for ( int i = 0; i < serializedData.InventoryCount; i++ ) {
			
	// 		var item = serializedData.InventoryItems[ i ];
			
	// 		if ( item.ID != "" ) {
	// 			SetInventoryItem( i, InventoryItem.Deserialize( item ) );
	// 		}
	// 	}
	// }

	// *******************************************

	public int InventoryCount {
		get { return _inventoryCount; }
	}

	public delegate void OnInventoryItemChangedEvent( int index, Item item );
	public OnInventoryItemChangedEvent OnInventoryItemChanged;

	public Item GetInventoryItem( int index ){

		if ( index < _inventoryItems.Length ) {
			return _inventoryItems[ index ];
		}

		return new Item();

	}
	public void SetInventoryItem( int index, Item item ){
		
		if ( item != null ) {
			item.OnCountChanged -= () => DestroyItem( index, item );
		}
		
		if ( index < _inventoryItems.Length ) {

			_inventoryItems[ index ] = item;

			if ( item != null ){
				item.OnCountChanged += () => DestroyItem( index, item);
			}

			FireOnInventoryItemChangedEvent( index, item );
		}
	}
	public bool AddInventoryItem ( Item item ) {

		// add to existing slot
		for( int i=0; i<_inventoryCount; i++ ){

			var slot = _inventoryItems[ i ];
			if ( slot != null && slot.PrefabID == item.PrefabID  ){

				// take as many as possible
				if ( slot.Count + item.Count > slot.MaxCount ){

					var canTake = slot.MaxCount - slot.Count;

					slot.AddCount( canTake );
					item.ReduceCount( canTake ); 
				}

				// take all
				else{

					slot.AddCount( item.Count );
					return true;
				}
			}
		}

		// set to new slot
		for( int i=0; i<_inventoryCount; i++ ){

			var slot = _inventoryItems[ i ];
			if ( slot == null ){
				SetInventoryItem( i, item );
				return true;
			}
		}

		return false;
	}

	// *******************************************

	public static void MoveItem( InventoryUI1.DragObject fromObject, InventoryUI1.DragObject toObject ){

		var fromItem = fromObject.Inventory.GetInventoryItem( fromObject.Index );
		var toItem = toObject.Inventory.GetInventoryItem( toObject.Index );

		if ( fromItem != null ) {
			fromItem.OnCountChanged = null;
		}
		if ( toItem != null ) {
			toItem.OnCountChanged = null;
		}

		fromObject.Inventory.SetInventoryItem( fromObject.Index, toItem );
		toObject.Inventory.SetInventoryItem( toObject.Index, fromItem );
	}

	// ***************** PRIVATE *******************

	[SerializeField] private int _inventoryCount;
	[SerializeField] protected Item[] _inventoryItems;

	// *******************************************

	private void DestroyItem ( int index, Item item ) {
		
		if ( item.Count <= 0 ){
			item = null;
		}
		
		FireOnInventoryItemChangedEvent( index, item );
	}

	private void FireOnInventoryItemChangedEvent ( int index, Item item ){

		if ( OnInventoryItemChanged != null ) {
			OnInventoryItemChanged( index, item );
		}
	}

	// *******************************************

	public Serialized Serialize () {
		
		return new Serialized( this );
	}
	public class Serialized {

		[SerializeField] public int InventoryCount;
		[SerializeField] public Item[] InventoryItems;

		public Serialized ( Inventory inventory ) {

			InventoryCount = inventory._inventoryCount;
			InventoryItems = new Item[ InventoryCount ];

			for ( int i = 0; i < InventoryCount; i++ ) {
				
				var item = inventory._inventoryItems[ i ];
				if ( item != null ) {
					InventoryItems[ i ] = item;
				}
			}
		}
	}
}