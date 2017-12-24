namespace Interactable.Component {

	public class SwitchToCraftingContext : Interactable {

		public override void Interact( Player player, InventoryItem item ){

			Game.UIController.ChangeContext( UIController.UiContext.Identifier.Crafting );
		}
	}
}