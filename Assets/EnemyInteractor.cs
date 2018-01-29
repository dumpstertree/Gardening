using UnityEngine;

public class EnemyInteractor : Interactor {

	public void Use () {

		// get interactable
		_interactable = GetInteractableObject();


		// use the item
		var canUseItem = GetCanUseItem( _currentItem, _interactable );

		if ( !_inAction && canUseItem ) {
			_inAction = true;
			_currentItem.Use( _creature, () => _inAction = false );
		}
	}

	private void Start () {

		var enemy = _creature as Enemy;
		
		if ( enemy != null ) {

			var index = _creature.QuickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Center );
			_currentItem = _creature.QuickslotInventory.GetInventoryItem( index );
		}
	}
}
