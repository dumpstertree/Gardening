using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels {
	
	public class GunCrafting : UiPanel {


		// ********** PUBLIC *****************
		
		public void SetItemToEdit ( InventoryItem itemToEdit ) {
			
			_itemBeingEdited = itemToEdit;

			if ( itemToEdit != null ) {
				
				// clear old gun
				_partGraphSubpanel.Clear();
				
				// reload parts list
				_gunPartsSubpanel.Reload();
				
				// foreach part in this new items gun add it to the graph
				foreach( Model.Gun.Part p in itemToEdit._shootData.Gun.WeaponParts ) {
					_partGraphSubpanel.AddPartToGraph( p );
				}


				UpdateGunStats( itemToEdit._shootData.Gun );
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
				
				Debug.LogWarning( "crafting currently broken here" );
				// _gunRef.UpdateModel( newGun );
				UpdateGunStats ( newGun );
			};
		}

		protected override void OnPresent () {
			
			Game.Area.LoadedPlayer.GunParts.OnPartListChanged += _gunPartsSubpanel.Reload;

			_gunPartsSubpanel.OnDragBegin += part => {

				Game.Area.LoadedPlayer.GunParts.RemovePart( part );				
				_partGraphSubpanel.AddPartToGraph( part, true );
			};
		}
		protected override void OnDismiss () {

			Game.Area.LoadedPlayer.GunParts.OnPartListChanged -= _gunPartsSubpanel.Reload;

			_gunPartsSubpanel.OnDragBegin -= part => {

				Game.Area.LoadedPlayer.GunParts.RemovePart( part );				
				_partGraphSubpanel.AddPartToGraph( part, true );
			};
		}

		[SerializeField] private Text FireRateText;
		[SerializeField] private Text ReloadTimeText;
		[SerializeField] private Text BulletSpeedText;
		[SerializeField] private Text ClipSizeText;
		[SerializeField] private Text NumberOfBulletsText;
		
		private void UpdateGunStats( Model.Gun gun )  {

			FireRateText.text = gun.WeaponStats.FireRate.ToString();
			ReloadTimeText.text = gun.WeaponStats.ReloadTime.ToString();
			BulletSpeedText.text = gun.WeaponStats.BulletSpeed.ToString();
			ClipSizeText.text = gun.WeaponStats.ClipSize.ToString();
			NumberOfBulletsText.text = gun.WeaponStats.NumberOfBullets.ToString();
		}
	}
}


