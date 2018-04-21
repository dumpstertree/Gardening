using System.Collections.Generic;
using UnityEngine;
using Interactable;

public class Interactor : MonoBehaviour {

	// ***************** PUBLIC *******************

	public InteractableObject InteractableObject { 
		get{ return _interactable; }
	}

	// ***************** PRIVATE ******************* 

	[SerializeField] protected Creature _creature;

	protected List<InteractableObject> _interactableObjectStack;
	protected InteractorPostion _interactorPositionInstance;
	protected InteractableObject _interactable;
	protected InventoryItem _currentItem;
	protected bool _inAction;


	// ******************************************

	private void Awake () {

		// init list
		_interactableObjectStack = new List<InteractableObject>();
	}
	private void OnTriggerEnter ( Collider collider ) {
		
		_interactableObjectStack.Add( collider.GetComponent<InteractableObject>() );
	}
	private void OnTriggerExit ( Collider collider ) {

		_interactableObjectStack.Remove( collider.GetComponent<InteractableObject>() );
	}


	// ******************************************

	protected bool GetCanUseItem ( InventoryItem inventoryItem, InteractableObject interactableItem ){

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
	protected InteractableObject GetInteractableObject () {

		// all valid interactables
		var validInteractables = new List<InteractableObject>();
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
		InteractableObject closest = null;
		var shortestLength = Mathf.Infinity;
		foreach( InteractableObject interactable in validInteractables ){

			var distance = Vector3.Distance( interactable.transform.position, transform.position );  

			if ( distance < shortestLength ){
				closest = interactable;
				shortestLength = distance;
			}
		}

		return closest;
	}
}
