
namespace Eden.Life.Chips {

	public class UIChip : Dumpster.Core.Life.Chip<Eden.Model.Life.Visual> {
	
		public void OpenInventory () {

			EdensGarden.Instance.UI.Present( 
				
				EdensGarden.Constants.NewUILayers.Midground, 
				EdensGarden.Constants.UIContexts.Inventory 
			);
		}
	}
}