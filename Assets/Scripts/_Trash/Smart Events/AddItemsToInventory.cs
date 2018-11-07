using UnityEngine;
using Dumpster.Events;

namespace Eden.Events {
	
	// public class AddItemsToInventory : MonoBehaviour {

	// 	[SerializeField] private Eden.Interactable.Actionable _actionable;
	// 	[SerializeField] DropItem _dropItem;
	// 	[SerializeField] private SmartEvent[] _onAddItemToInventory;

	// 	private void Awake () {

	// 		_actionable.OnAction += HandleRecieveAction;
	// 	}
	// 	private void HandleRecieveAction( Eden.Life.BlackBox user ) {
			
	// 		user.Inventory.AddInventoryItem( _dropItem.Item );
	// 		FireAddItemToInventoryEvent ();
	// 	}
	// 	private void FireAddItemToInventoryEvent () {
			
	// 		foreach ( SmartEvent e in _onAddItemToInventory ) {
	// 			e.EventTriggered();
	// 		}
	// 	}
	// }
}