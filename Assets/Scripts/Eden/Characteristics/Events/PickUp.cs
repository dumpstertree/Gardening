using Dumpster.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Characteristics {

	public class PickUp : Dumpster.Core.Characteristic {

		public void Pickup ( Actor otherActor ) {

			var inventory = _actor.GetCharacteristic<Inventory>( true ).Inv;
			var otherInventory = otherActor.GetCharacteristic<Inventory>( true ).Inv;

			for ( int i=0; i<inventory.InventoryCount; i++ ) {
				
				// get item from inventory
				var item = inventory.GetInventoryItem( i );
				if ( item != null ) {

					
					// remove from old inventory
					inventory.SetInventoryItem( i, null );
					
					// add to new inventory
					otherInventory.AddInventoryItem( item );
				}
			}

			_actor.DestroyActor();
		}
	}
}