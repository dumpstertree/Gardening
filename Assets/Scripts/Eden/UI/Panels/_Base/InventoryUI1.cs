using UnityEngine;
using UnityEngine.EventSystems;
using Eden.Controller;
using Eden.Model;

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

		protected override void OnPresent () {
			
			base.OnPresent();
			
			Clear();
			Load();

			EventSystem.current.SetSelectedGameObject( _itemBubbles[ 0 ].gameObject );
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
				var ib = itemBubble;
				SetItemBubble( index, _inventory.GetInventoryItem( index ) );

				itemBubble.OnSelect += () => {
					HOVER_OBJECT = new DragObject( _inventory, itemBubble.Index );
				};

				itemBubble.OnClick += () => {

					if ( DRAG_OBJECT == null ) {
						if ( ib.HasItem ) {
							DRAG_OBJECT = HOVER_OBJECT; 
							return;
						}
					} else {
						Inventory.MoveItem( DRAG_OBJECT, HOVER_OBJECT );
						DRAG_OBJECT = null;
					}
				};
			}
		}
		private void SetItemBubble( int index, Item item ){

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

		public class DragObject {

			public Inventory Inventory{get;}
			public int Index{get;}

			public DragObject( Inventory inventory, int index ){
				Inventory = inventory;
				Index = index;
			}
		}
	}
}