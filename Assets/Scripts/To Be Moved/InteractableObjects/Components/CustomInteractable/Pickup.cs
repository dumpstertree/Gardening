using UnityEngine;
using Dumpster.Core.BuiltInModules.Effects;

namespace Interactable.Component {

	[RequireComponent(typeof(OptionalComponent.Destroyable))]
	public class Pickup : Interactable {

		// ********* PUBLIC **************

		public InventoryItem Item {
			get{ return _item; }
			set{ _item = value; }
		}

		public override void Interact ( Creature user, InventoryItem item ) {

			if ( user.QuickslotInventory.AddInventoryItem( _item ) || user.Inventory.AddInventoryItem( _item ) ){

				_destroyable.Destroy();
			}
			else{

				EdensGarden.Instance.Effects.OneShot( ParticleType.Hit, transform.position, transform.rotation );
			}
		}

		// ********* PRIVATE **************

		[SerializeField] private Model.Template.InventoryItemTemplate _itemTemplate;
		[SerializeField] private int _itemStartCount;
		
		private InventoryItem _item;
		private OptionalComponent.Destroyable _destroyable;

		// ******************************

		private void Start () {

			if ( _itemTemplate != null) { _item = _itemTemplate.GetInstance( _itemStartCount ); };

			_destroyable = GetComponent<OptionalComponent.Destroyable>();
		}
	}
}
