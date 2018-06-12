using UnityEngine;
using UnityEngine.UI;

namespace Eden.UI.Panels {
	
	public class MenuButtons : UiPanel {

		[SerializeField] private Button _inventoryButton;
		[SerializeField] private Button _craftingButton;
		[SerializeField] private Button _gunCrafting;

		protected override void OnInit() {
			
			_inventoryButton.onClick.AddListener( () => {
				Game.UIController.ChangeContext( UIController.UiContext.Identifier.Inventory );
			});
			
			_craftingButton.onClick.AddListener( () => {
				Game.UIController.ChangeContext( UIController.UiContext.Identifier.Crafting );
			});
			
			_gunCrafting.onClick.AddListener( () => {
				Game.UIController.ChangeContext( UIController.UiContext.Identifier.GunCrafting );
			});
		}
	}
}