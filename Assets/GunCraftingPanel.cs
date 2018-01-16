using UnityEngine;
using UnityEngine.UI;

public class GunCraftingPanel : UiPanel {

	public void SetItemToEdit ( InventoryItem itemToEdit ) {
		
		_itemBeingEdited = itemToEdit;

		if ( itemToEdit != null && itemToEdit._shootData.CraftedGun != null ) {
			
			// clear old gun
			_partGraphSubpanel.Clear();
			
			// reload parts list
			_gunPartsSubpanel.Reload();
			
			// foreach part in this new items gun add it to the graph
			foreach( CraftedGun.Component c in itemToEdit._shootData.CraftedGun.GunComponents ) {
				_partGraphSubpanel.AddPartToGraph( c );
			}	
		}
	}

	// *****************************

	[SerializeField] private Button _exitButton;
	[SerializeField] private UI.GunCrafting.Subpanel.PartGraph _partGraphSubpanel;
	[SerializeField] private GunParts _gunPartsSubpanel;

	private InventoryItem _itemBeingEdited;

	// *****************************

	private void Awake () {

		_exitButton.onClick.AddListener( () => {
			Game.UIController.ChangeContext( UIController.UiContext.Identifier.Farm );
		});

		_partGraphSubpanel.CraftedGunChanged += newGun => {
			_itemBeingEdited._shootData.CraftedGun = newGun;
		};
		
		// init
		_gunPartsSubpanel.OnDragBegin += part => {
			
			Game.Area.LoadedPlayer.GunParts.Remove( part );
			
			_partGraphSubpanel.AddPartToGraph( part, true );
			_gunPartsSubpanel.Reload();
		};
	}
}



