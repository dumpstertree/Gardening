using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;

namespace Eden.Characteristics {

	public class DropInventory : Dumpster.Characteristics.NotificationResponder {

		[SerializeField] private Actor _itemToDropPrefab;

		protected override void Respond () {
			
			DropItems ();
		}

		private void DropItems () {

			var inventory = _actor.GetCharacteristic<Inventory>().Inv;
			for ( int i=0; i< inventory.InventoryCount; i++ ) {
				
				// get item from inventory
				var item = inventory.GetInventoryItem( i );
				if ( item != null ) {
					
					// create a new drop
					var dropInst = Instantiate( _itemToDropPrefab );
					dropInst.GetCharacteristic<Inventory>( true ).Inv.AddInventoryItem( item );
					
					// set transform
					dropInst.transform.position = transform.position;

					// remove from inventory
					inventory.SetInventoryItem( i, null );
				}
			}
		}
	}
}