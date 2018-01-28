namespace Interactable.Component {

	public class SwitchToCraftingContext : Interactable {

		public override void Interact( Creature user, InventoryItem item ){

			Game.UIController.ChangeContext( UIController.UiContext.Identifier.Crafting );
		}
	}
}