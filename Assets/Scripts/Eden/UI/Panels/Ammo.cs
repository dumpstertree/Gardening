using UnityEngine;
using UnityEngine.UI;
using Dumpster.Core.BuiltInModules.UI;

namespace Eden.UI.Panels {

	public class Ammo : Panel {

		[SerializeField] private Text _availableAmmo;
		[SerializeField] private Text _maxAmmo;
		[SerializeField] private Image _reloadTime;

		private InventoryItem _item;

		private Eden.Life.BlackBoxes.Player _player {
			get{ return EdensGarden.Instance.Rooms.CurrentArea.LoadedPlayer.GetComponent<Eden.Life.BlackBoxes.Player>(); }
		}


		protected override void OnInit () {

			base.OnInit();
			
			_player.QuickslotChip.OnInputChanged += index => { 

				var item = _player.EquipedItems.GetInventoryItem( index );

				// remove all old connectionss
				if ( _item != null ) {

					item._shootData.OnAvailableBulletsChange -= newCount => { UpdateAvailableAmmo( newCount ); };
					item._shootData.OnReloadTimeChanged -= (currentReloadTime, reloadTime ) => { UpdateReloadTime( currentReloadTime, reloadTime ); };
				}


				_item = null;

				if ( item != null && item.CanShoot ) {
					
					_item = item;

					// update available ammo
					UpdateAvailableAmmo( item._shootData.AvailableBullets );
					item._shootData.OnAvailableBulletsChange += newCount => { UpdateAvailableAmmo( newCount ); };
					item._shootData.OnReloadTimeChanged += (currentReloadTime, reloadTime ) => { UpdateReloadTime( currentReloadTime, reloadTime ); };

					// update max ammo
					var gunStats = new GunDataController().LoadGun( _item.ID ).WeaponStats;
					UpdateMaxAmmo( gunStats.ClipSize );
				} 
				else {

					// hide all ammo
					Hide();
				}
			};
		}
		
		private void UpdateMaxAmmo ( int amount ) {

			_maxAmmo.color = new Color( 1,1,1,1 );
			_maxAmmo.text = amount.ToString();
		}
		private void UpdateAvailableAmmo ( int amount ) {
			
			_availableAmmo.color = new Color( 1,1,1,1 );
			_availableAmmo.text = amount.ToString();
		}
		private void UpdateReloadTime ( float currentReloadTime, float reloadTime ) {

			_reloadTime.fillAmount = currentReloadTime/reloadTime;
		}
		private void Hide () {
			
			_availableAmmo.color = new Color( 1,1,1,0 );
			_maxAmmo.color = new Color( 1,1,1,0 );
		}
	}
}
