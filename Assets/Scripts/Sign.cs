namespace Interactable.Component {

	public class Sign : Interactable {

		public override void Interact( Creature user, InventoryItem item ) {
			
			Game.UIController.PresentDialog( Dialogs.Sign.Hype, () => {
				
				Game.UIController.ChangeContext( UIController.UiContext.Identifier.Farm );
			});
		}
	}
}