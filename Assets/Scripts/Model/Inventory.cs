using UnityEngine;

public class Inventory {

	// ***************** PUBLIC  *******************

	public Inventory ( int inventoryCount ) {
		_inventoryCount = inventoryCount;
		_inventoryItems = new InventoryItem[ _inventoryCount ];
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
			if ( slot != null && slot.name == item.name  ){

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
		
		return new Serialized( this );
	}
	public static Inventory Deserialize ( Serialized serializedData ) {

		var inventory = new Inventory( serializedData.InventoryCount );

		for ( int i = 0; i < serializedData.InventoryCount; i++ ) {
				
			var item = serializedData.InventoryItems[ i ];
			if ( item.ID != "" ) {
				inventory.SetInventoryItem( i, InventoryItem.Deserialize( item ) );
			}
		}

		return inventory;
	}
	public class Serialized {

		[SerializeField] public int InventoryCount;
		[SerializeField] public InventoryItem.Serialized[] InventoryItems;

		public Serialized ( Inventory inventory ) {

			InventoryCount = inventory._inventoryCount;
			InventoryItems = new InventoryItem.Serialized[ InventoryCount ];

			for ( int i = 0; i < InventoryCount; i++ ) {
				
				var item = inventory._inventoryItems[ i ];
				if ( item != null ) {
					InventoryItems[ i ] = InventoryItem.Serialize( item );
				}
			}
		}
	}
}

public class QuickSlotInventory : Inventory {

	public QuickSlotInventory ( int inventoryCount ) : base( inventoryCount ) {

		_inventoryItems[ ConvertQuickSlotIDToIndex( ID.Center ) ] = Resources.Load( "hand" ) as InventoryItem;
	}

	// *******************************************

	public ID ConvertIndexToQuickSlotID ( int index ) { 

		switch( index ){
			case TOP_INDEX:
				return ID.Top;
			case RIGHT_INDEX:
				return ID.Right;
			case BOTTOM_INDEX:
				return ID.Bottom;
			case LEFT_INDEX:
				return ID.Left;
			case CENTER_INDEX:
				return ID.Center;
			default:
				return ID.Invalid;
		}
	}
	public int ConvertQuickSlotIDToIndex ( ID id ) {

		switch( id ){
			case ID.Top:
				return TOP_INDEX;
			case ID.Right:
				return RIGHT_INDEX;
			case ID.Bottom:
				return BOTTOM_INDEX;
			case ID.Left:
				return LEFT_INDEX;
			case ID.Center:
				return CENTER_INDEX;
			default:
				return -1;
		}
	}

	// *******************************************

	private const int TOP_INDEX    = 1;
	private const int RIGHT_INDEX  = 2;
	private const int BOTTOM_INDEX = 3;
	private const int LEFT_INDEX   = 4;
	private const int CENTER_INDEX = 0;

	// *******************************************

	public enum ID {
		Invalid,
		Top,
		Right,
		Bottom,
		Left,
		Center
	}

	public static QuickSlotInventory Deserialize ( Serialized serializedData ) {

		var inventory = new QuickSlotInventory( serializedData.InventoryCount );

		for ( int i = 0; i < serializedData.InventoryCount; i++ ) {
				
			var item = serializedData.InventoryItems[ i ];
			if ( item.ID != "" ) {
				inventory.SetInventoryItem( i, InventoryItem.Deserialize( item ) );
			}
		}

		return inventory;
	}
}