using Dumpster.Core;
using Dumpster.BuiltInModules;
using Eden.Modules;

namespace Eden.Life.Chips {

	public class UIChip : Dumpster.Core.Life.Chip<Eden.Model.Life.Visual> {
	
		public void OpenInventory () {
			
			Game.GetModule<Dumpster.BuiltInModules.UI>().Present( 
				
				Game.GetModule<Constants>().UILayers.Midground,
				Game.GetModule<Constants>().UIContexts.Inventory
			);
		}
	}
}