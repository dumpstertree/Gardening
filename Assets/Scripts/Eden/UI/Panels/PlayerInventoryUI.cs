using UnityEngine;
using Eden.Life;
using Eden.UI.Elements;

namespace Eden.UI.Panels {
	
	public class PlayerInventoryUI : InventoryUI {

		[SerializeField] private ItemSlot[] _inventorySlots;
		[SerializeField] private ItemSlot[] _equipedItemsSlots;

		private BlackBox _blackBox {
			get{ return EdensGarden.Instance.Rooms.CurrentArea.LoadedPlayer.GetComponent<BlackBox>(); }
		}

		protected override void OnInit () {

			base.OnInit ();

			RegisterInventory( _blackBox.Inventory, _inventorySlots );
			RegisterInventory( _blackBox.EquipedItems, _equipedItemsSlots );
		}
		
		public override void ReciveInput( Input.Package package ) {

			if ( package.Face.Right_Down ) {
				
				EdensGarden.Instance.UI.Dismiss (
					
					EdensGarden.Constants.NewUILayers.Midground, 
					EdensGarden.Constants.UIContexts.Inventory 
				);
			}
		}
	}
}