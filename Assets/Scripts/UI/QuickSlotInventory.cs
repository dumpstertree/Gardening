using UnityEngine;

public class QuickSlotInventory : Inventory {

	public QuickSlotInventory ( int inventoryCount ) : base( inventoryCount ) {

		var handTemplate = Resources.Load( "hand" ) as Model.Template.InventoryItemTemplate;
		_inventoryItems[ ConvertQuickSlotIDToIndex( ID.Center ) ] = handTemplate.GetInstance( 1 );
	}
	public QuickSlotInventory ( Serialized serializedData ) : base( serializedData ) {

		var handTemplate = Resources.Load( "hand" ) as Model.Template.InventoryItemTemplate;
		_inventoryItems[ ConvertQuickSlotIDToIndex( ID.Center ) ] = handTemplate.GetInstance( 1 );
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
}