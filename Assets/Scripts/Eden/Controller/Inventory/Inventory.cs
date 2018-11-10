using UnityEngine;
using Eden.Model;

namespace Eden.Controller {
	
	[System.Serializable]
	public class Inventory: ISerializationCallbackReceiver {

	 
		// ***************** PUBLIC  *******************

		public Inventory ( int inventoryCount ) {
			
			_inventoryCount = inventoryCount;
			_inventoryItems = new Item[ _inventoryCount ];
		}
		

		// ***************** Cloning  *******************
		
		public Inventory Clone () {

	        return new Inventory( this );
	    }
		protected Inventory( Inventory objectToClone ) {
	    	
	    	_inventoryCount = objectToClone._inventoryCount;
	    	_inventoryItems = objectToClone._inventoryItems.Clone() as Item[];
	    }

		
		// *******************************************

		public delegate void OnInventoryItemChangedEvent( int index, Item item );
		public OnInventoryItemChangedEvent OnInventoryItemChanged;

		
		public int InventoryCount {
			get { return _inventoryCount; }
		}


		public Item GetInventoryItem( int index ){

			if ( index < _inventoryItems.Length ) {
				return _inventoryItems[ index ];
			}

			return null;
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

		
		// public static void MoveItem( InventoryUI1.DragObject fromObject, InventoryUI1.DragObject toObject ){

		// 	var fromItem = fromObject.Inventory.GetInventoryItem( fromObject.Index );
		// 	var toItem = toObject.Inventory.GetInventoryItem( toObject.Index );

		// 	if ( fromItem != null ) {
		// 		fromItem.OnCountChanged = null;
		// 	}
		// 	if ( toItem != null ) {
		// 		toItem.OnCountChanged = null;
		// 	}

		// 	fromObject.Inventory.SetInventoryItem( fromObject.Index, toItem );
		// 	toObject.Inventory.SetInventoryItem( toObject.Index, fromItem );
		// }

		
		// ***************** PRIVATE *******************

		[SerializeField] private int _inventoryCount;
		[SerializeField] protected Item[] _inventoryItems;

	
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

		public void OnAfterDeserialize () {}
		public void OnBeforeSerialize () {}

	}
}