using System.Collections.Generic;
using UnityEngine;
using Eden.Model;

namespace Eden.Life.Chip {

	[RequireComponent( typeof( Collider ) )]
	public class InteractorChip : MonoBehaviour {

		public Interactable.InteractableObject InteractableObject {
			get{ return _interactable; }
		}
		
		[SerializeField] private Eden.Life.BlackBox _blackBox;

		private List<Eden.Interactable.InteractableObject> _interactableObjectStack;
		private Item _currentItem;
		private Eden.Interactable.InteractableObject _interactable;
		private bool _inAction;

		private void Start () {

			_interactableObjectStack = new List<Eden.Interactable.InteractableObject>();
			if( _blackBox.QuickslotChip ) _blackBox.QuickslotChip.OnInputChanged += OnItemChanged;

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

		private void OnItemChanged ( int index ) {

			_currentItem = _blackBox.EquipedItems.GetInventoryItem( index );
		}
		private void OnTriggerEnter ( Collider collider ) {
			
			var interactable = collider.GetComponentInChildren<Eden.Interactable.InteractableObject>();
			if ( interactable != null ) {
				_interactableObjectStack.Add( interactable );
			}
		}
		private void OnTriggerExit ( Collider collider ) {

			_interactableObjectStack.Remove( collider.GetComponentInChildren<Eden.Interactable.InteractableObject>() );
		}

		
		// ******************************************

		protected bool CanUseItem ( Item inventoryItem, Eden.Interactable.InteractableObject interactableItem ){

			if ( inventoryItem == null ){
				return false;
			}

			if ( inventoryItem.IsShootable ){
				return true;
			}
				
			if ( inventoryItem != null && interactableItem != null ){

				if ( inventoryItem.IsActionable && interactableItem.Actionable) {

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

					if ( _currentItem.IsActionable && currentInteractable.Actionable ) {

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