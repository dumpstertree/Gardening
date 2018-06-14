using UnityEngine;
using UnityEngine.UI;
using Dumpster.Core.BuiltInModules.UI;

namespace Eden.UI.Panels {
	
	public class MenuButtons1 : Panel {

		[SerializeField] private Button _inventoryButton;
		[SerializeField] private Button _craftingButton;
		[SerializeField] private Button _gunCrafting;

		protected override void OnInit() {
			
			_inventoryButton.onClick.AddListener( () => {
				EdensGarden.Instance.UI.Present( EdensGarden.Constants.NewUILayers.Midground, EdensGarden.Constants.UIContexts.Inventory );
			});
		}
	}
}