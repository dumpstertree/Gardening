using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;

namespace Eden.Characteristics {
	
	public class EquipedItemSwapper : Characteristic {


		public int EquipedIndex {
			get; set;
		}


		public void ShiftLeft () {

			var equipedItemInventory = _actor.GetCharacteristic<EquippedItemsInventory>();
			if ( equipedItemInventory != null ) {
				EquipedIndex = Mathf.RoundToInt( Mathf.Repeat( (float)EquipedIndex - 1, (float)equipedItemInventory.NumOfItem  ) );
			}
		}
		public void ShiftRight () {

			var equipedItemInventory = _actor.GetCharacteristic<EquippedItemsInventory>();
			if ( equipedItemInventory != null ) {
				EquipedIndex = Mathf.RoundToInt( Mathf.Repeat( (float)EquipedIndex + 1, (float)equipedItemInventory.NumOfItem  ) );
			}
		}
	}
}
