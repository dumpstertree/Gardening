using System.Collections.Generic;
using UnityEngine;
using Eden.Model;
using Eden.Interactable;
using Eden.Model.Life;

namespace Eden.Life.Chips {

	[RequireComponent( typeof( Collider ) )]
	public class InteractorChip : Dumpster.Core.Life.Chip<Visual> {



		// *********************** Public ************************

		public void Use ( Item item ) {

			if ( !_inAction ) {

				_inAction = true;
				item.Use( this, EndAction );
			}
		}

		public RangedWeaponChip RangedWeaponChip {
			get{ return _rangedWeaponChip; }
		}

		public InteractableObject GetInteractableObject ( Item item ) {

			// all valid interactables
			var validInteractables = new List<InteractableObject>();
			
			for( int i = 0; i<_interactableObjectStack.Count; i++ ){

				var currentInteractable = _interactableObjectStack[i];
				var valid = false;

				if ( item != null && currentInteractable != null ){

					if ( item.IsActionable && currentInteractable.Actionable ) {

						valid = true;
					}
				}

				if ( valid ){
					validInteractables.Add( currentInteractable );
				}
			}
		
			// sort by distance
			InteractableObject closest = null;
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


		// *********************** Private ************************

		[SerializeField] private RangedWeaponChip _rangedWeaponChip;

		private List<InteractableObject> _interactableObjectStack;
		private bool _inAction;


		// chip
		protected override void Init () {

			_interactableObjectStack = new List<Eden.Interactable.InteractableObject>();
		}
		
		// mono
		private void OnTriggerEnter ( Collider collider ) {
			
			var interactable = collider.GetComponentInChildren<InteractableObject>();
			if ( interactable != null ) {
				_interactableObjectStack.Add( interactable );
			}
		}
		private void OnTriggerExit ( Collider collider ) {

			_interactableObjectStack.Remove( collider.GetComponentInChildren<InteractableObject>() );
		}
		private void EndAction () {

			_inAction = false;
		}
	}
}