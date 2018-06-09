using UnityEngine;
using Interactable;

public class PlayerInteractor : Interactor, IInputReciever {

	// Input Events
	void IInputReciever.OnConfirmDown (){}
	void IInputReciever.OnConfirmUp (){
		
		Use();
	}
	void IInputReciever.OnCancelDown (){}
	void IInputReciever.OnCancelUp (){}
	void IInputReciever.OnStartDown (){}
	void IInputReciever.OnStartUp (){}
	void IInputReciever.HorizontalChanged ( float horizontal ) {}
	void IInputReciever.VerticalChanged ( float vertical ) {}


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

		_interactorPositionInstance = Instantiate( _interactorPositionPrefab );
		_interactorPositionInstance.transform.position = transform.position;
		
		// listen for quickslot changes
		// var player =  _creature as Player;
		// if ( player != null ) {
			
		// 	Game.Area.LoadedPlayer.QuickSlot.OnInputChanged += newId => {
		// 		var index = Game.Area.LoadedPlayer.QuickslotInventory.ConvertQuickSlotIDToIndex( newId );
		// 		_currentItem = Game.Area.LoadedPlayer.QuickslotInventory.GetInventoryItem( index );
		// 	};
		// }
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
