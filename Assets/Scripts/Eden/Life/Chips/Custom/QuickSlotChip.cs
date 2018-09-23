using Dumpster.Core.Life;
using UnityEngine;

namespace Eden.Life.Chips {
	
	public class QuickSlotChip: Chip<Eden.Model.Life.Visual> {


		public int EquipedIndex {
			get; set;
		}


		public void ShiftLeft () {

			var blackBox = BlackBox as Eden.Life.BlackBox;
			EquipedIndex = Mathf.RoundToInt( Mathf.Repeat( (float)EquipedIndex - 1, (float)blackBox.EquipedItems.InventoryCount ) );
		}
		public void ShiftRight () {

			var blackBox = BlackBox as Eden.Life.BlackBox;
			EquipedIndex = Mathf.RoundToInt( Mathf.Repeat( (float)EquipedIndex + 1, (float)blackBox.EquipedItems.InventoryCount  ) );
		}
	}
}