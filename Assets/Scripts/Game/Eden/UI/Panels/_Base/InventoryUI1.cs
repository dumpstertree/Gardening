using UnityEngine;

namespace Eden.UI {
	
	public abstract class InventoryUI1 : InteractivePanel {

		private static DragObject HOVER_OBJECT;
		private static DragObject DRAG_OBJECT;

		[SerializeField] protected ItemBubbleUI _itemBubblePrefab;

		private const int COLLUMNS = 3;
		private const float PADDING = 25;

		private Inventory _inventory;
		private ItemBubbleUI[] _itemBubbles;

		
		// *******************************

		protected override void OnPresent(){
			
			base.OnPresent();
			
			Clear();
			Load();
		}

		
		// *******************************

		private void Clear () {
			
			if ( _itemBubbles != null ) {
				
				foreach ( ItemBubbleUI item in _itemBubbles ) {
					
					if ( !item.Indestuctable ){
						Destroy( item.gameObject );
					}
				}
			}

			_itemBubbles = null;
			_inventory = null;
		}
		private void Load () {

			_inventory = GetInventory();
			_itemBubbles = GetItemBubbles();

			_inventory.OnInventoryItemChanged += ( index, item ) => { 
				SetItemBubble( index, item );
			};
		
			foreach( ItemBubbleUI itemBubble in _itemBubbles ){

				var index = itemBubble.Index;
				SetItemBubble( index, _inventory.GetInventoryItem( index ) );

				itemBubble.PointerEnter += () => {
					itemBubble.SetAnimationState( ItemBubbleUI.State.Hover );
					HOVER_OBJECT = new DragObject( _inventory, itemBubble.Index );
				};

				itemBubble.PointerExit += () => {
					itemBubble.SetAnimationState( ItemBubbleUI.State.Default );
				};

				itemBubble.PointerDown += () => {
					DRAG_OBJECT = HOVER_OBJECT;
				};

				itemBubble.PointerUp += () => {
					Inventory.MoveItem( DRAG_OBJECT, HOVER_OBJECT );
				};
			}
		}
		private void SetItemBubble( int index, InventoryItem item ){

			if (index > _itemBubbles.Length || _itemBubbles[ index ] == null){
				return;
			}

			var itemBubble = _itemBubbles[ index ];
			itemBubble.SetItem( item );
		}

		// *******************************

		protected abstract Inventory GetInventory ();
		protected abstract ItemBubbleUI[] GetItemBubbles ();

		// *******************************

		public struct DragObject {

			public Inventory Inventory{get;}
			public int Index{get;}

			public DragObject( Inventory inventory, int index ){
				Inventory = inventory;
				Index = index;
			}
		}
	}
}