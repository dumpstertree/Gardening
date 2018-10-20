using UnityEngine;
using UnityEngine.EventSystems;
using Eden.Controller;
using Eden.Model;
using Eden.UI.Elements;

namespace Eden.UI.Elements {
	
	public abstract class InventoryUI1 : InteractivePanel {

	// 	private static DragObject HOVER_OBJECT;
	// 	private static DragObject DRAG_OBJECT;

	// 	[SerializeField] protected ItemSlot _itemBubblePrefab;
	// 	[SerializeField] protected Item _itemPrefab;

	// 	private const int COLLUMNS = 3;
	// 	private const float PADDING = 25;

	// 	private Inventory _inventory;
	// 	private ItemSlot[] _itemBubbles;

	// 	private static ItemSlot HoveringSlot;
	// 	private static ItemSlot DraggingSlot;
	// 	private static Item ItemBeingDragged;

	// 	public static void SetDragSlot ( ItemSlot slot  ) {

	// 		// Start dragging new object
	// 		if ( DraggingSlot == null ) {

	// 			if ( slot.Item != null ) {

	// 				ItemBeingDragged = slot.ItemInSlot;
					
	// 				HoveringSlot = slot; 
	// 				HoveringSlot.SetItem( null );
	// 				return;
	// 			}

	// 		// Perform a swap
	// 		} else {

	// 			// Inventory.MoveItem( DraggingSlot., HOVER_OBJECT );
	// 			ItemBeingDragged = null;
	// 			DraggingSlot = null;
	// 		}
	// 	}
	// 	public static void SetHoverSlot ( ItemSlot slot ) {

	// 		HoveringSlot = slot;
	// 	}
		
	// 	// *******************************

	// 	protected override void OnPresent () {
			
	// 		base.OnPresent();
			
	// 		Clear();
	// 		Load();

	// 		EventSystem.current.SetSelectedGameObject( _itemBubbles[ 0 ].gameObject );
	// 	}

		
	// 	// *******************************

	// 	private void Clear () {
			
	// 		if ( _itemBubbles != null ) {
				
	// 			foreach ( ItemSlot item in _itemBubbles ) {
					
	// 				item.SetItem( null );
	// 			}
	// 		}

	// 		_itemBubbles = null;
	// 		_inventory = null;
	// 	}
	// 	private void Load () {

	// 		_inventory = GetInventory();
	// 		_itemBubbles = GetItemBubbles();
			
	// 		foreach( ItemSlot slot in _itemBubbles ){

	// 			var index = slot.Index;
	// 			var itemBackingValue = _inventory.GetInventoryItem( index );

	// 			if ( itemBackingValue != null ) {
					
	// 				var item = CreateItem( itemBackingValue );
	// 				slot.ItemInSlot = item;
	// 			}
	// 		}
	// 	}

	// 	private Item CreateItem ( Eden.Model.Item backingItem ) {

	// 		var item = Instantiate( _itemPrefab );
	// 		item.SetBackingItem( backingItem );

	// 		return item;
	// 	}
	
	// 	// *******************************

	// 	protected abstract Inventory GetInventory ();
	// 	protected abstract ItemSlot[] GetItemBubbles ();

		
	// 	// *******************************


	// 	public class DragObject {

	// 		public Inventory Inventory{ get; }
	// 		public int Index{ get; }

	// 		public DragObject( Inventory inventory, int index ){
	// 			Inventory = inventory;
	// 			Index = index;
	// 		}
	// 	}
	}
}