using UnityEngine;

public class Inventory {

	// ***************** PUBLIC  *******************

	public Inventory ( int inventoryCount ) {
		
		_inventoryCount = inventoryCount;
		_inventoryItems = new InventoryItem[ _inventoryCount ];
	}
	public Inventory ( Serialized serializedData ) {

		Debug.Log( "Deserialize! " + serializedData.InventoryCount );
		
		_inventoryCount = serializedData.InventoryCount;
		_inventoryItems = new InventoryItem[ _inventoryCount ];

		for ( int i = 0; i < serializedData.InventoryCount; i++ ) {
			
			var item = serializedData.InventoryItems[ i ];
			
			if ( item.ID != "" ) {
				SetInventoryItem( i, InventoryItem.Deserialize( item ) );
			}
		}
	}

	// *******************************************

	public int InventoryCount {
		get { return _inventoryCount; }
	}

	public delegate void OnInventoryItemChangedEvent( int index, InventoryItem item );
	public OnInventoryItemChangedEvent OnInventoryItemChanged;

	public InventoryItem GetInventoryItem( int index ){

		if ( index < _inventoryItems.Length ) {
			return _inventoryItems[ index ];
		}

		return new InventoryItem();

	}
	public void SetInventoryItem( int index, InventoryItem item ){
		
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
	public bool AddInventoryItem ( InventoryItem item ) {

		// add to existing slot
		for( int i=0; i<_inventoryCount; i++ ){

			var slot = _inventoryItems[ i ];
			if ( slot != null && slot.ID == item.ID  ){

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

	public static void MoveItem( InventoryUI.DragObject fromObject, InventoryUI.DragObject toObject ){

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
	[SerializeField] protected InventoryItem[] _inventoryItems;

	// *******************************************

	private void DestroyItem ( int index, InventoryItem item ) {
		
		if ( item.Count <= 0 ){
			item = null;
		}
		
		FireOnInventoryItemChangedEvent( index, item );
	}

	private void FireOnInventoryItemChangedEvent ( int index, InventoryItem item ){

		if ( OnInventoryItemChanged != null ) {
			OnInventoryItemChanged( index, item );
		}
	}

	// *******************************************

	public Serialized Serialize () {
		
		Debug.Log( "Serialize!" );
		return new Serialized( this );
	}
	public class Serialized {

		[SerializeField] public int InventoryCount;
		[SerializeField] public InventoryItem[] InventoryItems;

		public Serialized ( Inventory inventory ) {

			InventoryCount = inventory._inventoryCount;
			InventoryItems = new InventoryItem[ InventoryCount ];

			for ( int i = 0; i < InventoryCount; i++ ) {
				
				var item = inventory._inventoryItems[ i ];
				if ( item != null ) {
					InventoryItems[ i ] = item;
				}
			}
		}
	}
}