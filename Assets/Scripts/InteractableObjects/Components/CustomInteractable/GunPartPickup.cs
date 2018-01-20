using UnityEngine;
using System.Collections.Generic;

namespace Interactable.Component {

	[RequireComponent(typeof(OptionalComponent.Destroyable))]
	public class GunPartPickup : Interactable {

		// ********* PUBLIC **************
		
		public override void Interact ( Player player, InventoryItem item ) {
 			
 			player.GunParts.AddPart( _partToPickup );
			_destroyable.Destroy();
		}

		// ********* PRIVATE **************

		[SerializeField] private List<GameObject> _partPrefabs;
		
		private Model.Gun.Part _partToPickup;
		private OptionalComponent.Destroyable _destroyable;

		// ******************************

		private void Awake () {
			
			// get components
			_destroyable = GetComponent<OptionalComponent.Destroyable>();
		}
		private void Start () {
			
			if ( _partToPickup == null) {
				
				var rand = Random.Range( 0, _partPrefabs.Count );
				var prefab = _partPrefabs[ rand ];
				_partToPickup = new Model.Gun.Part( prefab.GetComponent<UI.Elements.GunCrafting.Part>() );
			};
		}
	}
}
