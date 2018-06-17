using UnityEngine;
using System.Collections.Generic;

namespace Interactable.Component {

	[RequireComponent(typeof(OptionalComponent.Destroyable))]
	public class GunPartPickup : Interactable {

		// ********* PUBLIC **************
		
		public override void Interact ( Creature user, InventoryItem item ) {
 			
 			var player = user as Player;
 			
 			if ( player != null ) {
  				player.GunParts.AddPart( _partToPickup );
				_destroyable.Destroy();
			}
		}

		// ********* PRIVATE **************

		[SerializeField] private List<GunPartTemplate> _partPrefabs;
		
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
				var prefab = Instantiate( _partPrefabs[ rand ] );

				_partToPickup = prefab.GetPart();
			};
		}
	}
}
