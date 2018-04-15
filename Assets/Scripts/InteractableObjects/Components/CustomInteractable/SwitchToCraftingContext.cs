using System.Collections.Generic;
using Model.Dialog;

namespace Interactable.Component {

	public class SwitchToCraftingContext : Interactable {

		public override void Interact( Creature user, InventoryItem item ) {

			var seq =  new Sequence(
				
				new List<Sequence.Dialog>() {
					
					new Sequence.Dialog( SpeakerInfo.ZeroTwo(), "Hey, nice to meet you, wanna craft some good ass shit?" ),
					new Sequence.Dialog( SpeakerInfo.Player(), "Oh helllllll yea, lets do it!" ),
					new Sequence.Dialog( SpeakerInfo.ZeroTwo(), "Dope!" ),
					new Sequence.Dialog( SpeakerInfo.ZeroTwo(), "Hope you brought some good supplies!" ),
					new Sequence.Dialog( SpeakerInfo.ZeroTwo(), "God knows i didnt!" )
				} );
			
			Game.UIController.PresentDialog( seq, () => {
				Game.UIController.ChangeContext( UIController.UiContext.Identifier.Crafting );
			});
		}
	}
}