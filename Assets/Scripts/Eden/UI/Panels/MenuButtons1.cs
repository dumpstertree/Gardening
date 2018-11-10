using Dumpster.BuiltInModules;
using Dumpster.Core;
using Eden.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Eden.UI.Panels {
	
	public class MenuButtons1 : Panel {

		[SerializeField] private Button _inventoryButton;
		[SerializeField] private Button _craftingButton;
		[SerializeField] private Button _gunCrafting;

		protected override void OnInit() {
			
			_inventoryButton.onClick.AddListener( () => {
				
				Game.GetModule<Dumpster.BuiltInModules.UI>().Present( 
					Game.GetModule<Constants>().UILayers.Midground, 
					Game.GetModule<Constants>().UIContexts.Inventory 
				);
			});
		}
	}
}