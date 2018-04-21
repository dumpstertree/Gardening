namespace Interactable.Component {

	public class SwitchToCraftingContext : Interactable {

		public override void Interact( Creature user, InventoryItem item ) {
			
			Game.UIController.PresentDialog( Dialogs.NPC.Crafting.Hello, () => {
				
				Game.UIController.ChangeContext( UIController.UiContext.Identifier.Crafting, () => {

					Game.UIController.PresentDialog( Dialogs.NPC.Crafting.Goodbye, () => {

						Game.UIController.ChangeContext( UIController.UiContext.Identifier.Farm );
					});
				});
			});
		}
	}
}