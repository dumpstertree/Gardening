using System.Collections.Generic;
using UnityEngine;
using Interactable;

public class Interactor : MonoBehaviour {

	// ***************** PUBLIC *******************

	public InteractableObject InteractableObject { 
		get{ return _interactable; }
	}

	// ***************** PRIVATE ******************* 

	[SerializeField] private InteractorPostion _interactorPositionPrefab;

	private List<InteractableObject> _interactableObjectStack;
	private InteractorPostion _interactorPositionInstance;
	private InteractableObject _interactable;
	private InventoryItem _currentItem;
	private bool _inAction;


	// ******************************************

	private void Awake () {

		// init list
		_interactableObjectStack = new List<InteractableObject>();
		
		// create new interactor position
		_interactorPositionInstance = Instantiate( _interactorPositionPrefab );
		_interactorPositionInstance.transform.position = transform.position;
	}
	private void Start () {

		// listen for quickslot changes
		Game.Area.LoadedPlayer.QuickSlot.OnInputChanged += newId => {
			var index = Game.Area.LoadedPlayer.QuickslotInventory.ConvertQuickSlotIDToIndex( newId );
			_currentItem = Game.Area.LoadedPlayer.QuickslotInventory.GetInventoryItem( index );
		};
	}
	private void Update () {

		// get interactable
		_interactable = GetInteractableObject();

		// use the item
		var canUseItem = GetCanUseItem( _currentItem, _interactable );
		if ( !_inAction && canUseItem && Input.GetKey(KeyCode.Space) ) {
			_inAction = true;
			_currentItem.Use( Game.Area.LoadedPlayer, () => _inAction = false );
		}

		// change state in interactor position
		var state = GetState( canUseItem );
		var tracking = GetTracking( _currentItem, _interactable );

		_interactorPositionInstance.ChangeState( state );
		_interactorPositionInstance.ChangeTracking( tracking );
	}
	private void OnTriggerEnter ( Collider collider ) {

		_interactableObjectStack.Add( collider.GetComponent<InteractableObject>() );
	}
	private void OnTriggerExit ( Collider collider ) {

		_interactableObjectStack.Remove( collider.GetComponent<InteractableObject>() );
	}

	// ******************************************

	private OrbPosition.State GetState ( bool canUseItem ) {
		
		if ( canUseItem ) {
			return OrbPosition.State.Excited;
		} else{ 
			return OrbPosition.State.Passive;
		}
	}
	private InteractorPostion.Tracking GetTracking ( InventoryItem item, InteractableObject interactable ) {
		
		if ( item == null ){
			return InteractorPostion.Tracking.Player;
		}
		else if ( item.CanPlace && interactable == null ){
			return InteractorPostion.Tracking.True;
		}
		else if ( (item != null && interactable != null) &&
				  (item.CanInteract && interactable.Interactable ||
				   item.CanHit && interactable.Hitable ||
				   item.CanPlant && interactable.Plantable ||
				   item.CanFeed && interactable.Feedable )) {
						return InteractorPostion.Tracking.Interactable;
		} else {
			return InteractorPostion.Tracking.Player;
		}
	}

	// ******************************************

	private bool GetCanUseItem ( InventoryItem inventoryItem, InteractableObject interactableItem ){

		if ( inventoryItem == null ){
			return false;
		}

		if ( inventoryItem.CanPlace && interactableItem == null ){
			return true;
		}

		if ( inventoryItem != null && interactableItem != null ){


			if ( inventoryItem.CanInteract && interactableItem.Interactable ||
				 inventoryItem.CanHit && interactableItem.Hitable ||
				 inventoryItem.CanPlant && interactableItem.Plantable ||
			 	 inventoryItem.CanFeed && interactableItem.Feedable ) {
					
				return true;
			}
		}

		return false;
	}
	private InteractableObject GetInteractableObject () {

		// all valid interactables
		var validInteractables = new List<InteractableObject>();
		for( int i = 0; i<_interactableObjectStack.Count; i++ ){

			var currentInteractable = _interactableObjectStack[i];
			var valid = false;

			if ( _currentItem != null && currentInteractable != null ){
				
				if ( _currentItem.CanInteract && currentInteractable.Interactable ||
					_currentItem.CanHit && currentInteractable.Hitable ||
					_currentItem.CanPlant && currentInteractable.Plantable ||
					_currentItem.CanFeed && currentInteractable.Feedable ) {

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
