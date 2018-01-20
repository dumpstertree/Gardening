using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels {
	
	public class GunCrafting : UiPanel {


		// ********** PUBLIC *****************
		
		public void SetItemToEdit ( InventoryItem itemToEdit ) {
			
			_itemBeingEdited = itemToEdit;

			if ( itemToEdit != null && itemToEdit._shootData.CraftedGun != null ) {
				
				// clear old gun
				_partGraphSubpanel.Clear();
				
				// reload parts list
				_gunPartsSubpanel.Reload();
				
				// foreach part in this new items gun add it to the graph
				foreach( Model.Gun.Part c in itemToEdit._shootData.CraftedGun.WeaponParts ) {
					_partGraphSubpanel.AddPartToGraph( c );
				}	
			}
		}

		// ************ PRIVATE ***************

		// buttons
		[SerializeField] private Button _exitButton;

		// subpanels
		[SerializeField] private UI.Subpanels.GunCrafting.PartGraph _partGraphSubpanel;
		[SerializeField] private UI.Subpanels.GunCrafting.GunParts _gunPartsSubpanel;

		private InventoryItem _itemBeingEdited;

		// ************************************

		private void Awake () {

			_exitButton.onClick.AddListener( () => {
				Game.UIController.ChangeContext( UIController.UiContext.Identifier.Farm );
			});

			_partGraphSubpanel.CraftedGunChanged += newGun => {
				_itemBeingEdited._shootData.CraftedGun = newGun;
			};
		}

		protected override void OnPresent () {
			
			Game.Area.LoadedPlayer.GunParts.OnPartListChanged += _gunPartsSubpanel.Reload;

			_gunPartsSubpanel.OnDragBegin += part => {

				Game.Area.LoadedPlayer.GunParts.RemovePart( part );				
				_partGraphSubpanel.AddPartToGraph( part, true );
			};
		}
	}
}


