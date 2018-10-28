using UnityEngine;
using Dumpster.Core;
using Dumpster.BuiltInModules;
using Eden.Life;
using Eden.UI.Elements;
using Eden.Modules;

namespace Eden.UI.Panels {
	
	public class PlayerInventoryUI : InventoryUI {

		[SerializeField] private ItemSlot[] _inventorySlots;
		[SerializeField] private ItemSlot[] _equipedItemsSlots;

		private BlackBox _blackBox {
			get{ return Game.GetModule<Navigation>()?.CurrentArea.LoadedPlayer.GetComponent<BlackBox>(); }
		}

		protected override void OnInit () {

			base.OnInit ();

			RegisterInventory( _blackBox.Inventory, _inventorySlots );
			RegisterInventory( _blackBox.EquipedItems, _equipedItemsSlots );
		}
		
		public override void ReciveInput( Input.Package package ) {

			if ( package.Face.Right_Down ) {
				
				Game.GetModule<Dumpster.BuiltInModules.UI>().Dismiss (
					
					Game.GetModule<Constants>().UILayers.Midground, 
					Game.GetModule<Constants>().UIContexts.Inventory 
				);
			}
		}
	}
}