using UnityEngine;
using Interactable;
using Dumpster.Core.BuiltInModules.Input;

public class PlayerInteractor : Interactor, IInputReciever<Eden.Input.Package> {

	void IInputReciever<Eden.Input.Package>.RecieveInput ( Eden.Input.Package package ) {
		if( package.ConfirmUp ) { Use(); }
	}
	void IInputReciever<Eden.Input.Package>.EnteredInputFocus () {}
	void IInputReciever<Eden.Input.Package>.ExitInputFocus () {}

	private Player _player {
		get{ return EdensGarden.Instance.Rooms.CurrentArea.LoadedPlayer.GetComponent<Player>(); }		
	}


	[SerializeField] protected InteractorPostion _interactorPositionPrefab;

	private void Use () {

		// use the item
		var canUseItem = GetCanUseItem( _currentItem, _interactable );
		if ( !_inAction && canUseItem ) {
			_inAction = true;
			_currentItem.Use( _creature, () => _inAction = false );
		}
	}
	private void Update () {

		_interactable = GetInteractableObject();

		// change state in interactor position
		var canUseItem = GetCanUseItem( _currentItem, _interactable );
		var state = GetState( canUseItem );
		var tracking = GetTracking( _currentItem, _interactable );

		_interactorPositionInstance.ChangeState( state );
		_interactorPositionInstance.ChangeTracking( tracking );

	}
	private void Start () {

		EdensGarden.Instance.Input.RegisterToInputLayer( EdensGarden.Constants.InputLayers.Player, this );
		
		_interactorPositionInstance = Instantiate( _interactorPositionPrefab );
		_interactorPositionInstance.transform.position = transform.position;
		
		// listen for quickslot changes
		_player.QuickSlot.OnInputChanged += newId => {
			var index = _player.QuickslotInventory.ConvertQuickSlotIDToIndex( newId );
			_currentItem = _player.QuickslotInventory.GetInventoryItem( index );
		};
	}

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
				   item.CanPlant && interactable.Plantable )) {
						return InteractorPostion.Tracking.Interactable;
		} else {
			return InteractorPostion.Tracking.Player;
		}
	}
}
