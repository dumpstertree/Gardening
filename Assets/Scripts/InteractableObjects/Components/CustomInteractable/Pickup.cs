using UnityEngine;

namespace Interactable.Component {

	[RequireComponent(typeof(OptionalComponent.Destroyable))]
	public class Pickup : Interactable {

		// ********* PUBLIC **************

		public InventoryItem Item {
			get{ return _item; }
			set{ _item = value; }
		}

		public override void Interact ( Player player, InventoryItem item ) {

			if ( player.QuickslotInventory.AddInventoryItem( Item ) || player.Inventory.AddInventoryItem( Item ) ){

				_destroyable.Destroy();
			}
			else{

				Game.Effects.OneShot( Application.Effects.Type.Hit, transform.position, transform.rotation );
			}
		}

		// ********* PRIVATE **************

		[SerializeField] private InventoryItem _item;
		private OptionalComponent.Destroyable _destroyable;

		// ******************************

		private void Start () {

			if( _item != null) {
			  _item = Game.ItemManager.RequestItem( _item.name );
			};
			
			_destroyable = GetComponent<OptionalComponent.Destroyable>();
		}
	}
}
