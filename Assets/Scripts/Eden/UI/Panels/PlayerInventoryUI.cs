using UnityEngine;
using Dumpster.Core;
using Dumpster.BuiltInModules;
using Eden.UI.Elements;
using Eden.Modules;
using Eden.Characteristics;

namespace Eden.UI.Panels {
	
	public class PlayerInventoryUI : InventoryUI {

		[SerializeField] private ItemSlot[] _inventorySlots;
		[SerializeField] private ItemSlot[] _equipedItemsSlots;

		private Actor _actor {
			get{ return Game.GetModule<Navigation>()?.CurrentArea.LoadedPlayer.GetComponent<Actor>(); }
		}

		protected override void OnInit () {

			base.OnInit ();

			RegisterInventory( _actor.GetCharacteristic<Inventory>().Inv, _inventorySlots );
			RegisterInventory( _actor.GetCharacteristic<EquippedItemsInventory>().Inventory, _equipedItemsSlots );
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