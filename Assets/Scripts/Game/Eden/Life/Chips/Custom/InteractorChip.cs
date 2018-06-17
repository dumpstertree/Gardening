using System.Collections.Generic;
using UnityEngine;
using Interactable;

namespace Eden.Life.Chip {

	[RequireComponent( typeof( Collider ) )]
	public class InteractorChip : MonoBehaviour {
		
		[SerializeField] private Eden.Life.Brain.BlackBoxBrain _blackBox;

		private List<Eden.Interactable.InteractableObject> _interactableObjectStack;
		private InventoryItem _currentItem;
		private Eden.Interactable.InteractableObject _interactable;
		private bool _inAction;

		private void Start () {

			_interactableObjectStack = new List<Eden.Interactable.InteractableObject>();
			_blackBox.QuickslotChip.OnInputChanged += OnItemChanged;

			_currentItem = _blackBox.EquipedItems.GetInventoryItem( 0 ); // TODO this is placeholder
		}
		public void Use () {

			// get interactable
			_interactable = GetInteractableObject();

			// use the item
			var canUseItem = CanUseItem( _currentItem, _interactable );

			if ( !_inAction && canUseItem ) {
				_inAction = true;
				_currentItem.Use( _blackBox, _interactable, () => _inAction = false );
			}
		}

		private void OnItemChanged ( QuickSlotInventory.ID id ) {
		}
		private void OnTriggerEnter ( Collider collider ) {
		
			_interactableObjectStack.Add( collider.GetComponent<Eden.Interactable.InteractableObject>() );
		}
		private void OnTriggerExit ( Collider collider ) {

			_interactableObjectStack.Remove( collider.GetComponent<Eden.Interactable.InteractableObject>() );
		}

		
		// ******************************************

		protected bool CanUseItem ( InventoryItem inventoryItem, Eden.Interactable.InteractableObject interactableItem ){

			if ( inventoryItem == null ){
				return false;
			}

			if ( inventoryItem.CanShoot ){
				return true;
			}

			if ( inventoryItem.CanPlace && interactableItem == null ){
				return true;
			}

			if ( inventoryItem != null && interactableItem != null ){


				if ( inventoryItem.CanInteract && interactableItem.Interactable ||
					 inventoryItem.CanHit && interactableItem.Hitable ||
					 inventoryItem.CanPlant && interactableItem.Plantable ) {
						
					return true;
				}
			}

			return false;
		}
		protected Eden.Interactable.InteractableObject GetInteractableObject () {

			// all valid interactables
			var validInteractables = new List<Eden.Interactable.InteractableObject>();
			for( int i = 0; i<_interactableObjectStack.Count; i++ ){

				var currentInteractable = _interactableObjectStack[i];
				var valid = false;

				if ( _currentItem != null && currentInteractable != null ){
					
					if ( _currentItem.CanInteract && currentInteractable.Interactable ||
						_currentItem.CanHit && currentInteractable.Hitable ||
						_currentItem.CanPlant && currentInteractable.Plantable ) {

						valid = true;
					}
				}

				if ( valid ){
					validInteractables.Add( currentInteractable );
				}
			}
		
			// sort by distance
			Eden.Interactable.InteractableObject closest = null;
			var shortestLength = Mathf.Infinity;
			foreach( Eden.Interactable.InteractableObject interactable in validInteractables ){

				var distance = Vector3.Distance( interactable.transform.position, transform.position );  

				if ( distance < shortestLength ){
					closest = interactable;
					shortestLength = distance;
				}
			}

			return closest;
		}
	}
}