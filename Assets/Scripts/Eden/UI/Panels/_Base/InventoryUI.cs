using System.Collections.Generic;
using UnityEngine;

namespace Eden.UI.Elements {
	
	public abstract class InventoryUI : InteractivePanel  {


		// **************** Protected ********************
		
		protected override void OnInit () {

			_registeredSlots =  new List<RegisteredItemSlot>();
		}
		protected override void OnPresent () {

			Reload ();
			UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject( _firstSelection.gameObject );
		}
		protected override void OnDismiss () {
			
			if ( _itemBeingDragged != null ) {
				GiveSlotItem( _slotDragStart, _itemBeingDragged );
			}
		}


		protected void RegisterInventory ( Eden.Controller.Inventory inventory, ItemSlot[] slots ) {

			for ( int i=0; i<slots.Length; i++ ) {

				var slot = slots[ i ];
				var index = i;
				var inv = inventory;
				var registeredSlot = new RegisteredItemSlot( slot, index, inv );


				// listen for click event
				slot.OnClick += () => {
					SlotClicked( registeredSlot );
				};
				

				// listen for select event
				slot.OnSelect += () => {
					SlotSelected( registeredSlot );
				};


				// register the slot for later
				_registeredSlots.Add( registeredSlot );


				// set the state
				SetState( registeredSlot );
			}
		}


		// ***************** Private ********************


		[SerializeField] private Item _itemPrefab;
		[SerializeField] private Transform _content;
		[SerializeField] private ItemSlot _firstSelection;


		readonly private Vector3 _draggedItemScale = new Vector3( 0.7f, 0.7f, 0.7f );
		readonly private Vector3 _draggedItemOffset = new Vector3( 40f, 40f, 0f );

		private List<RegisteredItemSlot> _registeredSlots;
		private RegisteredItemSlot _currentlyOverSlot;
		private RegisteredItemSlot _slotDragStart;
		private Item _itemBeingDragged;


		private void Update () {

			if ( _itemBeingDragged != null ) {
				
				var startPos = _itemBeingDragged.transform.position;
				var targetPos = _currentlyOverSlot.Slot.transform.position + _draggedItemOffset;
				_itemBeingDragged.transform.position = Vector3.Lerp( startPos, targetPos, 0.5f );

				var startScale = _itemBeingDragged.transform.localScale;
				var targetScale = _draggedItemScale;
				_itemBeingDragged.transform.localScale = Vector3.Lerp( startScale, targetScale, 0.5f );
			}
		}
		private void Reload () {

			foreach ( RegisteredItemSlot slot in _registeredSlots ) {
				
				var modelItem  = slot.Inventory.GetInventoryItem( slot.Index );
				var hasModelItem = modelItem != null;


				// Make sure the model is the same
				if ( hasModelItem && slot.Slot.HasItemInSlot ) {
					slot.Slot.ItemInSlot.SetBackingItem( modelItem );
				}

				
				// Add an item
				if ( hasModelItem && !slot.Slot.HasItemInSlot ) {
					slot.Slot.ItemInSlot = CreateItem( modelItem, slot.Slot.transform.position, Vector3.one );
				}


				// Remove item
				if ( !hasModelItem && slot.Slot.HasItemInSlot ) {
					Destroy( slot.Slot.ItemInSlot.gameObject );
				}
			}
		}
		private void SlotSelected ( RegisteredItemSlot slot ) {
			
			var lastInSlot = _currentlyOverSlot;

			_currentlyOverSlot = slot;
			
			SetState( _currentlyOverSlot );
			SetState( lastInSlot );
		}
		private void SlotClicked ( RegisteredItemSlot slot ) {
			

			if ( _itemBeingDragged == null ) {

				if ( slot.Slot.HasItemInSlot ) {

					// save items for slot
					_slotDragStart = slot;

					// set the item as now being dragged
					_itemBeingDragged = TakeSlotItem( _slotDragStart );

					// change state of slot
					SetState( _slotDragStart );
				}
			} else {
				
				// move the second slot item to the first slot
				var currentlyOverSlotItem = TakeSlotItem( _currentlyOverSlot );

				
				if ( _currentlyOverSlot != _slotDragStart ) {
					
					// swap items
					GiveSlotItem( _currentlyOverSlot, _itemBeingDragged );
					GiveSlotItem( _slotDragStart, currentlyOverSlotItem );
				
				} else {

					// put item back
					GiveSlotItem( _currentlyOverSlot, _itemBeingDragged );
				}


				// change state of slot	
				var wasDragStart = _slotDragStart;


				// clear
				_slotDragStart = null;
				_itemBeingDragged = null;


				// change state of slot
				SetState( wasDragStart );
			}
		}
		private void SetState ( RegisteredItemSlot itemSlot ) {

			if ( itemSlot == null ) {
				return;
			}

			if ( itemSlot == _slotDragStart ) {
				itemSlot.Slot.SetSlotState( ItemSlot.State.DragStart );
				return;
			}

			if ( itemSlot == _currentlyOverSlot ) {
				itemSlot.Slot.SetSlotState( ItemSlot.State.Selected );
				return;
			}

			itemSlot.Slot.SetSlotState( ItemSlot.State.Default );
		}
		private void GiveSlotItem ( RegisteredItemSlot itemSlot, Item item ) {

			// Set UI
			itemSlot.Slot.ItemInSlot = item;

			// Set Model
			itemSlot.Inventory.SetInventoryItem( itemSlot.Index, ( item != null ) ? item.BackingItem : null );
		}
		private Item TakeSlotItem ( RegisteredItemSlot itemSlot ) {

			if ( itemSlot.Slot.HasItemInSlot ) {

				var item = itemSlot.Slot.ItemInSlot;

				// Set UI
				itemSlot.Slot.ItemInSlot = null;

				// Set Model 
				itemSlot.Inventory.SetInventoryItem( itemSlot.Index, null );

				return item;
			}

			return null;
		}
		private Item CreateItem ( Eden.Model.Item item, Vector3 startPos, Vector3 startScale ) {

			var inst = Instantiate( _itemPrefab );
			inst.transform.SetParent( _content, false );
			inst.transform.localScale = startScale;
			inst.transform.position = startPos;
			inst.SetBackingItem( item );
			return inst;
		}


		private class RegisteredItemSlot {

			public ItemSlot Slot;
			public Eden.Controller.Inventory Inventory;
			public int Index;

			public RegisteredItemSlot( ItemSlot slot, int index, Eden.Controller.Inventory inventory ) {
			
				Slot = slot;
				Inventory = inventory;
				Index = index;
			}
		}
	}
}